using OnlineBookStore.Messages.Models.Messages;

namespace BookStore.Application.Abstractions.Contracts.Interfaces
{
    public interface IBookPublishingFacade
    {
        Task PublishBookAsync(BookPublishingMessage bookPublishingMessage);
    }
}