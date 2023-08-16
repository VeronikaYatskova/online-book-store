namespace OnlineBookStore.Messages.Models.Messages
{
    public class RequestUpdatedMessage
    {
        public string Id { get; set; } = default!;
        public string UserEmail { get; set; } = default!;
        public string PublisherEmail { get; set; } = default!;
        public bool IsApproved { get; set; } = false;
    }
}
