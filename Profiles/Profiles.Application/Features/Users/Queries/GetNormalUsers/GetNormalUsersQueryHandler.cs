using AutoMapper;
using MediatR;
using Profiles.Application.DTOs.Response;
using Profiles.Application.Interfaces.Repositories;
using OnlineBookStore.Exceptions.Exceptions;

namespace Profiles.Application.Features.Users.Queries.GetNormalUsers
{
    public class GetNormalUsersQueryHandler : IRequestHandler<GetNormalUsersQuery, IEnumerable<GetUsersResponse>>
    {
        private readonly INormalUserRepository _normalUserRepository;
        private readonly IMapper _mapper;

        public GetNormalUsersQueryHandler(INormalUserRepository normalUserRepository, IMapper mapper)
        {
            _normalUserRepository = normalUserRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GetUsersResponse>> Handle(GetNormalUsersQuery request, CancellationToken cancellationToken)
        {
            var users = await _normalUserRepository.GetNormalUsersAsync() ??
                throw new NotFoundException(ExceptionMessages.UsersNotFoundMessage);
                
            var usersResponse = _mapper.Map<IEnumerable<GetUsersResponse>>(users);

            return usersResponse;
        }
    }
}
