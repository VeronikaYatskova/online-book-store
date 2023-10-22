using BookStore.Application.DTOs.Response;
using BookStore.Application.Services.CloudServices.Azurite.Models;
using Microsoft.AspNetCore.Http;

namespace BookStore.Application.Abstractions.Contracts.Interfaces
{
    public interface IAzureService
    {
        Task<BlobResponse> UploadAsync(IFormFile blob, string toContainer, string? fileFakeName = null);
        Task<byte[]> DownloadAsync(string blobFileName);
        Task DeleteAsync(string blobFileName);
        Task<IEnumerable<Blob>> GetAllAsync();
        Task CopyFileAsync(string fileName, string fromContainer, string toContainer);
        Task<string?> GetBlobByNameAsync(string blobName, string fromContainer);
        Task LoadRelatedData(IEnumerable<BookDto> books);
    }
}
