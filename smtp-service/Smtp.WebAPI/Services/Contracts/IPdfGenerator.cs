using Smtp.WebAPI.Models;

namespace Smtp.WebAPI.Services.Contracts
{
    public interface IPdfGenerator
    {
        byte[] CreatePDF(Book book);
    }
}