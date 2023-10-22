using OnlineBookStore.Messages.Models.Messages;

namespace PdfGenerator.Interfaces
{
    public interface IRequestUpdatedMessageHtmlTemplateGenerator
    {
        string GenerateHtmlTemplate(RequestUpdatedMessage message);
    }
}