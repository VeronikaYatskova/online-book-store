using BookStore.Services.CloudServices.Amazon.Models;

namespace BookStore.Application.DTOs
{
    public class AwsData
    {
        public AwsCredentials AwsCredentials { get; set; } = default!;
        public string? FileName { get; set; } = default;
        public string? BucketName { get; set; } = default;
    }
}
