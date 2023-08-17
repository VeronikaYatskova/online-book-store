using EmailService.Services.PdfGeneration.Interfaces;
using EmailService.Services.PdfGeneration.Templates;

namespace EmailService.Services.PdfGeneration
{
    public class TemplateGenerator : ITemplateGenerator
    {
        public IRequestCreatedMessageHtmlTemplateGenerator RequestCreatedMessageHtmlTemplateGenerator()
        {
            return new RequestCreatedMessageHtmlTemplateGenerator();
        }

        public IRequestUpdatedMessageHtmlTemplateGenerator RequestUpdatedMessageHtmlTemplateGenerator()
        {
            return new RequestUpdatedMessageHtmlTemplateGenerator();
        }
    }
}