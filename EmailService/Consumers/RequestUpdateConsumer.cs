using EmailService.Models;
using EmailService.Services;
using MassTransit;
using RequestsEmailServices.Communication.Models;
using Serilog;

namespace EmailService.Consumers
{
    public class RequestUpdateConsumer : IConsumer<RequestUpdatedMessage>
    {
        private readonly IEmailService _emailService;

        public RequestUpdateConsumer(IEmailService emailService)
        {
            _emailService = emailService;
        }

        public async Task Consume(ConsumeContext<RequestUpdatedMessage> context)
        {
            Log.Logger.Information("Message to send an email is recieved.");

            var requestInfo = context.Message;
            string messageContent = string.Empty;

            if (requestInfo.IsApproved)
            {
                messageContent = EmailMessages.RequestApproved;
            }
            else
            {
                messageContent = EmailMessages.RequestRejected;
            }

            var message = new Message(
                new string[] { context.Message.UserEmail },
                "Book publishing request",
                messageContent);

            await _emailService.SendEmailAsync(message);
        }
    }
}
