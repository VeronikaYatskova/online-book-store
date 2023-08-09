using EmailService.Models;
using Microsoft.Extensions.Options;
using MimeKit;

using MailKitSmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace EmailService.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailConfiguration _emailConfig;

        public EmailService(IOptions<EmailConfiguration> emailConfig)
        {
            _emailConfig = emailConfig.Value;
        }

        public async Task SendEmailAsync(Message message)
        {
            var emailMessage = CreateMessage(message);

            await Send(emailMessage);
        }

        private MimeMessage CreateMessage(Message message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("email", _emailConfig.From));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Text)
            {
                Text = message.Content,
            };

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
