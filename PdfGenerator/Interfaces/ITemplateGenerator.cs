namespace PdfGenerator.Interfaces
{
    public interface ITemplateGenerator
    {
        IRequestUpdatedMessageHtmlTemplateGenerator RequestUpdatedMessageHtmlTemplateGenerator();
        IRequestCreatedMessageHtmlTemplateGenerator RequestCreatedMessageHtmlTemplateGenerator();
        IBookPublishedMessageTemplateGenerator BookPublishedMessageTemplateGenerator(); 
    }
}
