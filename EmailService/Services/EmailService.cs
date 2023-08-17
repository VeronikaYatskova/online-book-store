using EmailService.Models;
using EmailService.Services.PdfGeneration.Interfaces;
using Microsoft.Extensions.Options;
using MimeKit;

using MailKitSmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace EmailService.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailConfiguration _emailConfig;
        private readonly IPdfGenerator _pdfGenerator;

        public EmailService(IOptions<EmailConfiguration> emailConfig, IPdfGenerator pdfGenerator)
        {
            _emailConfig = emailConfig.Value;
            _pdfGenerator = pdfGenerator;
        }

        public async Task SendEmailAsync(Message message, string? template = null)
        {
            MimeMessage emailMessage = new MimeMessage();

            if (template is null)
            {
                emailMessage = CreateMessage(message);
            }
            else
            {
                var pdfFile = _pdfGenerator.CreatePDF(template);
                emailMessage = CreateMessage(message, pdfFile);
            }

            await Send(emailMessage);
        }

        private MimeMessage CreateMessage(Message message, byte[]? pdfFile = null)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("email", _emailConfig.From));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;

            var builder = new BodyBuilder();
            builder.TextBody = message.Content;
            
            if (pdfFile is not null)
            {
                builder.Attachments.Add("Request.pdf", pdfFile);
            }

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
