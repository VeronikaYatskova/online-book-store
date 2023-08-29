using Microsoft.AspNetCore.Http;
using Requests.BLL.DTOs.General;

namespace Requests.BLL.Services.Interfaces
{
    public interface IBlobStorageService
    {
        Task<IEnumerable<Blob>> GetAllAsync();
        Task<string?> GetBlobByNameAsync(string blobName);
        Task<BlobManipulation> UploadAsync(IFormFile blob, string toContainer, string? fileFakeName = null);
    }
}
