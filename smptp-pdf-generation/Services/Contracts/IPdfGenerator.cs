using smptp_pdf_generation.Models;

namespace smptp_pdf_generation.Services.Contracts
{
    public interface IPdfGenerator
    {
        byte[] CreatePDF(Book book);
    }
}