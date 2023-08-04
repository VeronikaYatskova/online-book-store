namespace BookStore.Application.DTOs.Request
{
    public class BookCommentDto
    {
        public string BookId { get; set; } = default!;
        public string UserId { get; set; } = default!;
        public string Text { get; set; } = default!;
    }
}
