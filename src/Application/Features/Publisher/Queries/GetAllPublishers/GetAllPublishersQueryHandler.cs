using Application.Abstractions.Contracts.Interfaces;
using Application.DTOs.Response;
using Application.Exceptions;
using AutoMapper;
using MediatR;

namespace Application.Features.Publisher.Queries.GetAllPublishers
{
    public class GetAllPublishersQueryHandler : IRequestHandler<GetAllPublishersQuery, IEnumerable<PublisherDto>>
    {
        private readonly IUnitOfWork unitOfWork; 
        private readonly IMapper mapper;

        public GetAllPublishersQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<PublisherDto>> Handle(GetAllPublishersQuery request, CancellationToken cancellationToken)
        {
            var publishers = await unitOfWork.PublishersRepository.GetAllAsync();

            if (publishers is null)
            {
                throw new NotFoundException(ExceptionMessages.PublishersListIsEmpty);
            }

            return mapper.Map<IEnumerable<PublisherDto>>(publishers);
        }
    }
}