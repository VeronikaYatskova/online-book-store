namespace EmailService.Services.PdfGeneration.Interfaces
{
    public interface IPdfGenerator
    {
        byte[] CreatePDF(string template);
    }
}