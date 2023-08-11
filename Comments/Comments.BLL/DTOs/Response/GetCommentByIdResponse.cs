namespace Comments.BLL.DTOs.Response
{
    public class GetCommentByIdResponse
    {
        public string UserId { get; set; } = default!;
        public string Text { get; set; } = default!;
        public string Date { get; set; } = default!;
    }
}
