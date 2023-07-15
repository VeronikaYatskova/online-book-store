namespace BookStore.Domain.Entities
{
    public class S3Object
    {
        public string Name { get; set; } = default!;
        public MemoryStream InputStream { get; set; } = default!;
        public string BucketName { get; set; } = default!;
    }
}
