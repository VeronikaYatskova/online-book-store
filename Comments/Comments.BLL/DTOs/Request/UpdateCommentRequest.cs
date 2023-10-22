namespace Comments.BLL.DTOs.Request
{
    public class UpdateCommentRequest
    {
        public string Id { get; set; } = default!;
        public string Text { get; set; } = default!;
    }
}
