using EmailService.Models;
using Microsoft.Extensions.Options;
using MimeKit;

using MailKitSmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace EmailService.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailConfiguration _emailConfig;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IOptions<EmailConfiguration> emailConfig, ILogger<EmailService> logger)
        {
            _emailConfig = emailConfig.Value;
            _logger = logger;
        }

        public async Task SendEmailAsync(Message message)
        {
            MimeMessage emailMessage = new MimeMessage();
            emailMessage = CreateMessage(message);

            await Send(emailMessage);
        }

        private MimeMessage CreateMessage(Message message)
        {
            var emailMessage = new MimeMessage();
            
            emailMessage.From.Add(new MailboxAddress("email", _emailConfig.From));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;

            var builder = new BodyBuilder();
            builder.TextBody = message.Content;

            emailMessage.Body = builder.ToMessageBody();
            
            return emailMessage;
        }

        private async Task Send(MimeMessage mimeMessage)
        {
            using var client = new MailKitSmtpClient();

            try
            {
                await client.ConnectAsync(_emailConfig.SmtpServer, _emailConfig.Port, true);
                client.Authenticate(_emailConfig.From, _emailConfig.Password);
                
                await client.SendAsync(mimeMessage);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                await client.DisconnectAsync(true);
                client.Dispose();
            }
        }
    }
}
