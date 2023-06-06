using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Application.Abstractions.Contracts.Interfaces;
using AutoMapper;
using MediatR;

namespace Application.Features.Book.Commands.DeleteBook
{
    public class DeleteBookCommandHandler : IRequestHandler<DeleteBookCommand>
    {
        private readonly IUnitOfWork unitOfWork; 
        private readonly IMapper mapper;

        public DeleteBookCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task Handle(DeleteBookCommand request, CancellationToken cancellationToken)
        {
            var bookId = new Guid(request.bookId);
            var bookToDelete = await unitOfWork.BooksRepository.GetByIdAsync(bookId);

            if (bookToDelete is null)
            {
                throw Exceptions.Exceptions.BookNotFound;
            }

            unitOfWork.BooksRepository.DeleteBook(bookToDelete);
            await unitOfWork.SaveChangesAsync();

            await DeleteFileFromCloud(request, bookToDelete.BookFakeName!);
        }

        private async Task DeleteFileFromCloud(DeleteBookCommand request, string fileName)
        {
            var credentials = new BasicAWSCredentials(request.awsCred.AwsKey, request.awsCred.AwsSecretKey);

            var awsS3Config = new AmazonS3Config()
            {
                RegionEndpoint = Amazon.RegionEndpoint.EUWest2,
                ForcePathStyle = true,
                ServiceURL = request.clientUrl!
            };

            try
            {
                using var client = new AmazonS3Client(credentials, awsS3Config);

                var transferUtility = new TransferUtility(client);

                await transferUtility.S3Client.DeleteObjectAsync(new DeleteObjectRequest()
                {
                    BucketName = request.bucketName,
                    Key = fileName,
                });
            }
            catch(AmazonS3Exception ex)
            {
                throw new AmazonS3Exception(ex);
            }  
        }
    }
}
