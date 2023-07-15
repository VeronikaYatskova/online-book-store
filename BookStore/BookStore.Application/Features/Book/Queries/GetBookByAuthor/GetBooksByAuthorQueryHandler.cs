using BookStore.Application.Abstractions.Contracts.Interfaces;
using BookStore.Application.DTOs.Response;
using BookStore.Application.Exceptions;
using AutoMapper;
using BookStore.Domain.Entities;
using MediatR;

namespace BookStore.Application.Features.Book.Queries.GetBookByAuthor
{
    public class GetBooksByAuthorQueryHandler : IRequestHandler<GetBooksByAuthorQuery, IEnumerable<BookDto>>
    {
        private readonly IUnitOfWork unitOfWork; 
        private readonly IMapper mapper;

        public GetBooksByAuthorQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<BookDto>> Handle(GetBooksByAuthorQuery request, CancellationToken cancellationToken)
        {
            var books = await unitOfWork.BooksRepository.GetAllAsync();
            var authors = await unitOfWork.AuthorRepository.GetAllAsync();
            var authorsBooks = await unitOfWork.AuthorsBooksRepository.GetAllAsync();

            var author = authors.FirstOrDefault(a => a.AuthorLastName == request.authorName);
            if (author is null)
            {
                throw new NotFoundException(ExceptionMessages.AuthorNotFound);
            }
            
            var authorBooksGuids = authorsBooks.Where(ab => ab.AuthorGuid == author.AuthorGuid);

            if (authorBooksGuids is null)
            {
                throw new NotFoundException(ExceptionMessages.BookNotFound);
            }

            var authorBooksEntities = new List<BookEntity>();

            foreach (var ab in authorBooksGuids)
            {
                var book = books.Where(b => b.BookGuid == ab.BookGuid).First();
                authorBooksEntities.Add(book);
            }

            return (authorBooksEntities as IEnumerable<BookDto>)!;
        }
    }
}
