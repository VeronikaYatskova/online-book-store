using EmailService.Models;

namespace EmailService.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(Message message, string? template = null);
    }
}
