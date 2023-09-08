using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using BookStore.Application.Abstractions.Contracts.Interfaces;
using BookStore.Application.DTOs.Response;
using BookStore.Application.Services.CloudServices.Azurite.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BookStore.Application.Services.CloudServices.Azurite
{
    public class AzureService : IAzureService
    {
        private readonly BlobStorageSettings _blobStorageSettings;
        private readonly ILogger<AzureService> _logger;

        public AzureService(IOptions<BlobStorageSettings> blobStorageSettings, ILogger<AzureService> logger)
        {
            _blobStorageSettings = blobStorageSettings.Value;
            _logger = logger;
        }

        public async Task<IEnumerable<Blob>> GetAllAsync()
        {
            var blobServiceClient = new BlobServiceClient(_blobStorageSettings.ConnectionString);
            var container = blobServiceClient
                .GetBlobContainerClient(_blobStorageSettings.PublishedBooksContainerName);
            
            await container.CreateIfNotExistsAsync();

            var files = new List<Blob>();

            await foreach(var file in container.GetBlobsAsync())
            {
                var uri = container.Uri.ToString();
                var name = file.Name;
                
                var fullUri = $"{uri}/{name}";

                files.Add(new Blob
                {
                    Uri = fullUri,
                    Name = name,
                    ContentType = file.Properties.ContentType,
                });
            }

            return files;
        }

        public async Task<string?> GetBlobByNameAsync(string blobName, string fromContainer)
        {
            var container = new BlobContainerClient(
                _blobStorageSettings.ConnectionString, 
                fromContainer);

            await container.SetAccessPolicyAsync(PublicAccessType.BlobContainer);

            var blob = container.GetBlobClient(blobName);
            return blob.Uri.AbsoluteUri;
        }

        public async Task<byte[]> DownloadAsync(string blobFilename)
        {
            var client = new BlobContainerClient(
                _blobStorageSettings.ConnectionString, 
                _blobStorageSettings.PublishedBooksContainerName);
                
            var file = client.GetBlobClient(blobFilename);

            if (await file.ExistsAsync())
            {
                var data = await file.OpenReadAsync();
                var blobContent = data;

                using var ms = new MemoryStream();
                await file.DownloadToAsync(ms);
                
                return ms.ToArray();
            }
            
            throw new FileNotFoundException();
        }

        public async Task<BlobResponse> UploadAsync(IFormFile blob, string toContainer, string? fileFakeName = null)
        {
            var container = new BlobContainerClient(
                _blobStorageSettings.ConnectionString, 
                toContainer);

            await container.CreateIfNotExistsAsync();

            var fileName = fileFakeName is null ? blob.FileName : fileFakeName;  
            var client = container.GetBlobClient(fileName);

            await using Stream? data = blob.OpenReadStream();
            await client.UploadAsync(data);
            
            return new BlobResponse
            {
                StatusCode = 200,
                Blob = new Blob
                {
                    Uri = client.Uri.AbsoluteUri,
                    Name = client.Name,
                }
            };
        }

        public async Task DeleteAsync(string blobFileName)
        {
            var client = new BlobContainerClient(
                _blobStorageSettings.ConnectionString, 
                _blobStorageSettings.RequestedBooksContainerName);

            var file = client.GetBlobClient(blobFileName);

            await file.DeleteAsync();
        }

        public async Task CopyFileAsync(string fileName, string fromContainer, string toContainer)
        {
            var requestedBooksContainer = new BlobContainerClient(
                _blobStorageSettings.ConnectionString, 
                fromContainer);

            var publishedBooksContainer = new BlobContainerClient(
                _blobStorageSettings.ConnectionString, 
                toContainer);

            await publishedBooksContainer.CreateIfNotExistsAsync();

            var file = requestedBooksContainer.GetBlobClient(fileName);
            var publishedFile = publishedBooksContainer.GetBlobClient(fileName);

            await publishedFile.StartCopyFromUriAsync(file.Uri);
            
            await DeleteAsync(fileName);
        }

        public async Task LoadRelatedData(IEnumerable<BookDto> books)
        {
            foreach (var book in books)
            {
                var blobUrl = await GetBlobByNameAsync(
                    book.BookFakeName, 
                    _blobStorageSettings.PublishedBooksContainerName);

                if (book.BookCoverFakeName is not null)
                {
                    var BookCoverFakeName = await GetBlobByNameAsync(
                        book.BookCoverFakeName, 
                        _blobStorageSettings.BookCoversContainerName);

                     book.BookCoverFakeName = BookCoverFakeName!;
                }

                if (blobUrl is not null)
                {
                    book.FileURL = blobUrl!;
                }
            }
        }
    }
}
