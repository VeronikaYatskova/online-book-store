using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Requests.BLL.DTOs.General;
using Requests.BLL.Services.Interfaces;
using Requests.DAL.Models;
using Requests.DAL.Repositories.Interfaces;

namespace Requests.BLL.Services.Implementations
{
    public class BlobStorageService : IBlobStorageService
    {
        private readonly IBlobStorageRepository _blobStorageRepository;
        private readonly BlobStorageSettings _blobStorageSettings;

        public BlobStorageService(
            IBlobStorageRepository blobStorageRepository, 
            IOptions<BlobStorageSettings> blobStorageSettings)
        {
            _blobStorageRepository = blobStorageRepository;
            _blobStorageSettings = blobStorageSettings.Value;
        }

        public async Task<string?> GetBlobByNameAsync(string blobName)
        {
            return await _blobStorageRepository.GetBlobByNameAsync(blobName, _blobStorageSettings.BookCoversContainerName);
        }

        public async Task<IEnumerable<Blob>> GetAllAsync()
        {
            return await _blobStorageRepository.GetAllAsync(_blobStorageSettings.RequestsContainerName);
        }

        public async Task<BlobManipulation> UploadAsync(IFormFile blob, string toContainer, string? fileFakeName = null)
        {
            return await _blobStorageRepository.UploadAsync(
                blob, 
                toContainer,
                fileFakeName);
        }
    }
}
