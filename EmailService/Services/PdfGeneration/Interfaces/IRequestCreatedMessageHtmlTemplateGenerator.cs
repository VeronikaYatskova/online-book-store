using OnlineBookStore.Messages.Models.Messages;

namespace EmailService.Services.PdfGeneration.Interfaces
{
    public interface IRequestCreatedMessageHtmlTemplateGenerator
    {
        string GenerateHtmlTemplate(RequestCreatedMessage message);
    }
}