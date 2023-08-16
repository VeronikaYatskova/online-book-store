using AutoMapper;
using MediatR;
using Profiles.Application.DTOs.Response;
using Profiles.Application.Interfaces.Repositories;
using OnlineBookStore.Exceptions.Exceptions;

namespace Profiles.Application.Features.Users.Queries.GetAuthors
{
    public class GetAuthorsQueryHandler : IRequestHandler<GetAuthorsQuery, IEnumerable<GetUsersResponse>>
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly IMapper _mapper;

        public GetAuthorsQueryHandler(IAuthorRepository authorRepository, IMapper mapper)
        {
            _authorRepository = authorRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GetUsersResponse>> Handle(GetAuthorsQuery request, CancellationToken cancellationToken)
        {
            var authors = await _authorRepository.GetAuthorsAsync() ??
                throw new NotFoundException(ExceptionMessages.AuthorsNotFoundMessage);
                
            var authorsResponse = _mapper.Map<IEnumerable<GetUsersResponse>>(authors);

            return authorsResponse;
        }
    }
}
