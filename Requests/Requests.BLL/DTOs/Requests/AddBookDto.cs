using Microsoft.AspNetCore.Http;

namespace Requests.BLL.DTOs.Requests
{
    public class AddBookDto
    {
        public string BookName { get; set; } = default!;
        public string BookFakeName { get; set; } = default!;
        public string ISBN10 { get; set; } = default!;
        public string ISBN13 { get; set; } = default!;
        public int PagesCount { get; set; }
        public string? BookPictureURL { get; set; }
        public string PublishYear { get; set; } = default!;
        public string Language { get; set; } = default!;
        public string CategoryGuid { get; set; } = default!;
        public string PublisherGuid { get; set; } = default!; 
        public string[] AuthorsGuid { get; set; } = default!;
    }
}
