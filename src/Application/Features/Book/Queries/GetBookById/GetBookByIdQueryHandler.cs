using Application.Abstractions.Contracts.Interfaces;
using Application.DTOs.Response;
using Application.Exceptions;
using AutoMapper;
using MediatR;

namespace Application.Features.Book.Queries.GetBookById
{
    public class GetBookByIdQueryHandler : IRequestHandler<GetBookByIdQuery, BookDto>
    {
        private readonly IUnitOfWork unitOfWork; 
        private readonly IMapper mapper;

        public GetBookByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<BookDto> Handle(GetBookByIdQuery request, CancellationToken cancellationToken)
        {
            var book = await unitOfWork.BooksRepository.GetByIdAsync(new Guid(request.id));

            if (book is null)
            {
                throw new NotFoundException(ExceptionMessages.BookNotFound);
            }

            return mapper.Map<BookDto>(book);
        }
    }
}
