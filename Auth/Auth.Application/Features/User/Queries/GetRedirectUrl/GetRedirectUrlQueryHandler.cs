using System.Text;
using Auth.Application.Abstractions.Interfaces.Repositories;
using Auth.Application.Abstractions.Interfaces.Services;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace Auth.Application.Features.User.Queries.GetRedirectUrl
{
    public class GetRedirectUrlQueryHandler : IRequestHandler<GetRedirectUrlQuery, string>
    {
        private readonly IUserRepository userRepository;
        private readonly ITokenService tokenService;
        private readonly IConfiguration config;
        private readonly string clientId;
        private readonly string redirectUrl;

        public GetRedirectUrlQueryHandler(IUserRepository userRepository, ITokenService tokenService, IConfiguration config)
        {
            this.userRepository = userRepository;
            this.tokenService = tokenService;
            this.config = config;

            clientId = config["GoogleAuth:ClientId"];
            redirectUrl = config["GoogleAuth:RedirectUrl"];
        }

        public Task<string> Handle(GetRedirectUrlQuery request, CancellationToken cancellationToken)
        {
            var scopes = new StringBuilder();
        
            scopes.Append("https://www.googleapis.com/auth/userinfo.email ");
            scopes.Append("https://www.googleapis.com/auth/userinfo.profile ");
            
            var authString = $"https://accounts.google.com/o/oauth2/auth?client_id={clientId}&redirect_uri={redirectUrl}&access_type=offline&response_type=code&scope={scopes}";
            
            return Task.FromResult(authString);
        }
    }
}
