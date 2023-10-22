using AzureFunctions.Models;
using EmailService.Models;
using EmailService.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Company.Function
{
    public class DailyEmailBookRecomendationTrigger
    {
        private readonly IEmailService _emailService;
        private readonly ILogger<DailyEmailBookRecomendationTrigger> _logger;

        public DailyEmailBookRecomendationTrigger(
            IEmailService emailService,
            ILogger<DailyEmailBookRecomendationTrigger> logger)
        {
            _emailService = emailService;
            _logger = logger;
        }

        [Function("DailyEmailBookRecomendationTrigger")]
        public async Task Run([RabbitMQTrigger("daily-book-recommendation",  ConnectionStringSetting = "RabbitMqConnection")] 
            string inputMessage, ILogger log)
        {
            var message = JsonConvert.DeserializeObject<QueueMessage>(inputMessage);
            _logger.LogInformation($"Message from queue: {inputMessage}");

            var queueMessage = message!.Message;
            var content = $"{queueMessage.EmailTitle} {queueMessage.BookLink} here";
            
            _logger.LogInformation($"Email content: {content}");

            var emailMessage = new Message(
                queueMessage.SendTo,
                queueMessage.EmailTitle,
                content);

            _logger.LogInformation($"Email message: {emailMessage.Content} {emailMessage.Subject} {content}");

            await _emailService.SendEmailAsync(emailMessage);
        }
    }
}
