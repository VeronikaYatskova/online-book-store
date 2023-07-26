using AutoMapper;
using MediatR;
using Profiles.Application.DTOs.Response;
using Profiles.Application.Interfaces.Repositories;
using Profiles.Domain.Exceptions;

namespace Profiles.Application.Features.Users.Queries.GetPublishers
{
    public class GetPublishersQueryHandler : IRequestHandler<GetPublishersQuery, IEnumerable<GetUsersResponse>>
    {
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;

        public GetPublishersQueryHandler(IUserRepository userRepository, IMapper mapper)
        {
            this.userRepository = userRepository;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<GetUsersResponse>> Handle(GetPublishersQuery request, CancellationToken cancellationToken)
        {
            var publishers = await userRepository.GetPublishersAsync() ??
                throw new NotFoundException(ExceptionMessages.PublishersNotFoundMessage);
            var publishersResponse = mapper.Map<IEnumerable<GetUsersResponse>>(publishers);

            return publishersResponse;
        }
    }
}
