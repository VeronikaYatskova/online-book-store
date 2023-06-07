using Smtp.ConsoleApp.Models;

namespace Smtp.ConsoleApp.Services.Contracts
{
    public interface ITemplateGenerator
    {
        string GetHTMLString(Book book);
    }
}