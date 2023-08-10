using Microsoft.AspNetCore.Http;
using Requests.BLL.DTOs.General;

namespace Requests.DAL.Repositories.Interfaces
{
    public interface IBlobStorageRepository
    {
        Task<IEnumerable<Blob>> GetAllAsync();
        Task<Blob?> GetBlobByNameAsync(string blobName);
        Task<BlobManipulation> UploadAsync(IFormFile blob, string? fileFakeName = null);
    }
}
