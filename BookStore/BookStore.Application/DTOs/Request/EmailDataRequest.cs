namespace BookStore.Application.DTOs.Request
{
    public class EmailDataRequest
    {
        public string EmailTo { get; set; } = default!;
        public SendBookInfoDto Book { get; set; } = default!;
    }
}