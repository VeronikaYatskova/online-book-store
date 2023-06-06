using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Smtp.WebAPI.Models;
using Smtp.WebAPI.Services.Contracts;

namespace Smtp.WebAPI.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration config;
        private readonly EmailSettings emailSettings;

        public EmailSender(IConfiguration config)
        {
            this.config = config;
            emailSettings = new EmailSettings
            {
                EmailServer = config["EmailSender:Server"],
                EmailAddress = config["EmailSender:Email"],
                EmailPassword = config["EmailSender:Password"],
                Port = Convert.ToInt32(config["EmailSender:Port"]),
            };
        }

        public void SendEmail(string toEmail, byte[] fileToSend)
        {
            var email = new MimeMessage();

            email.From.Add(MailboxAddress.Parse(emailSettings.EmailAddress));
            email.To.Add(MailboxAddress.Parse(toEmail));
            email.Subject = "Reply for book publish request.";
            
            var builder = new BodyBuilder();
            builder.TextBody = "Your request was received.";
            builder.Attachments.Add("Reply.pdf", fileToSend);

            email.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();

            smtp.Connect(emailSettings.EmailServer, emailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(emailSettings.EmailAddress, emailSettings.EmailPassword);
            smtp.Send(email);
            smtp.Disconnect(true);
        }    
    }
}
