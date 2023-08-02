using BookStore.Application.Abstractions.Contracts.Interfaces;
using BookStore.Application.DTOs.Response;
using BookStore.Application.Exceptions;
using AutoMapper;
using BookStore.Domain.Entities;
using MediatR;

namespace BookStore.Application.Features.Book.Queries.GetFavoriteBooks
{
    public class GetFavoriteBooksQueryHandler : IRequestHandler<GetFavoriteBooksQuery, IEnumerable<BookDto>>
    {
        private readonly IUnitOfWork unitOfWork; 
        private readonly IMapper mapper;

        public GetFavoriteBooksQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<BookDto>> Handle(GetFavoriteBooksQuery request, CancellationToken cancellationToken)
        {
            var userId = new Guid(request.userId);
            var favoriteBooks = await unitOfWork.UserBooksRepository.FindAllAsync(b => b.UserId == userId);

            if (!favoriteBooks.Any())
            {
                throw new NotFoundException(ExceptionMessages.NoFavoriteBooks);
            }

            var bookGuids = favoriteBooks.Select(fb => fb.BookId);
            
            var allBooks = await unitOfWork.BooksRepository.FindAllAsync();
            
            var bookEntities = allBooks.Where(b => bookGuids.Contains(b.BookGuid));

            return mapper.Map<IEnumerable<BookDto>>(bookEntities);
        }
    }
}
