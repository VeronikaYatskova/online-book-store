using BookStore.Services.CloudServices.Amazon.Models;

namespace BookStore.Application.Services.CloudServices.Amazon.Models
{
    public class UploadFileModel
    {
        public S3ObjectModel S3obj { get; set; } = default!;
        public AwsCredentials AwsCredentials { get; set; } = default!;
        public string ClientUrl { get; set; } = default!;
    }
}
