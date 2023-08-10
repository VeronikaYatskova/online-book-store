using BookStore.Application.Abstractions.Contracts.Interfaces;
using AutoMapper;
using BookStore.Domain.Entities;
using MediatR;

namespace BookStore.Application.Features.Publisher.Commands.AddPublisher
{
    public class AddPublisherCommandHandler : IRequestHandler<AddPublisherCommand>
    {
        private readonly IUnitOfWork unitOfWork; 
        private readonly IMapper mapper;

        public AddPublisherCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task Handle(AddPublisherCommand request, CancellationToken cancellationToken)
        {
            var publisher = new PublisherEntity()
            {
                PublisherName = request.publisher.PublisherName,
            };

            await unitOfWork.PublishersRepository.AddPublisherAsync(publisher);
            await unitOfWork.SaveChangesAsync();
        }
    }
}
