using Auth.Application.Abstractions.Services;
using MediatR;

namespace Auth.Application.Features.User.Commands.GetRefreshToken
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
            var user = await tokenService.GetUserAsync();

            if (user is null)
            {
                throw new ArgumentNullException("No user was found");
            }

            return await tokenService.UpdateRefreshTokenAsync(user);
        }
    }
}
