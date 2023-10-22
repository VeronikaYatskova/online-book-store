namespace OnlineBookStore.Messages.Models.Messages
{
    public class RequestCreatedMessage
    {
        public string Id { get; set; } = default!;
        public string UserId { get; set; } = default!;
        public string PublisherEmail { get; set; } = default!;
        public bool IsApproved { get; set; } = false;
    }
}
