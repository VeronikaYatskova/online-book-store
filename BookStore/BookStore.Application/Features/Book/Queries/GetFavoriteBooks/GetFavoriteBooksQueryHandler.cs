using BookStore.Application.Abstractions.Contracts.Interfaces;
using BookStore.Application.DTOs.Response;
using BookStore.Application.Exceptions;
using AutoMapper;
using MediatR;

namespace BookStore.Application.Features.Book.Queries.GetFavoriteBooks
{
    public class GetFavoriteBooksQueryHandler : IRequestHandler<GetFavoriteBooksQuery, IEnumerable<BookDto>>
    {
        private readonly IUnitOfWork _unitOfWork; 
        private readonly IMapper _mapper;

        public GetFavoriteBooksQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<BookDto>> Handle(GetFavoriteBooksQuery request, CancellationToken cancellationToken)
        {
            var userId = new Guid(request.userId);
            var favoriteBooks = await _unitOfWork.UserBooksRepository
                .FindAllAsync(b => b.UserId == userId) ??
                    throw new NotFoundException(ExceptionMessages.NoFavoriteBooks);;

            var bookGuids = favoriteBooks.Select(fb => fb.BookId);
            
            var allBooks = await _unitOfWork.BooksRepository.FindAllAsync();
            
            var bookEntities = allBooks.Where(b => bookGuids.Contains(b.BookGuid));

            return _mapper.Map<IEnumerable<BookDto>>(bookEntities);
        }
    }
}
