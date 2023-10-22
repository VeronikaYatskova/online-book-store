using BookStore.Services.CloudServices.Amazon.Models;

namespace BookStore.Application.Services.CloudServices.Amazon.Models
{
    public class DeleteFileModel
    {
        public AwsCredentials AwsCredentials { get; set;} = default!;
        public string ClientUrl { get; set;} = default!;
        public string BucketName { get; set;} = default!;
    }
}
