using System.Net;
using Application.Abstractions.Contracts.Interfaces;
using Application.Exceptions;

namespace Application.Services
{
    public class ExceptionsService : IExceptionsService
    {
        private readonly Dictionary<Type, HttpStatusCode> _statusCodes;

        public ExceptionsService()
        {
            _statusCodes = new Dictionary<Type, HttpStatusCode>()
            {
                { typeof(NotFoundException), HttpStatusCode.NotFound },
                { typeof(InvalidDataException), HttpStatusCode.BadRequest },
                { typeof(AlreadyExistsException), HttpStatusCode.Conflict },
                { typeof(ValidationException), HttpStatusCode.BadRequest },
            };
        }

        public HttpStatusCode GetStatusCodeOnException(Exception exception)
        {
            if (!_statusCodes.ContainsKey(exception.GetType()))
            {
                return HttpStatusCode.InternalServerError;
            }

            return _statusCodes[exception.GetType()];
        }
    }
}
