using AutoMapper;
using MediatR;
using Profiles.Application.DTOs.Response;
using Profiles.Application.Features.Users.Queries.GetNormalUsers;
using Profiles.Application.Interfaces.Repositories;
using Profiles.Domain.Exceptions;

namespace Profiles.Application.Features.Users.Queries.GetAuthors
{
    public class GetNormalUsersQueryHandler : IRequestHandler<GetNormalUsersQuery, IEnumerable<GetUsersResponse>>
    {
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;

        public GetNormalUsersQueryHandler(IUserRepository userRepository, IMapper mapper)
        {
            this.userRepository = userRepository;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<GetUsersResponse>> Handle(GetNormalUsersQuery request, CancellationToken cancellationToken)
        {
            var users = await userRepository.GetNormalUsersAsync() ??
                throw new NotFoundException(ExceptionMessages.UsersNotFoundMessage);
            var usersResponse = mapper.Map<IEnumerable<GetUsersResponse>>(users);

            return usersResponse;
        }
    }
}
