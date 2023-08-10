using BookStore.Application.Abstractions.Contracts.Interfaces;
using BookStore.Application.DTOs.Response;
using AutoMapper;
using MediatR;

namespace BookStore.Application.Features.Book.Queries.GetBooksByName
{
    public class GetBooksByNameQueryHandler : IRequestHandler<GetBooksByNameQuery, IEnumerable<BookDto>>
    {
        private readonly IUnitOfWork unitOfWork; 
        private readonly IMapper mapper;

        public GetBooksByNameQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }
        
        public Task<IEnumerable<BookDto>> Handle(GetBooksByNameQuery request, CancellationToken cancellationToken)
        {
            var books = unitOfWork.BooksRepository.GetByName(request.name);

            return Task.FromResult(mapper.Map<IEnumerable<BookDto>>(books));
        }
    }
}