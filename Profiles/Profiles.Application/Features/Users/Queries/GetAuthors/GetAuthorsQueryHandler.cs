using AutoMapper;
using MediatR;
using Profiles.Application.DTOs.Response;
using Profiles.Application.Interfaces.Repositories;
using Profiles.Domain.Exceptions;

namespace Profiles.Application.Features.Users.Queries.GetAuthors
{
    public class GetAuthorsQueryHandler : IRequestHandler<GetAuthorsQuery, IEnumerable<GetUsersResponse>>
    {
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;

        public GetAuthorsQueryHandler(IUserRepository userRepository, IMapper mapper)
        {
            this.userRepository = userRepository;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<GetUsersResponse>> Handle(GetAuthorsQuery request, CancellationToken cancellationToken)
        {
            var authors = await userRepository.GetAuthorsAsync() ??
                throw new NotFoundException(ExceptionMessages.AuthorsNotFoundMessage);
            var authorsResponse = mapper.Map<IEnumerable<GetUsersResponse>>(authors);

            return authorsResponse;
        }
    }
}
