namespace BookStore.Application.Services.CloudServices.Azurite.Models
{
    public class BlobResponse
    {
        public int StatusCode { get; set; }  
        public Blob Blob { get; set; } = default!;    
    }
}