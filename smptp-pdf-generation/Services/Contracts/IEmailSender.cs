namespace smptp_pdf_generation.Services.Contracts
{
    public interface IEmailSender
    {
        void SendEmail(string toEmail, byte[] fileToSend);
    }
}
