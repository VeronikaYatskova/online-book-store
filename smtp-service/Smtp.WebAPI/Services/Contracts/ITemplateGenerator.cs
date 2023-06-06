using Smtp.WebAPI.Models;

namespace Smtp.WebAPI.Services.Contracts
{
    public interface ITemplateGenerator
    {
        string GetHTMLString(Book book);
    }
}