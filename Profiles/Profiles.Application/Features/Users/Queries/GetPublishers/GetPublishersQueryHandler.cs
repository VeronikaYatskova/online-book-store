using AutoMapper;
using MediatR;
using Profiles.Application.DTOs.Response;
using Profiles.Application.Interfaces.Repositories;
using Profiles.Domain.Exceptions;

namespace Profiles.Application.Features.Users.Queries.GetPublishers
{
    public class GetPublishersQueryHandler : IRequestHandler<GetPublishersQuery, IEnumerable<GetUsersResponse>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public GetPublishersQueryHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GetUsersResponse>> Handle(GetPublishersQuery request, CancellationToken cancellationToken)
        {
            var publishers = await _userRepository.GetPublishersAsync() ??
                throw new NotFoundException(ExceptionMessages.PublishersNotFoundMessage);
            var publishersResponse = _mapper.Map<IEnumerable<GetUsersResponse>>(publishers);

            return publishersResponse;
        }
    }
}
