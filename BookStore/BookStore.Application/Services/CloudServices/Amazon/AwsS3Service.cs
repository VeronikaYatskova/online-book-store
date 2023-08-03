using System.Net;
using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using BookStore.Application.Abstractions.Contracts.Interfaces;
using BookStore.Application.DTOs;
using BookStore.Application.DTOs.Response;
using BookStore.Application.Features.Book.Queries.DownloadFile;
using BookStore.Application.Services.CloudServices.Amazon.Models;
using BookStore.Services.CloudServices.Amazon.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace BookStore.Application.Services.CloudServices.Amazon
{
    public class AwsS3Service : IAwsS3Service
    {
        private readonly AwsDataWithClientUrl _awsDataWithClientUrl;
        private readonly IOptions<MinioConfiguration> _minioConfiguration;
        private readonly RegionEndpoint _regionEndpoint = RegionEndpoint.EUWest2;

        public AwsS3Service(IOptions<MinioConfiguration> minioConfiguration)
        {
            _minioConfiguration = minioConfiguration;
            
            var cred = new AwsCredentials()
            {
                AwsKey = minioConfiguration.Value.MinioAccessKey,
                AwsSecretKey = minioConfiguration.Value.MinioSecretKey,
            };

            var bucketName = minioConfiguration.Value.BucketName;
            var clientUrl = minioConfiguration.Value.ClientUrl;

            _awsDataWithClientUrl = new AwsDataWithClientUrl()
            {
                AwsCredentials = cred,
                BucketName = bucketName,
                ClientUrl = clientUrl,
            };
        }

        public string GetFilePreSignedUrl(string fileName)
        {            
            var credentials = new BasicAWSCredentials(
                _awsDataWithClientUrl.AwsCredentials.AwsKey,
                _awsDataWithClientUrl.AwsCredentials.AwsSecretKey);

            var awsS3Config = new AmazonS3Config()
            {
                RegionEndpoint = _regionEndpoint,
                ForcePathStyle = true,
                ServiceURL = _awsDataWithClientUrl.ClientUrl,
            };

            using var client = new AmazonS3Client(credentials, awsS3Config);
            var bucketName = _awsDataWithClientUrl.BucketName;

            var urlRequest = new GetPreSignedUrlRequest()
            {
                BucketName = bucketName,
                Key = fileName,
                Expires = DateTime.UtcNow.AddMinutes(5)
            };

            return client.GetPreSignedURL(urlRequest);
        }

        public async Task<byte[]> DownloadFileFromAwsAsync(DownloadFileQuery request)
        {
            MemoryStream ms = null!;

            GetObjectRequest getObjectRequest = new GetObjectRequest
            {
                BucketName = _awsDataWithClientUrl.BucketName,
                Key = request.FileName,
            };
            
            var credentials = new BasicAWSCredentials(
                _awsDataWithClientUrl.AwsCredentials.AwsKey, 
                _awsDataWithClientUrl.AwsCredentials.AwsSecretKey);

            var awsS3Config = new AmazonS3Config()
            {
                RegionEndpoint = _regionEndpoint,
                ForcePathStyle = true,
                ServiceURL = _awsDataWithClientUrl.ClientUrl
            };
            
            using var client = new AmazonS3Client(credentials, awsS3Config);
            
            using var response = await client.GetObjectAsync(getObjectRequest);
            
            if (response.HttpStatusCode == HttpStatusCode.OK)
            {
                using (ms = new MemoryStream())
                {
                    await response.ResponseStream.CopyToAsync(ms);
                }
            }

            if (ms is null || ms.ToArray().Length < 1)
                throw new FileNotFoundException(string.Format("The document '{0}' is not found", request.FileName));

            return ms.ToArray();
        }

        public async Task DeleteFileFromCloudAsync(string fileName)
        {
            var credentials = new BasicAWSCredentials(
                _awsDataWithClientUrl.AwsCredentials.AwsKey, 
                _awsDataWithClientUrl.AwsCredentials.AwsSecretKey);

            var awsS3Config = new AmazonS3Config()
            {
                RegionEndpoint = _regionEndpoint,
                ForcePathStyle = true,
                ServiceURL = _awsDataWithClientUrl.ClientUrl!
            };

            try
            {
                using var client = new AmazonS3Client(credentials, awsS3Config);

                var transferUtility = new TransferUtility(client);

                await transferUtility.S3Client.DeleteObjectAsync(new DeleteObjectRequest()
                {
                    BucketName = _awsDataWithClientUrl.BucketName,
                    Key = fileName,
                });
            }
            catch(AmazonS3Exception ex)
            {
                throw new AmazonS3Exception(ex);
            }  
        }

        public async Task<S3ResponseDto> UploadFileToCloudAsync(UploadFileModel uploadFileModel)
        {
            var credentials = new BasicAWSCredentials(uploadFileModel.AwsCredentials.AwsKey, uploadFileModel.AwsCredentials.AwsSecretKey);

            var awsS3Config = new AmazonS3Config()
            {
                RegionEndpoint = _regionEndpoint,
                ForcePathStyle = true,
                ServiceURL = uploadFileModel.ClientUrl!
            };

            var response = new S3ResponseDto();

            var uploadRequest = new TransferUtilityUploadRequest()
            {
                InputStream = uploadFileModel.S3obj.InputStream,
                Key = uploadFileModel.S3obj.Name,
                BucketName = uploadFileModel.S3obj.BucketName,
                CannedACL = S3CannedACL.NoACL
            };

            using var client = new AmazonS3Client(credentials, awsS3Config);

            var transferUtility = new TransferUtility(client);

            await transferUtility.UploadAsync(uploadRequest);
            
            response.BookFakeName = uploadFileModel.S3obj.Name;
            response.StatusCode = 200;
            response.Message = $"{uploadFileModel.S3obj.Name} has been uploaded successfully";
            
            return response;
        }
    
        public async Task<S3ResponseDto> UploadFileToBucketAsync(IFormFile file)
        {
            await using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);

            string objName = CreateFileName(file);
            
            var clientUrl = _awsDataWithClientUrl.ClientUrl;

            BasicAWSCredentials credentials;
            AmazonS3Config awsS3Config;
            S3ObjectModel s3obj;
            TransferUtilityUploadRequest uploadRequest;

            SetModelsToUploadData();

            var response = new S3ResponseDto();

            using var client = new AmazonS3Client(credentials, awsS3Config);

            var transferUtility = new TransferUtility(client);

            await transferUtility.UploadAsync(uploadRequest);
            
            response.BookFakeName = s3obj.Name;
            response.StatusCode = 200;
            response.Message = $"{s3obj.Name} has been uploaded successfully";
            
            return response;

            void SetModelsToUploadData()
            {
                credentials = new BasicAWSCredentials(
                    _awsDataWithClientUrl.AwsCredentials.AwsKey, 
                    _awsDataWithClientUrl.AwsCredentials.AwsSecretKey);

                awsS3Config = new AmazonS3Config()
                {
                    RegionEndpoint = _regionEndpoint,
                    ForcePathStyle = true,
                    ServiceURL = _awsDataWithClientUrl.ClientUrl!
                };

                s3obj = new S3ObjectModel()
                {
                    BucketName = _awsDataWithClientUrl.BucketName!,
                    InputStream = memoryStream,
                    Name = objName
                };

                uploadRequest = new TransferUtilityUploadRequest()
                {
                    InputStream = s3obj.InputStream,
                    Key = s3obj.Name,
                    BucketName = s3obj.BucketName,
                    CannedACL = S3CannedACL.NoACL
                };
            }
        }

        private string CreateFileName(IFormFile file)
        {
            var fileExtension = Path.GetExtension(file.FileName);
            var objName = $"{Guid.NewGuid()}-{DateTime.Now.ToString("yyyy'-'MM'-'dd")}{fileExtension}";
            
            return objName;
        }
    }
}
