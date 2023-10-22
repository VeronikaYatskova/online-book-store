using BookStore.Application.Abstractions.Contracts.Interfaces;
using BookStore.Application.DTOs.Response;
using AutoMapper;
using MediatR;
using OnlineBookStore.Exceptions.Exceptions;
using BookStore.Domain.Exceptions;
using BookStore.Application.Services.CloudServices.Azurite.Models;
using Microsoft.Extensions.Options;

namespace BookStore.Application.Features.Book.Queries.GetBookByAuthor
{
    public class GetBooksByAuthorQueryHandler : IRequestHandler<GetBooksByAuthorQuery, IEnumerable<BookDto>>
    {
        private readonly IUnitOfWork _unitOfWork; 
        private readonly IMapper _mapper;
        private readonly IAzureService _azureService;
        private readonly BlobStorageSettings _blobStorageSettings;

        public GetBooksByAuthorQueryHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IAzureService azureService,
            IOptions<BlobStorageSettings> blobStorageSettings)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _azureService = azureService;
            _blobStorageSettings = blobStorageSettings.Value;
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

            await _azureService.LoadRelatedData(booksDto);
            
            return booksDto;
        }
    }
}
