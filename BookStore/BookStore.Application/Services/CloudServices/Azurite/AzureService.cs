using Azure.Storage.Blobs;
using BookStore.Application.Abstractions.Contracts.Interfaces;
using BookStore.Application.Services.CloudServices.Azurite.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace BookStore.Application.Services.CloudServices.Azurite
{
    public class AzureService : IAzureService
    {
        private readonly BlobStorageSettings _blobStorageSettings;

        public AzureService(IOptions<BlobStorageSettings> blobStorageSettings)
        {
            _blobStorageSettings = blobStorageSettings.Value;
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

        public async Task<BlobResponse> UploadAsync(IFormFile blob, string? fileFakeName = null)
        {
            var container = new BlobContainerClient(
                _blobStorageSettings.ConnectionString, 
                _blobStorageSettings.PublishedBooksContainerName);

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
    }
}
