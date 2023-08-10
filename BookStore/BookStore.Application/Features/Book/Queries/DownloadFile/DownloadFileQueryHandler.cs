using System.Net;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using BookStore.Application.Abstractions.Contracts.Interfaces;
using AutoMapper;
using MediatR;

namespace BookStore.Application.Features.Book.Queries.DownloadFile
{
    public class DownloadFileQueryHandler : IRequestHandler<DownloadFileQuery, byte[]>
    {
        private readonly IUnitOfWork unitOfWork; 
        private readonly IMapper mapper;

        public DownloadFileQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }
        
        public async Task<byte[]> Handle(DownloadFileQuery request, CancellationToken cancellationToken)
        {
            MemoryStream ms = null!;
        
            GetObjectRequest getObjectRequest = new GetObjectRequest
            {
                BucketName = request.request.BucketName,
                Key = request.request.FileName
            };
            
            var credentials = new BasicAWSCredentials(
                request.request.AwsCredentials.AwsKey, 
                request.request.AwsCredentials.AwsSecretKey);

            var awsS3Config = new AmazonS3Config()
            {
                RegionEndpoint = Amazon.RegionEndpoint.EUWest2,
                ForcePathStyle = true,
                ServiceURL = request.request.ClientUrl
            };
            
            using var client = new AmazonS3Client(credentials, awsS3Config);
            
            using (var response = await client.GetObjectAsync(getObjectRequest))
            {
                if (response.HttpStatusCode == HttpStatusCode.OK)
                {
                    using (ms = new MemoryStream())
                    {
                        await response.ResponseStream.CopyToAsync(ms);
                    }
                }
            }

            if (ms is null || ms.ToArray().Length < 1)
                throw new FileNotFoundException(string.Format("The document '{0}' is not found", request.request.FileName));

            return ms.ToArray();
        }
    }
}
