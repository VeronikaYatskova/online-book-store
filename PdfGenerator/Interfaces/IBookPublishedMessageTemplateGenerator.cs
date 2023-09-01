using PdfGenerator.Models;

namespace PdfGenerator.Interfaces
{
    public interface IBookPublishedMessageTemplateGenerator
    {
        string GenerateHtmlTemplate(BookInPdf message);
    }
}