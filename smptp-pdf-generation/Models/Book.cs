namespace smptp_pdf_generation.Models
{
    public class Book
    {
        public string BookName { get; set; } = default!;   
        public string[] BookAuthors { get; set; } = default!;
        public string PublishDate { get; set; } = default!;
        public string PublisherName { get; set; } = default!;
    }
}
