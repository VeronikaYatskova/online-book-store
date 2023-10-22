using Microsoft.AspNetCore.Http;

namespace Requests.BLL.DTOs.Requests
{
    public class AddRequestDto
    {
        public string PublisherId { get; set; } = default!;
        public IFormFile File { get; set; } = default!;
        public IFormFile? BookCoverFile { get; set; }
    }
}
