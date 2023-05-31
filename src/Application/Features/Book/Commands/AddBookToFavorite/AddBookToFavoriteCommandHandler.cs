using Application.Abstractions.Contracts.Interfaces;
using AutoMapper;
using MediatR;

namespace Application.Features.Book.Commands.AddBookToFavorite
{
    public class AddBookToFavoriteCommandHandler : IRequestHandler<AddBookToFavoriteCommand>
    {
        private readonly IUnitOfWork unitOfWork; 
        private readonly IMapper mapper;

        public AddBookToFavoriteCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task Handle(AddBookToFavoriteCommand request, CancellationToken cancellationToken)
        {
            var userId = new Guid(request.userId);
            var bookId = new Guid(request.bookId);
            
            await unitOfWork.FavoriteBooksRepository.AddBookToFavorite(userId, bookId);
            await unitOfWork.SaveChangesAsync();
        }
    }
}
