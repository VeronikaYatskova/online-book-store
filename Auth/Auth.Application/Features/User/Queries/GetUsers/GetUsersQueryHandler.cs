using Auth.Application.Abstractions.Interfaces.Repositories;
using Auth.Application.DTOs.Response;
using Auth.Domain.Exceptions;
using AutoMapper;
using MediatR;
using OnlineBookStore.Exceptions.Exceptions;

using UserEntity = Auth.Domain.Models.User;

namespace Auth.Application.Features.User.Queries.GetUsers
{
    public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, IEnumerable<GetUsersResponse>>
    {
        private readonly IRepository<UserEntity> _userRepository;
        private readonly IMapper _mapper;

        public GetUsersQueryHandler(IRepository<UserEntity> userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GetUsersResponse>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            var users = await _userRepository.FindAllAsync()! ??
                throw new NotFoundException(ExceptionMessages.UserNotFoundMessage);

            return _mapper.Map<IEnumerable<GetUsersResponse>>(users);
        }
    }
}
