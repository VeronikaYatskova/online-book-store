using System.Text;
using Auth.Application.Abstractions.Interfaces.Repositories;
using Auth.Application.Abstractions.Interfaces.Services;
using Auth.Domain.Models;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using UserEntity = Auth.Domain.Models.User;

namespace Auth.Application.Features.User.Queries.GetRedirectUrl
{
    public class GetRedirectUrlQueryHandler : IRequestHandler<GetRedirectUrlQuery, string>
    {
        private readonly IRepository<UserEntity> _userRepository;
        private readonly ITokenService _tokenService;
        private readonly IConfiguration _config;
        private readonly IOptions<GoogleCredentials> _googleCredentials;
        
        public GetRedirectUrlQueryHandler(
            IRepository<UserEntity> userRepository, 
            ITokenService tokenService, 
            IConfiguration config,
            IOptions<GoogleCredentials> googleCredentials)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
            _config = config;
            _googleCredentials = googleCredentials;
        }

        public Task<string> Handle(GetRedirectUrlQuery request, CancellationToken cancellationToken)
        {
            var scopes = new StringBuilder();
        
            scopes.Append("https://www.googleapis.com/auth/userinfo.email ");
            scopes.Append("https://www.googleapis.com/auth/userinfo.profile ");
            
            var authString = $"https://accounts.google.com/o/oauth2/auth?client_id={_googleCredentials.Value.ClientId}&redirect_uri={_googleCredentials.Value.RedirectUrl}&access_type=offline&response_type=code&scope={scopes}";
            
            return Task.FromResult(authString);
        }
    }
}
