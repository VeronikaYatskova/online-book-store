using BookStore.Application.Services.CloudServices.Azurite.Models;
using Microsoft.AspNetCore.Http;

namespace BookStore.Application.Abstractions.Contracts.Interfaces
{
    public interface IAzureService
    {
        Task<BlobResponse> UploadAsync(IFormFile blob, string containerName, string? fileFakeName = null);
        Task<byte[]> DownloadAsync(string blobFileName);
        Task DeleteAsync(string blobFileName);
        Task<IEnumerable<Blob>> GetAllAsync();
        Task CopyFileAsync(string fileName);
        Task<Blob?> GetBlobByAsync(Func<Blob, bool> expression);
    }
}
