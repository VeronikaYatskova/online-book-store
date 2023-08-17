using EmailService.Models;
using EmailService.Services;
using EmailService.Services.PdfGeneration.Interfaces;
using MassTransit;
using OnlineBookStore.Messages.Models.Messages;
using Serilog;

namespace EmailService.Consumers
{
    public class RequestUpdateConsumer : IConsumer<RequestUpdatedMessage>
    {
        private readonly IEmailService _emailService;
        private readonly ITemplateGenerator _templateGenerator;

        public RequestUpdateConsumer(
            IEmailService emailService, 
            ITemplateGenerator templateGenerator)
        {
            _emailService = emailService;
            _templateGenerator = templateGenerator;
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

            var pdfGenerator = _templateGenerator.RequestUpdatedMessageHtmlTemplateGenerator(); 
            var template = pdfGenerator.GenerateHtmlTemplate(context.Message);

            await _emailService.SendEmailAsync(message, template);
        }
    }
}
