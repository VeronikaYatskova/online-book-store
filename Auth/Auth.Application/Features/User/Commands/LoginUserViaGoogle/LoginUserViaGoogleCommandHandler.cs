using Auth.Application.Abstractions.Interfaces.Repositories;
using Auth.Application.Abstractions.Interfaces.Services;
using Auth.Application.DTOs.Request;
using Auth.Application.DTOs.Response;
using Auth.Application.Features.User.Commands.LoginUser;
using Auth.Application.Features.User.Commands.RegisterUser;
using AutoMapper;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Responses;
using MediatR;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Auth.Application.Features.User.Commands.LoginUserViaGoogle
{
    public class LoginUserViaGoogleCommandHandler : IRequestHandler<LoginUserViaGoogleCommand, string>
    {
        private readonly ITokenService tokenService;
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;
        private readonly IConfiguration config;
        private readonly IMediator mediator;
        private readonly IHttpClientFactoryService httpClientService;
        private readonly string clientId;
        private readonly string clientSecret;
        private readonly string redirectUrl;

        public LoginUserViaGoogleCommandHandler(
            ITokenService tokenService, 
            IUserRepository userRepository, 
            IMapper mapper, 
            IConfiguration config,
            IMediator mediator,
            IHttpClientFactoryService httpClientService)
        {
            this.tokenService = tokenService;
            this.userRepository = userRepository;
            this.mapper = mapper;
            this.config = config;
            this.mediator = mediator;
            this.httpClientService = httpClientService;

            clientId = config["GoogleAuth:ClientId"];
            clientSecret = config["GoogleAuth:ClientSecret"];
            redirectUrl = config["GoogleAuth:RedirectUrl"];
        }

        public async Task<string> Handle(LoginUserViaGoogleCommand request, CancellationToken cancellationToken)
        {
            var googleToken = await GetGoogleAccessTokenAsync(request.Code);
            var userGoogleRegistrationDto = await GetUserInfoByToken(googleToken);

            var userExists = userRepository.FindUserBy(u => u.Email == userGoogleRegistrationDto.Email);

            if (userExists is null)
            {
                var userData = mapper.Map<RegisterUserRequest>(userGoogleRegistrationDto);
                userData.RoleId = request.RoleId;

                var token = await mediator.Send(new RegisterUserCommand(userData));
                
                return token;
            }
            else
            {
                var loginUserDto = mapper.Map<LoginUserRequest>(userGoogleRegistrationDto);
                var token = await mediator.Send(new LoginUserCommand(loginUserDto));

                return token;
            }
        }

        private async Task<string> GetGoogleAccessTokenAsync(string code)
        {
            var clientSecrets = new ClientSecrets
            {
                ClientId = clientId,
                ClientSecret = clientSecret,
            };

            var credential = new GoogleAuthorizationCodeFlow(
                new GoogleAuthorizationCodeFlow.Initializer
                {
                    ClientSecrets = clientSecrets
                });

            TokenResponse token = await credential.ExchangeCodeForTokenAsync(
                "",
                code,
                redirectUrl,
                CancellationToken.None
            );

            return token.AccessToken;
        }

        private async Task<RegisterUserGoogleRequest> GetUserInfoByToken(string token)
        {
            var jsonUserData = await httpClientService.Execute(token);
            var user = JsonConvert.DeserializeObject<GoogleDataResponse>(jsonUserData);

            var userGoogleRegistrationDto = mapper.Map<RegisterUserGoogleRequest>(user);

            return userGoogleRegistrationDto;
        }
    }
}
