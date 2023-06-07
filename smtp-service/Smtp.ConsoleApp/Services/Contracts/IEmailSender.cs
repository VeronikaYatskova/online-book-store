namespace Smtp.ConsoleApp.Services.Contracts
{
    public interface IEmailSender
    {
        void SendEmail(string toEmail, byte[] fileToSend);
    }
}
