using System.Net;

namespace BookStore.Application.Abstractions.Contracts.Interfaces
{
    public interface IExceptionsService
    {
        HttpStatusCode GetStatusCodeOnException(Exception exception);
    }
}
