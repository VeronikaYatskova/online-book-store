using EmailService.Models;
using EmailService.Services;
using MassTransit;
using OnlineBookStore.Messages.Models.Messages;
using Serilog;

namespace EmailService.Consumers
{
    public class RequestCreatedConsumer : IConsumer<RequestCreatedMessage>
    {
        private readonly IEmailService _emailService;

        public RequestCreatedConsumer(IEmailService emailService)
        {
            _emailService = emailService;
        }

        public async Task Consume(ConsumeContext<RequestCreatedMessage> context)
        {
            Log.Logger.Information("Message to publish a book recieved.");

            var message = new Message(
                new string[] { context.Message.PublisherEmail },
                "Book publishing request",
                EmailMessages.RequestCreated);
                
            await _emailService.SendEmailAsync(message);
        }
    }
}
