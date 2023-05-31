using Microsoft.AspNetCore.Http;

namespace Application.DTOs.Request
{
    public class AddBookDto
    {
        public string BookName { get; set; } = default!;
        public string ISBN10 { get; set; } = default!;
        public string ISBN13 { get; set; } = default!;
        public int PagesCount { get; set; }
        public string? BookPictureURL { get; set; }
        public IFormFile File { get; set; } = default!;
        public string PublishYear { get; set; } = default!;
        public string Language { get; set; } = default!;
        public string CategotyGuid { get; set; } = default!;
        public string PublisherGuid { get; set; } = default!; 
        public string[] AuthorsGuid { get; set; } = default!;
    }
}
