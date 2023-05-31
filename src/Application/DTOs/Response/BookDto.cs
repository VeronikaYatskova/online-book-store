namespace Application.DTOs.Response
{
    public class BookDto
    {
        public Guid BookGuid { get; set; } = default!;
        public string BookName { get; set; } = default!;
        public string BookFakeName { get; set; } = default!;
        public string ISBN10 { get; set; } = default!;
        public string ISBN13 { get; set; } = default!;
        public int PagesCount { get; set; }
        public string? BookPictureURL { get; set; }
        public string PublishYear { get; set; } = default!;
        public string Language { get; set; } = default!;
        public string FileURL { get; set; } = default!;
        public string Categoty { get; set; } = default!;
        public string Publisher { get; set; } = default!; 
        public string[] Authors { get; set; } = default!;
    }
}
