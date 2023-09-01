using EmailService.Models;
using EmailService.Services;
using MassTransit;
using OnlineBookStore.Messages.Models.Messages;
using Serilog;

namespace EmailService.Consumers
{
    public class DailyEmailBookRecommendationConsumer : IConsumer<BookRecommendationMessage>
    {
        private readonly IEmailService _emailService;

        public DailyEmailBookRecommendationConsumer(IEmailService emailService)
        {
            _emailService = emailService;
        }

        public async Task Consume(ConsumeContext<BookRecommendationMessage> context)
        {
            Log.Logger.Information("daily book recommendation was received.");

            var info = context.Message;
            var content = $"{info.Content} <a href={info.BookLink}>here</a>";
            
            Log.Logger.Information(content);

            var message = new Message(
                info.SendTo,
                info.EmailTitle,
                content);

            await _emailService.SendEmailAsync(message);
        }
    }
}
