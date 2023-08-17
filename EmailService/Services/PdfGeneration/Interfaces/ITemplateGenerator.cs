namespace EmailService.Services.PdfGeneration.Interfaces
{
    public interface ITemplateGenerator
    {
        IRequestUpdatedMessageHtmlTemplateGenerator RequestUpdatedMessageHtmlTemplateGenerator();
        IRequestCreatedMessageHtmlTemplateGenerator RequestCreatedMessageHtmlTemplateGenerator();
    }
}
