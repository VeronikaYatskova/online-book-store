using EmailService.Models;
using EmailService.Services;
using EmailService.Services.PdfGeneration.Interfaces;
using MassTransit;
using OnlineBookStore.Messages.Models.Messages;
using Serilog;

namespace EmailService.Consumers
{
    public class RequestCreatedConsumer : IConsumer<RequestCreatedMessage>
    {
        private readonly IEmailService _emailService;
        private readonly ITemplateGenerator _templateGenerator;

        public RequestCreatedConsumer(
            IEmailService emailService, 
            ITemplateGenerator templateGenerator)
        {
            _emailService = emailService;
            _templateGenerator = templateGenerator;
        }

        public async Task Consume(ConsumeContext<RequestCreatedMessage> context)
        {
            Log.Logger.Information("Message to publish a book recieved.");

            var message = new Message(
                new string[] { context.Message.PublisherEmail },
                "Book publishing request",
                EmailMessages.RequestCreated);
            
            var pdfGenerator = _templateGenerator.RequestCreatedMessageHtmlTemplateGenerator(); 
            var template = pdfGenerator.GenerateHtmlTemplate(context.Message);

            await _emailService.SendEmailAsync(message, template);
        }
    }
}
