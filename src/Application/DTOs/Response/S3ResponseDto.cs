namespace Application.DTOs.Response
{
    public class S3ResponseDto
    {
        public string BookFakeName { get; set; }
        public int StatusCode { get; set; } = 200;
        public string Message { get; set; } = default!;
    }
}