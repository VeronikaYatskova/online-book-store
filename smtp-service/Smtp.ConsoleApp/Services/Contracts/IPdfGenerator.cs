using Smtp.ConsoleApp.Models;

namespace Smtp.ConsoleApp.Services.Contracts
{
    public interface IPdfGenerator
    {
        byte[] CreatePDF(Book book);
    }
}