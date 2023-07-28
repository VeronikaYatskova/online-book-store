using AutoMapper;
using MediatR;
using Profiles.Application.DTOs.Response;
using Profiles.Application.Interfaces.Repositories;
using Profiles.Domain.Exceptions;

namespace Profiles.Application.Features.Users.Queries.GetAuthors
{
    public class GetNormalUsersQueryHandler : IRequestHandler<GetAuthorsQuery, IEnumerable<GetUsersResponse>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public GetNormalUsersQueryHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GetUsersResponse>> Handle(GetAuthorsQuery request, CancellationToken cancellationToken)
        {
            var users = await _userRepository.GetNormalUsersAsync() ??
                throw new NotFoundException(ExceptionMessages.UsersNotFoundMessage);
            var usersResponse = _mapper.Map<IEnumerable<GetUsersResponse>>(users);

            return usersResponse;
        }
    }
}
