using EmailService.Models;
using Microsoft.Extensions.Options;
using MimeKit;
using Serilog;
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
            Log.Logger.Information("Email service: Email content " + message.Content);
            _logger.LogInformation("Email service: Email subject" + message.Subject);
            
            MimeMessage emailMessage = new MimeMessage();
            emailMessage = CreateMessage(message);

            await Send(emailMessage);
        }

        private MimeMessage CreateMessage(Message message)
        {
            var emailMessage = new MimeMessage();
            
            _logger.LogInformation("From email address: " + _emailConfig.From);

            // emailMessage.From.Add(new MailboxAddress("email", _emailConfig.From));
            emailMessage.From.Add(new MailboxAddress("email", "noreplymybookstore@gmail.com"));
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
                // await client.ConnectAsync(_emailConfig.SmtpServer, _emailConfig.Port, true);
                // client.Authenticate(_emailConfig.From, _emailConfig.Password);
                
                await client.ConnectAsync("smtp.gmail.com", 465, true);
                client.Authenticate("noreplymybookstore@gmail.com", "ndfkdgkfcpwcsjpk");

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
