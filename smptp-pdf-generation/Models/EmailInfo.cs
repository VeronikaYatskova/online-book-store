namespace smptp_pdf_generation.Models
{
    public class EmailInfo
    {
        public string EmailTo { get; set; } = default!;
        public Book Book { get; set; } = default!;
    }
}
