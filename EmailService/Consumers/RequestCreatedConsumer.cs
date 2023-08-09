using EmailService.Models;
using EmailService.Services;
using MassTransit;
using RequestsEmailServices.Communication.Models;
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
            Log.Logger.Information("message to publish a book recieved.");

            var message = new Message(new string[] { context.Message.PublisherEmail }, "Book publishing request", "Author request you to publish his book.");
            await _emailService.SendEmailAsync(message);
        }
    }
}
