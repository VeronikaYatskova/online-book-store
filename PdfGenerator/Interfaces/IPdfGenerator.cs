namespace PdfGenerator.Interfaces
{
    public interface IPdfGenerator
    {
        byte[] CreatePDF(string template);
    }
}