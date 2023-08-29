namespace Requests.DAL.Models
{
    public class BlobStorageSettings
    {
        public string ConnectionString { get; set; } = default!;
        public string RequestsContainerName { get; set; } = default!;
        public string BookCoversContainerName { get; set; } = default!;
    }
}