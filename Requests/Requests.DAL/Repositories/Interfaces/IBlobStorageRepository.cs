using Microsoft.AspNetCore.Http;
using Requests.BLL.DTOs.General;

namespace Requests.DAL.Repositories.Interfaces
{
    public interface IBlobStorageRepository
    {
        Task<IEnumerable<Blob>> GetAllAsync(string fromContainer);
        Task<string?> GetBlobByNameAsync(string blobName, string fromContainer);
        Task<BlobManipulation> UploadAsync(IFormFile blob, string toContainer, string? fileFakeName = null);
    }
}
