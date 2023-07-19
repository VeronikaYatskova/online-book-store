using Auth.Application.Abstractions.Interfaces.Repositories;
using Auth.Application.Abstractions.Interfaces.Services;
using AutoMapper;
using MediatR;

namespace Auth.Application.Features.Author.Commands.RegisterAuthor
{
    public class RegisterAuthorCommandHandler : IRequestHandler<RegisterAuthorCommand, string>
    {
        private readonly IAuthorRepository authorRepository;
        private readonly ITokenService tokenService;
        private readonly IMapper mapper;

        public RegisterAuthorCommandHandler(IAuthorRepository authorRepository, ITokenService tokenService, IMapper mapper)
        {
            this.authorRepository = authorRepository;
            this.tokenService = tokenService;
            this.mapper = mapper;
        }

        public async Task<string> Handle(RegisterAuthorCommand request, CancellationToken cancellationToken)
        {
            var authorData = request.AuthorDataRequest;
            var authorEntity = mapper.Map<Domain.Models.Author>(authorData); 
            authorEntity.AccountDataId = request.AccountData.Id;

            authorRepository.AddAuthor(authorEntity);
            await authorRepository.SaveAuthorChangesAsync();

            var token = tokenService.CreateToken(request.AccountData);
            await tokenService.SetRefreshTokenAsync(request.AccountData);

            return token;
        }
    }
}
