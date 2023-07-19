using Auth.Application.DTOs.Request;
using Auth.Domain.Models;
using MediatR;

namespace Auth.Application.Features.Account.Commands.RegisterAccount
{
    public record RegisterAccountCommand(
        RegisterAccountDataRequest RegisterAccountDataRequest, 
        string Role) : IRequest<AccountData>;
}
