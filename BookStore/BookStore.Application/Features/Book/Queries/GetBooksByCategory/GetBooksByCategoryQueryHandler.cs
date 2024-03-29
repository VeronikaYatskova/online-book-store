using AutoMapper;
using BookStore.Application.Abstractions.Contracts.Interfaces;
using BookStore.Application.DTOs.Response;
using MediatR;
using OnlineBookStore.Exceptions.Exceptions;
using BookStore.Domain.Exceptions;

namespace BookStore.Application.Features.Book.Queries.GetBooksByCategory
{
    public class GetBooksByCategoryQueryHandler : IRequestHandler<GetBooksByCategoryQuery, IEnumerable<BookDto>>
    {
        private readonly IUnitOfWork _unitOfWork; 
        private readonly IMapper _mapper;
        private readonly IAzureService _azureService;

        public GetBooksByCategoryQueryHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IAzureService azureService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _azureService = azureService;
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

            await _azureService.LoadRelatedData(booksDto);

            return booksDto;
        }
    }
}
