using AuthEmailService.Communication.Models;
using EmailService.Models;
using EmailService.Services;
using MassTransit;
using Serilog;

namespace EmailService.Consumers
{
    public class EmailConfirmationConsumer : IConsumer<EmailConfirmationMessage>
    {
        private readonly IEmailService _emailService;

        public EmailConfirmationConsumer(IEmailService emailService)
        {
            _emailService = emailService;
        }

        public async Task Consume(ConsumeContext<EmailConfirmationMessage> context)
        {
            Log.Logger.Information("Message to confirm an email is received.");

            var message = new Message(
                new string[] { context.Message.To },
                "Email Confirmation",
                context.Message.ConfirmationLink);
            
            await _emailService.SendEmailAsync(message);
        }
    }
}
