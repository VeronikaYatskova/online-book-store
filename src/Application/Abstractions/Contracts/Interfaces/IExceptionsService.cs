using System.Net;

namespace Application.Abstractions.Contracts.Interfaces
{
    public interface IExceptionsService
    {
        HttpStatusCode GetStatusCodeOnException(Exception exception);
    }
}
