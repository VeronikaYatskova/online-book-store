using Auth.Application.Abstractions.Interfaces.Services;
using MediatR;

namespace Auth.Application.Features.Account.Commands.GetRefreshToken
{    
    public class GetRefreshTokenCommandHandler : IRequestHandler<GetRefreshTokenCommand, string>
    {
        private readonly ITokenService tokenService;

        public GetRefreshTokenCommandHandler(ITokenService tokenService)
        {
            this.tokenService = tokenService;
        }

        public async Task<string> Handle(GetRefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var accountData = await tokenService.GetAccountDataAsync();

            if (accountData is null)
            {
                throw new ArgumentNullException("No user was found");
            }

            return await tokenService.UpdateRefreshTokenAsync(accountData);
        }
    }
}
