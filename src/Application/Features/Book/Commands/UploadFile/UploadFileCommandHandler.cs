using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Transfer;
using Application.Abstractions.Contracts.Interfaces;
using Application.DTOs.Response;
using AutoMapper;
using MediatR;
using Polly;

namespace Application.Features.Book.Commands.UploadFile
{
    public class UploadFileCommandHandler : IRequestHandler<UploadFileCommand, S3ResponseDto>
    {
        private readonly IUnitOfWork unitOfWork; 
        private readonly IMapper mapper;

        public UploadFileCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<S3ResponseDto> Handle(UploadFileCommand request, CancellationToken cancellationToken)
        {
            var credentials = new BasicAWSCredentials(request.awsCred.AwsKey, request.awsCred.AwsSecretKey);

            var awsS3Config = new AmazonS3Config()
            {
                RegionEndpoint = Amazon.RegionEndpoint.EUWest2,
                ForcePathStyle = true,
                ServiceURL = request.clientUrl!
            };

            var response = new S3ResponseDto();

            var uploadRequest = new TransferUtilityUploadRequest()
            {
                InputStream = request.s3obj.InputStream,
                Key = request.s3obj.Name,
                BucketName = request.s3obj.BucketName,
                CannedACL = S3CannedACL.NoACL
            };

            using var client = new AmazonS3Client(credentials, awsS3Config);

            var transferUtility = new TransferUtility(client);

            var policy = SetRetryPolicy(response);
            await policy.ExecuteAsync(async () => 
                await transferUtility.UploadAsync(uploadRequest)
            );
            
            response.BookFakeName = request.s3obj.Name;
            response.StatusCode = 200;
            response.Message = $"{request.s3obj.Name} has been uploaded successfully";
            
            return response;
        }

        private Polly.Retry.AsyncRetryPolicy SetRetryPolicy(S3ResponseDto response)
        {
            return Policy
                .Handle<AmazonS3Exception>()
                .WaitAndRetryAsync(5, i => TimeSpan.FromSeconds(10), onRetry: (exception, retryCount) =>
                {
                    Console.WriteLine("Upload Exception: " + exception.Message + ".. Retry count: " + retryCount);
                });
        }
    }
}
