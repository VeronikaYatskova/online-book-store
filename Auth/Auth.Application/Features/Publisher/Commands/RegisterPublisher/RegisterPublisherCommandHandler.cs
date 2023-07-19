using Auth.Application.Abstractions.Interfaces.Repositories;
using Auth.Application.Abstractions.Interfaces.Services;
using AutoMapper;
using MediatR;

namespace Auth.Application.Features.Publisher.Commands.RegisterPublisher
{
    public class RegisterPublisherCommandHandler : IRequestHandler<RegisterPublisherCommand, string>
    {
        private readonly IPublisherRepository publisherRepository;
        private readonly ITokenService tokenService;
        private readonly IMapper mapper;

        public RegisterPublisherCommandHandler(
            IPublisherRepository publisherRepository,
            ITokenService tokenService, 
            IMapper mapper)
        {
            this.publisherRepository = publisherRepository;
            this.tokenService = tokenService;
            this.mapper = mapper;
        }

        public async Task<string> Handle(RegisterPublisherCommand request, CancellationToken cancellationToken)
        {
            var publisherData = request.PublisherDataRequest;
            var publisherEntity = mapper.Map<Domain.Models.Publisher>(publisherData); 
            publisherEntity.AccountDataId = request.AccountData.Id;

            publisherRepository.AddPublisher(publisherEntity);
            await publisherRepository.SavePublisherChangesAsync();

            var token = tokenService.CreateToken(request.AccountData);
            await tokenService.SetRefreshTokenAsync(request.AccountData);

            return token;
        }
    }
}
