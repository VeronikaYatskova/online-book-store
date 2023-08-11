namespace BookStore.Application.Services.CloudServices.Azurite.Models
{
    public class BlobStorageSettings
    {
        public string ConnectionString { get; set; } = default!;
        public string RequestedBooksContainerName { get; set; } = default!; 
        public string PublishedBooksContainerName { get; set; } = default!; 
    }
}
