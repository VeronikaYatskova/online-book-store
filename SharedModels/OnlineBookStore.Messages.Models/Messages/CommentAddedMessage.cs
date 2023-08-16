namespace OnlineBookStore.Messages.Models.Messages
{
    public class CommentAddedMessage
    {
        public string BookId { get; set; } = default!;
        public string UserId { get; set; } = default!;
        public DateTime Date { get; set; } = default!;
        public string Text { get; set; } = default!;
    }
}
