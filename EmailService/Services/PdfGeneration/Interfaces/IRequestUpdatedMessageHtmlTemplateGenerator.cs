using OnlineBookStore.Messages.Models.Messages;

namespace EmailService.Services.PdfGeneration.Interfaces
{
    public interface IRequestUpdatedMessageHtmlTemplateGenerator
    {
        string GenerateHtmlTemplate(RequestUpdatedMessage message);
    }
}