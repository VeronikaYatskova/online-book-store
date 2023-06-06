using DinkToPdf;
using DinkToPdf.Contracts;
using smptp_pdf_generation.Models;
using smptp_pdf_generation.Services.Contracts;

namespace smptp_pdf_generation.Services
{
    public class PdfGenerator : IPdfGenerator
    {
        private readonly ITemplateGenerator templateGenerator;
        private readonly IConverter converter; 

        public PdfGenerator(ITemplateGenerator templateGenerator, IConverter converter)
        {
            this.templateGenerator = templateGenerator;
            this.converter = converter;
        }

        public byte[] CreatePDF(Book book)
        {
            var globalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4,
                Margins = new MarginSettings { Top = 10 },
                DocumentTitle = "Book publish report",
            };

            var objectSettings = new ObjectSettings
            {
                PagesCount = true,
                HtmlContent = templateGenerator.GetHTMLString(book),
                WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet =  Path.Combine(Directory.GetCurrentDirectory(), "assets", "styles.css") },
                HeaderSettings = { FontName = "Arial", FontSize = 9, Right = "Page [page] of [toPage]", Line = true },
                FooterSettings = { FontName = "Arial", FontSize = 9, Line = true, Center = "Report Footer" }
            };

            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings },
            };

            var file = converter.Convert(pdf);

            return file;
        }
    }
}
