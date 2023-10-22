using OnlineBookStore.Messages.Models.Messages;

namespace PdfGenerator.Interfaces
{
    public interface IRequestCreatedMessageHtmlTemplateGenerator
    {
        string GenerateHtmlTemplate(RequestCreatedMessage message);
    }
}