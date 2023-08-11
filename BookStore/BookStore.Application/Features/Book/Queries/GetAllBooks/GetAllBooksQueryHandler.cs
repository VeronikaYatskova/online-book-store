using BookStore.Application.Abstractions.Contracts.Interfaces;
using BookStore.Application.DTOs.Response;
using BookStore.Application.Exceptions;
using AutoMapper;
using MediatR;

namespace BookStore.Application.Features.Book.Queries.GetAllBooks
{
    public class GetAllBooksQueryHandler : IRequestHandler<GetAllBooksQuery, IEnumerable<BookDto>>
    {
        private readonly IUnitOfWork _unitOfWork; 
        private readonly IMapper _mapper;
        // private readonly IAwsS3Service _awsS3Service;

        public GetAllBooksQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, IAwsS3Service awsS3Service)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            // _awsS3Service = awsS3Service;
        }

        public async Task<IEnumerable<BookDto>> Handle(GetAllBooksQuery request, CancellationToken cancellationToken)
        {
            var books = await _unitOfWork.BooksRepository.FindAllAsync() ??
                throw new NotFoundException(ExceptionMessages.BookListIsEmptyMessage);

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
