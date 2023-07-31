using Auth.Application.Abstractions.Interfaces.Repositories;
using Auth.Application.Abstractions.Interfaces.Services;
using Auth.Application.DTOs.Request;
using Auth.Application.DTOs.Response;
using Auth.Application.Features.User.Commands.LoginUser;
using Auth.Application.Features.User.Commands.RegisterUser;
using Auth.Domain.Models;
using AutoMapper;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Responses;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

using UserEntity = Auth.Domain.Models.User;

namespace Auth.Application.Features.User.Commands.LoginUserViaGoogle
{
    public class LoginUserViaGoogleCommandHandler : IRequestHandler<LoginUserViaGoogleCommand, string>
    {
        private readonly ITokenService _tokenService;
        private readonly IRepository<UserEntity> _userRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;
        private readonly IMediator _mediator;
        private readonly IHttpClientFactoryService _httpClientService;
        private readonly IOptions<GoogleCredentials> _googleCredentials;

        public LoginUserViaGoogleCommandHandler(
            ITokenService tokenService, 
            IRepository<UserEntity> userRepository,
            IMapper mapper, 
            IConfiguration config,
            IMediator mediator,
            IHttpClientFactoryService httpClientService,
            IOptions<GoogleCredentials> googleCredentials)
        {
            _tokenService = tokenService;
            _userRepository = userRepository;
            _mapper = mapper;
            _config = config;
            _mediator = mediator;
            _httpClientService = httpClientService;
            _googleCredentials = googleCredentials;           
        }

        public async Task<string> Handle(LoginUserViaGoogleCommand request, CancellationToken cancellationToken)
        {
            var googleToken = await GetGoogleAccessTokenAsync(request.Code);
            var userGoogleRegistrationDto = await GetUserInfoByToken(googleToken);

            var user = await _userRepository.FindByConditionAsync(u => u.Email == userGoogleRegistrationDto.Email);

            if (user is null)
            {
                var userData = _mapper.Map<RegisterUserRequest>(userGoogleRegistrationDto);
                var token = await _mediator.Send(new RegisterUserCommand(userData, UserRolesConstants.UserRole));
                
                return token;
            }
            else
            {
                var loginUserDto = _mapper.Map<LoginUserRequest>(userGoogleRegistrationDto);
                var token = await _mediator.Send(new LoginUserCommand(loginUserDto));

                return token;
            }
        }

        private async Task<string> GetGoogleAccessTokenAsync(string code)
        {
            var clientSecrets = new ClientSecrets
            {
                ClientId = _googleCredentials.Value.ClientId,
                ClientSecret = _googleCredentials.Value.ClientSecret,
            };

            var credential = new GoogleAuthorizationCodeFlow(
                new GoogleAuthorizationCodeFlow.Initializer
                {
                    ClientSecrets = clientSecrets
                });

            TokenResponse token = await credential.ExchangeCodeForTokenAsync(
                "",
                code,
                _googleCredentials.Value.RedirectUrl,
                CancellationToken.None
            );

            return token.AccessToken;
        }

        private async Task<RegisterUserGoogleRequest> GetUserInfoByToken(string token)
        {
            var jsonUserData = await _httpClientService.Execute(token);
            var user = JsonConvert.DeserializeObject<GoogleDataResponse>(jsonUserData);

            var userGoogleRegistrationDto = _mapper.Map<RegisterUserGoogleRequest>(user);

            return userGoogleRegistrationDto;
        }
    }
}
