using BookStore.Application.Abstractions.Contracts.Interfaces;
using BookStore.Application.DTOs.Response;
using BookStore.Application.Exceptions;
using AutoMapper;
using MediatR;

namespace BookStore.Application.Features.Book.Queries.GetBookById
{
    public class GetBookByIdQueryHandler : IRequestHandler<GetBookByIdQuery, BookDto>
    {
        private readonly IUnitOfWork _unitOfWork; 
        private readonly IMapper _mapper;
        private readonly IAwsS3Service _awsS3Service;

        public GetBookByIdQueryHandler(
            IUnitOfWork unitOfWork, 
            IMapper mapper, 
            IAwsS3Service awsS3Service)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _awsS3Service = awsS3Service;
        }

        public async Task<BookDto> Handle(GetBookByIdQuery request, CancellationToken cancellationToken)
        {
            var book = await _unitOfWork.BooksRepository
                .FindByConditionAsync(b => b.BookGuid == Guid.Parse(request.id)) ??
                    throw new NotFoundException(ExceptionMessages.BookNotFoundMessage);
            
            _unitOfWork.BooksRepository.LoadRelatedDataWithReference(book, book => book.Category);
            _unitOfWork.BooksRepository.LoadRelatedDataWithReference(book, book => book.Publisher);
            _unitOfWork.BooksRepository.LoadRelatedDataWithCollection(book, book => book.BookAuthors);
        
            var bookDto = _mapper.Map<BookDto>(book);
            bookDto.FileURL = _awsS3Service.GetFilePreSignedUrl(book.BookFakeName!);

            return bookDto;
        }
    }
}
