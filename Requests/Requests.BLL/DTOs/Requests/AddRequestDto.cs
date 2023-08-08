namespace Requests.BLL.DTOs.Requests
{
    public class AddRequestDto
    {
        public string UserId { get; set; } = default!;
        public string PublisherId { get; set; } = default!;
        public string BookFakeName { get; set; } = default!;
    }
}
