using PdfGenerator.Interfaces;
using PdfGenerator.Templates;

namespace PdfGenerator
{
    public class TemplateGenerator : ITemplateGenerator
    {
        public IBookPublishedMessageTemplateGenerator BookPublishedMessageTemplateGenerator()
        {
            return new BookPublishedMessageTemplateGenerator();
        }

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