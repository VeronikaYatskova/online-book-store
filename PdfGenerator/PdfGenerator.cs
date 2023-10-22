using iText.Html2pdf;
using PdfGenerator.Interfaces;

namespace PdfGenerator
{
    public class PdfGenerator : IPdfGenerator
    {
        public byte[] CreatePDF(string template)
        {
            using var memoryStream = new MemoryStream();
            HtmlConverter.ConvertToPdf(template, memoryStream);
    
            return memoryStream.ToArray();
        }
    }
}
