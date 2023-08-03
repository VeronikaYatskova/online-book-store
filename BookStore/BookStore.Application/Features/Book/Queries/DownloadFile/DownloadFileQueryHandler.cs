using BookStore.Application.Abstractions.Contracts.Interfaces;
using MediatR;

namespace BookStore.Application.Features.Book.Queries.DownloadFile
{
    public class DownloadFileQueryHandler : IRequestHandler<DownloadFileQuery, byte[]>
    {
        private readonly IAwsS3Service _awsS3Service;

        public DownloadFileQueryHandler(IAwsS3Service awsS3Service)
        {
            _awsS3Service = awsS3Service;
        }

        public async Task<byte[]> Handle(DownloadFileQuery request, CancellationToken cancellationToken)
        {
            return await _awsS3Service.DownloadFileFromAwsAsync(request);
        }
    }
}
