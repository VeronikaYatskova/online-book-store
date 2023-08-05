using Auth.Application.Abstractions.Interfaces.Services;
using MediatR;

namespace Auth.Application.Features.User.Commands.GetRefreshToken
{    
    public class GetRefreshTokenCommandHandler : IRequestHandler<GetRefreshTokenCommand, string>
    {
        private readonly ITokenService _tokenService;

        public GetRefreshTokenCommandHandler(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        public async Task<string> Handle(GetRefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var user = await _tokenService.GetUserAsync();

            if (user is null)
            {
                throw new ArgumentNullException("No user was found");
            }

            return await _tokenService.UpdateRefreshTokenAsync(user);
        }
    }
}
