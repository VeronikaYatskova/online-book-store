using BookStore.Application.Abstractions.Contracts.Interfaces;
using BookStore.Application.DTOs.Response;
using AutoMapper;
using MediatR;
using BookStore.Domain.Exceptions;

namespace BookStore.Application.Features.Book.Queries.GetBookByAuthor
{
    public class GetBooksByAuthorQueryHandler : IRequestHandler<GetBooksByAuthorQuery, IEnumerable<BookDto>>
    {
        private readonly IUnitOfWork _unitOfWork; 
        private readonly IMapper _mapper;
        private readonly IAzureService _azureService;

        public GetBooksByAuthorQueryHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IAzureService azureService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _azureService = azureService;
        }

        public async Task<IEnumerable<BookDto>> Handle(
            GetBooksByAuthorQuery request,
            CancellationToken cancellationToken)
        {
            var authorId = Guid.Parse(request.AuthorId);

            var bookAuthorEntities = await _unitOfWork.AuthorsBooksRepository
                .FindAllAsync(b => b.AuthorGuid == authorId) ??
                    throw new NotFoundException(ExceptionMessages.BooksNotFoundMessage);
            
            var booksEntities = await _unitOfWork.BooksRepository.FindAllAsync();

            var books = booksEntities.Join(
                bookAuthorEntities, 
                obj => obj.BookGuid, 
                id => id.BookGuid, 
                (obj, id) => obj);
            
            foreach (var book in books)
            {
                _unitOfWork.BooksRepository.LoadRelatedDataWithReference(book, book => book.Category);
                _unitOfWork.BooksRepository.LoadRelatedDataWithReference(book, book => book.Publisher);
                _unitOfWork.BooksRepository.LoadRelatedDataWithCollection(book, book => book.BookAuthors);
            }
            
            var booksDto = _mapper.Map<List<BookDto>>(books.ToList());

            foreach (var book in booksDto)
            {
                var blob = await _azureService.GetBlobByAsync(b => b.Name == book.BookFakeName);
                    
                if (blob is not null)
                {
                    book.FileURL = blob.Uri!;
                }
            }
            
            return booksDto;
        }
    }
}
