using BookStore.Application.Abstractions.Contracts.Interfaces;
using BookStore.Application.Services.CloudServices.Azurite.Models;
using MediatR;

namespace BookStore.Application.Features.Book.Queries.DownloadFile
{
    public class DownloadFileQueryHandler : IRequestHandler<DownloadFileQuery, byte[]>
    {
        private readonly IAzureService _azureService;

        public DownloadFileQueryHandler(IAzureService azureService)
        {
            _azureService = azureService;
        }

        public async Task<byte[]> Handle(DownloadFileQuery request, CancellationToken cancellationToken)
        {
            return await _azureService.DownloadAsync(request.FileName);
        }
    }
}
