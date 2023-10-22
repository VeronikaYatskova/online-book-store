using BookStore.Application.Abstractions.Contracts.Interfaces;
using AutoMapper;
using MediatR;
using BookStore.Domain.Entities;

namespace BookStore.Application.Features.Book.Commands.AddBookToFavorite
{
    public class AddBookToFavoriteCommandHandler : IRequestHandler<AddBookToFavoriteCommand>
    {
        private readonly IUnitOfWork _unitOfWork; 
        private readonly IMapper _mapper;

        public AddBookToFavoriteCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task Handle(AddBookToFavoriteCommand request, CancellationToken cancellationToken)
        {
            var userId = new Guid(request.UserId);
            var bookId = new Guid(request.BookId);
            var userBookEntity = new UserBookEntity
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                BookId = bookId,
            };

            await _unitOfWork.UserBooksRepository.CreateAsync(userBookEntity);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
