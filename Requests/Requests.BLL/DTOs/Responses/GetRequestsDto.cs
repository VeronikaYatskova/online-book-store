namespace Requests.BLL.DTOs.Responses
{
    public class GetRequestsDto
    {
        public string Id { get; set; } = default!;
        public string UserId { get; set; } = default!;
        public string PublisherId { get; set; } = default!;
        public string BookFakeName { get; set; } = default!;
        public string? BookCoverFakeName { get; set; }
        public bool IsApproved { get; set; }
    }
}
