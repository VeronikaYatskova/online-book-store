namespace Requests.DAL.Models
{
    public class BlobStorageSettings
    {
        public string ConnectionString { get; set; } = default!;
        public string ContainerName { get; set; } = default!; 
    }
}