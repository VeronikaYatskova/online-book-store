namespace BookStore.Application.DTOs
{
    public class AwsDataWithClientUrl : AwsData
    {
        public string ClientUrl { get; set; } = default!;
    }
}
