using BookStore.Application.Abstractions.Contracts.Interfaces;
using BookStore.Application.DTOs.Response;
using MediatR;

namespace BookStore.Application.Features.Book.Commands.UploadFile
{
    public class UploadFileCommandHandler : IRequestHandler<UploadFileCommand, S3ResponseDto>
    {
        private readonly IAwsS3Service _awsS3Service;

        public UploadFileCommandHandler(IAwsS3Service awsS3Service)
        {
            _awsS3Service = awsS3Service;
        }

        public async Task<S3ResponseDto> Handle(UploadFileCommand request, CancellationToken cancellationToken)
        {
            return await _awsS3Service.UploadFileToCloudAsync(request.UploadFileModel);
        }
    }
}
