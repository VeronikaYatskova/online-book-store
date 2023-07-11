using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Application.Abstractions.Contracts.Interfaces;
using Application.DTOs;
using Application.DTOs.Response;
using Application.Exceptions;
using AutoMapper;
using MediatR;
using Polly;

namespace Application.Features.Book.Queries.GetAllBooks
{
    public class GetAllBooksQueryHandler : IRequestHandler<GetAllBooksQuery, IEnumerable<BookDto>>
    {
        private readonly IUnitOfWork unitOfWork; 
        private readonly IMapper mapper; 
        
        public GetAllBooksQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public Task<IEnumerable<BookDto>> Handle(GetAllBooksQuery request, CancellationToken cancellationToken)
        {
            var policy = SetRetryPolicyAsync();

            var books = policy.ExecuteAsync(async () => 
                await unitOfWork.BooksRepository.GetAllAsync()).Result;
            
            if (books is null)
            {
                throw new NotFoundException(ExceptionMessages.BookListIsEmpty);
            }

            var booksDto = mapper.Map<IEnumerable<BookDto>>(books);
            
            var awsData = request.request;
            foreach (var book in booksDto)
            {

                book.FileURL = GetFilePreSignedUrl(awsData, book.BookFakeName);
            }

            return Task.FromResult(booksDto);
        }

        private string GetFilePreSignedUrl(AwsDataWithClientUrl awsData, string fileName)
        {            
            var credentials = new BasicAWSCredentials(awsData.AwsCredentials.AwsKey, awsData.AwsCredentials.AwsSecretKey);

            var awsS3Config = new AmazonS3Config()
            {
                RegionEndpoint = Amazon.RegionEndpoint.EUWest2,
                ForcePathStyle = true,
                ServiceURL = awsData.ClientUrl!
            };

            using var client = new AmazonS3Client(credentials, awsS3Config);
            var bucketName = awsData.BucketName;

            var urlRequest = new GetPreSignedUrlRequest()
            {
                BucketName = bucketName,
                Key = fileName,
                Expires = DateTime.UtcNow.AddMinutes(5)
            };

            var retryPolicy = SetRetryPolicy();

            return retryPolicy.Execute(() => {
                return client.GetPreSignedURL(urlRequest);
            });
        }

        private Polly.Retry.RetryPolicy SetRetryPolicy()
        {
            return Policy
                .Handle<AmazonS3Exception>()
                .WaitAndRetry(3, i => TimeSpan.FromSeconds(5), onRetry: (exception, retryCount) =>
                {
                    Console.WriteLine("Get Exception: " + exception + "... Retry count: " + retryCount);
                });
        }

        private Polly.Retry.AsyncRetryPolicy SetRetryPolicyAsync()
        {
            return Policy
                .Handle<AmazonS3Exception>()
                .WaitAndRetryAsync(0, i => TimeSpan.FromSeconds(5), onRetry: (exception, retryCount) =>
                {
                    Console.WriteLine("Get Exception: " + exception + "... Retry count: " + retryCount);
                });
        }
    }
}
