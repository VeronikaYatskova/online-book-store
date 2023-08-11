using AutoMapper;
using BookStore.Application.Abstractions.Contracts.Interfaces;
using BookStore.Application.DTOs.Response;
using BookStore.Application.Exceptions;
using MediatR;

namespace BookStore.Application.Features.Book.Queries.GetBooksByCategory
{
    public class GetBooksByCategoryQueryHandler : IRequestHandler<GetBooksByCategoryQuery, IEnumerable<BookDto>>
    {
        private readonly IUnitOfWork _unitOfWork; 
        private readonly IMapper _mapper;

        public GetBooksByCategoryQueryHandler(
            IUnitOfWork unitOfWork, 
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        
        public async Task<IEnumerable<BookDto>> Handle(GetBooksByCategoryQuery request, CancellationToken cancellationToken)
        {
            var books = await _unitOfWork.BooksRepository
                .FindAllAsync(b => b.CategoryGuid == Guid.Parse(request.CategoryId)) ??
                    throw new NotFoundException(ExceptionMessages.BooksNotFoundMessage);
            
            foreach (var book in books)
            {
                _unitOfWork.BooksRepository.LoadRelatedDataWithReference(book, book => book.Category);
                _unitOfWork.BooksRepository.LoadRelatedDataWithReference(book, book => book.Publisher);
                _unitOfWork.BooksRepository.LoadRelatedDataWithCollection(book, book => book.BookAuthors);
            }

            var booksDto = _mapper.Map<IEnumerable<BookDto>>(books);

            // foreach (var book in booksDto)
            // {
            //     book.FileURL = _awsS3Service.GetFilePreSignedUrl(book.BookFakeName);
            // }

            return booksDto;
        }
    }
}
