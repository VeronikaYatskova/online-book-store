using Application.Abstractions.Contracts.Interfaces;
using Application.DTOs.Response;
using AutoMapper;
using MediatR;

namespace Application.Features.Publisher.Queries.GetAllPublishers
{
    public class GetPublisherByIdQueryHandler : IRequestHandler<GetPublisherByIdQuery, PublisherDto>
    {
        private readonly IUnitOfWork unitOfWork; 
        private readonly IMapper mapper;

        public GetPublisherByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<PublisherDto> Handle(GetPublisherByIdQuery request, CancellationToken cancellationToken)
        {
            var publisher = await unitOfWork.PublishersRepository.GetByIdAsync(new Guid(request.id));

            return mapper.Map<PublisherDto>(publisher);
        }
    }
}
