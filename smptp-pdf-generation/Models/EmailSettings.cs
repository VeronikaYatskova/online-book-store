namespace smptp_pdf_generation.Models
{
    public class EmailSettings
    {
        public string EmailServer { get; set; } = default!;
        public string EmailAddress { get; set; } = default!;
        public string EmailPassword { get; set; } = default!;
        public int Port { get; set; } = default!;
    }
}
