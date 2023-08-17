using DinkToPdf;
using DinkToPdf.Contracts;

namespace EmailService.Services.PdfGeneration.Interfaces
{
    public class PdfGenerator : IPdfGenerator
    {
        private readonly IConverter _converter;

        public PdfGenerator(IConverter converter)
        {
            _converter = converter;
        }

        public byte[] CreatePDF(string template)
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
                HtmlContent = template,
                WebSettings = 
                { 
                    DefaultEncoding = "utf-8", 
                },
                HeaderSettings = { FontName = "Arial", FontSize = 9, Line = false },
                FooterSettings = { FontName = "Arial", FontSize = 9, Line = false }
           };

            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings },
            };
            
            var file = _converter.Convert(pdf);

            return file;
        }
    }
}
