namespace BookStore.Services.CloudServices.Amazon.Models
{
    public class S3ObjectModel
    {
        public string Name { get; set; } = default!;
        public MemoryStream InputStream { get; set; } = default!;
        public string BucketName { get; set; } = default!;
    }
}
