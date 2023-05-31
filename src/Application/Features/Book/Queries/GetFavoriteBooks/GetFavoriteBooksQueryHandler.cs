using Application.Abstractions.Contracts.Interfaces;
using Application.DTOs.Response;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Features.Book.Queries.GetFavoriteBooks
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
            var favoriteBooks = await unitOfWork.FavoriteBooksRepository.GetFavouriteBooks(userId);

            if (!favoriteBooks.Any())
            {
                throw Exceptions.Exceptions.NoFavoriteBooks;
            }

            var bookGuids = favoriteBooks.Select(fb => fb.BookId);
            
            var allBooks = await unitOfWork.BooksRepository.GetAllAsync();
            
            var bookEntities = allBooks.Where(b => bookGuids.Contains(b.BookGuid));

            return mapper.Map<IEnumerable<BookDto>>(bookEntities);
        }
    }
}