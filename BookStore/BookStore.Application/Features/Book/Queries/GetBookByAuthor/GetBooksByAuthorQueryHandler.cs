using BookStore.Application.Abstractions.Contracts.Interfaces;
using BookStore.Application.DTOs.Response;
using AutoMapper;
using MediatR;
using BookStore.Application.Exceptions;

namespace BookStore.Application.Features.Book.Queries.GetBookByAuthor
{
    public class GetBooksByAuthorQueryHandler : IRequestHandler<GetBooksByAuthorQuery, IEnumerable<BookDto>>
    {
        private readonly IUnitOfWork _unitOfWork; 
        private readonly IMapper _mapper;

        public GetBooksByAuthorQueryHandler(
            IUnitOfWork unitOfWork, 
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
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

            // foreach (var book in booksDto)
            // {
            //     book.FileURL = _awsS3Service.GetFilePreSignedUrl(book.BookFakeName);
            // }
            
            return booksDto;
        }
    }
}
