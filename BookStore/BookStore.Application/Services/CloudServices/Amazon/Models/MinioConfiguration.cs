namespace BookStore.Application.Services.CloudServices.Amazon.Models
{
    public class MinioConfiguration
    {
        public string MinioAccessKey { get; set; } = default!;
        public string MinioSecretKey { get; set; } = default!;
        public string BucketName { get; set; } = default!;
        public string ClientUrl { get; set; } = default!;
    }
}