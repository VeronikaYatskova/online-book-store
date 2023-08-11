using BookStore.Application.Abstractions.Contracts.Interfaces;
using BookStore.Application.DTOs.Response;
using AutoMapper;
using MediatR;
using BookStore.Application.Exceptions;

namespace BookStore.Application.Features.Book.Queries.GetBooksByName
{
    public class GetBooksByNameQueryHandler : IRequestHandler<GetBooksByNameQuery, IEnumerable<BookDto>>
    {
        private readonly IUnitOfWork _unitOfWork; 
        private readonly IMapper _mapper;
        // private readonly IAwsS3Service _awsS3Service;

        public GetBooksByNameQueryHandler(
            IUnitOfWork unitOfWork, 
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<BookDto>> Handle(GetBooksByNameQuery request, CancellationToken cancellationToken)
        {
            var books = await _unitOfWork.BooksRepository
                .FindAllAsync(b => b.BookName == request.name) ??
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
