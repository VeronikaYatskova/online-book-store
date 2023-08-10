using Microsoft.AspNetCore.Http;
using Requests.BLL.DTOs.General;
using Requests.BLL.Services.Interfaces;
using Requests.DAL.Repositories.Interfaces;

namespace Requests.BLL.Services.Implementations
{
    public class BlobStorageService : IBlobStorageService
    {
        private readonly IBlobStorageRepository _blobStorageRepository;

        public BlobStorageService(IBlobStorageRepository blobStorageRepository)
        {
            _blobStorageRepository = blobStorageRepository;
        }

        public async Task<Blob?> GetBlobByNameAsync(string blobName)
        {
            return await _blobStorageRepository.GetBlobByNameAsync(blobName);
        }

        public async Task<IEnumerable<Blob>> GetAllAsync()
        {
            return await _blobStorageRepository.GetAllAsync();
        }

        public async Task<BlobManipulation> UploadAsync(IFormFile blob, string? fileFakeName = null)
        {
            return await _blobStorageRepository.UploadAsync(blob, fileFakeName);
        }
    }
}
