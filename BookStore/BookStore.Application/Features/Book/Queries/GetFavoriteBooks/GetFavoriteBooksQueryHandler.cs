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

            foreach (var book in favoriteBooksDto)
            {
                var blob = await _azureService.GetBlobByAsync(b => b.Name == book.BookFakeName)
                    ?? throw new FileDoesNotExistException();
                    
                book.FileURL = blob.Uri!;
            }

            return favoriteBooksDto;
        }
    }
}
