using Application.Abstractions.Contracts.Interfaces;
using AutoMapper;
using MediatR;

namespace Application.Features.Book.Commands.DeleteBookFromFavorite
{
    public class DeleteBookFromFavoriteCommandHandler : IRequestHandler<DeleteBookFromFavoriteCommand>
    {
        private readonly IUnitOfWork unitOfWork; 
        private readonly IMapper mapper;

        public DeleteBookFromFavoriteCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task Handle(DeleteBookFromFavoriteCommand request, CancellationToken cancellationToken)
        {
            var userId = new Guid(request.userId);
            var bookId = new Guid(request.bookId);

            unitOfWork.FavoriteBooksRepository.RemoveBookFromFavorite(userId, bookId);
            await unitOfWork.SaveChangesAsync();
        }
    }
}
