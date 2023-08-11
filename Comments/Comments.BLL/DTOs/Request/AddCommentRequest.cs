namespace Comments.BLL.DTOs.Request
{
    public class AddCommentRequest
    {
        public string BookId { get; set; } = default!;
        public string Text { get; set; } = default!;
    }
}
