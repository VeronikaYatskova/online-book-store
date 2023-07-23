using Auth.Application.Abstractions.Interfaces.Repositories;
using Auth.Application.DTOs.Response;
using AutoMapper;
using MediatR;
using Auth.Domain.Exceptions;

namespace Auth.Application.Features.User.Queries.GetUsers
{
    public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, IEnumerable<GetUsersResponse>>
    {
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;

        public GetUsersQueryHandler(IUserRepository userRepository, IMapper mapper)
        {
            this.userRepository = userRepository;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<GetUsersResponse>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            var users = await userRepository.FindAllUsersAsync();

            if (users is null)
            {
                throw new NotFoundException(ExceptionMessages.UserNotFoundMessage);
            }

            return mapper.Map<IEnumerable<GetUsersResponse>>(users);
        }
    }
}
