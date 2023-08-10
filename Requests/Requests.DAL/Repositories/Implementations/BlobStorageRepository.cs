using Azure.Storage.Blobs;
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

        public async Task<BlobManipulation> DeleteAsync(string blobFilename)
        {
            BlobContainerClient client = new BlobContainerClient(_blobStorageSettings.ConnectionString, _blobStorageSettings.ContainerName);

            BlobClient file = client.GetBlobClient(blobFilename);

            await file.DeleteAsync();

            return new BlobManipulation 
            { 
                Error = false, 
                Status = $"File: {blobFilename} has been successfully deleted." 
            };
        }

        public async Task<IEnumerable<Blob>> GetAllAsync()
        {
            var blobServiceClient = new BlobServiceClient(_blobStorageSettings.ConnectionString);
            var container = blobServiceClient.GetBlobContainerClient(_blobStorageSettings.ContainerName);
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
        
        public async Task<Blob?> GetBlobByNameAsync(string blobName)
        {
            var blobs = await GetAllAsync();

            return blobs.FirstOrDefault(x => x.Name == blobName);
        }

        public async Task<BlobManipulation> UploadAsync(IFormFile blob, string? fileFakeName = null)
        {
            var container = new BlobContainerClient(_blobStorageSettings.ConnectionString, _blobStorageSettings.ContainerName);

            await container.CreateIfNotExistsAsync();

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
