using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Requests.BLL.DTOs.General;
using Requests.DAL.Models;
using Requests.DAL.Repositories.Interfaces;

namespace Requests.DAL.Repositories.Implementations
{
    public class BlobStorageRepository : IBlobStorageRepository
    {
        private readonly BlobStorageSettings _blobStorageSettings;

        public BlobStorageRepository(IOptions<BlobStorageSettings> blobStorageSettings)
        {
            _blobStorageSettings = blobStorageSettings.Value;
        }

        public async Task<BlobManipulation> DeleteAsync(string blobFilename, string fromContainer)
        {
            BlobContainerClient client = new BlobContainerClient(
                _blobStorageSettings.ConnectionString, 
                fromContainer);

            BlobClient file = client.GetBlobClient(blobFilename);

            await file.DeleteAsync();

            return new BlobManipulation 
            { 
                Error = false, 
                Status = $"File: {blobFilename} has been successfully deleted." 
            };
        }

        public async Task<IEnumerable<Blob>> GetAllAsync(string fromContainer)
        {
            var blobServiceClient = new BlobServiceClient(_blobStorageSettings.ConnectionString);
            var container = blobServiceClient.GetBlobContainerClient(fromContainer);

            await container.CreateIfNotExistsAsync();
            await container.SetAccessPolicyAsync(Azure.Storage.Blobs.Models.PublicAccessType.BlobContainer);

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

        public async Task<BlobManipulation> UploadAsync(IFormFile blob, string toContainer, string? fileFakeName = null)
        {
            var container = new BlobContainerClient(
                _blobStorageSettings.ConnectionString, 
                toContainer);

            await container.CreateIfNotExistsAsync(PublicAccessType.BlobContainer);

            var fileName = fileFakeName is null ? blob.FileName : fileFakeName;  
            var client = container.GetBlobClient(fileName);

            await using Stream? data = blob.OpenReadStream();
            await client.UploadAsync(data);

            var response = new BlobManipulation
            {
                Status = $"File {blob.FileName} Uploaded Successfully",
                Error = false,
                Blob = new Blob
                {
                    Uri = client.Uri.AbsoluteUri,
                    Name = client.Name,
                }
            };

            return response;
        }
    }
}
