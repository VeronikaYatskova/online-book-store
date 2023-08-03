using BookStore.Application.Abstractions.Contracts.Interfaces;
using AutoMapper;
using MediatR;
using BookStore.Domain.Entities;

namespace BookStore.Application.Features.Book.Commands.DeleteBookFromFavorite
{
    public class DeleteBookFromFavoriteCommandHandler : IRequestHandler<DeleteBookFromFavoriteCommand>
    {
        private readonly IUnitOfWork _unitOfWork; 
        private readonly IMapper _mapper;

        public DeleteBookFromFavoriteCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task Handle(DeleteBookFromFavoriteCommand request, CancellationToken cancellationToken)
        {
            var userId = new Guid(request.userId);
            var bookId = new Guid(request.bookId);
            var userBookEntity = new UserBookEntity
            {
                UserId = userId,
                BookId = bookId,
            };

            _unitOfWork.UserBooksRepository.Delete(userBookEntity);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
