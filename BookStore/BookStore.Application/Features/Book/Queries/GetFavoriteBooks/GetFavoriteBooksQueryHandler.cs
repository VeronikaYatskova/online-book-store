using BookStore.Application.Abstractions.Contracts.Interfaces;
using BookStore.Application.DTOs.Response;
using AutoMapper;
using MediatR;
using OnlineBookStore.Exceptions.Exceptions;
using BookStore.Domain.Exceptions;

namespace BookStore.Application.Features.Book.Queries.GetFavoriteBooks
{
    public class GetFavoriteBooksQueryHandler : IRequestHandler<GetFavoriteBooksQuery, IEnumerable<BookDto>>
    {
        private readonly IUnitOfWork _unitOfWork; 
        private readonly IMapper _mapper;
        private readonly IAzureService _azureService;

        public GetFavoriteBooksQueryHandler(
            IUnitOfWork unitOfWork, 
            IMapper mapper, 
            IAzureService azureService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _azureService = azureService;
        }

        public async Task<IEnumerable<BookDto>> Handle(GetFavoriteBooksQuery request, CancellationToken cancellationToken)
        {
            var userId = new Guid(request.userId);
            var favoriteBooks = await _unitOfWork.UserBooksRepository
                .FindAllAsync(b => b.UserId == userId) ??
                    throw new NotFoundException(ExceptionMessages.NoFavoriteBooksMessage);;

            var bookGuids = favoriteBooks.Select(fb => fb.BookId);
            
            var allBooks = await _unitOfWork.BooksRepository.FindAllAsync();
            
            var bookEntities = allBooks.Where(b => bookGuids.Contains(b.BookGuid));

            var favoriteBooksDto = _mapper.Map<IEnumerable<BookDto>>(bookEntities);

            await _azureService.LoadRelatedData(favoriteBooksDto);

            return favoriteBooksDto;
        }
    }
}
