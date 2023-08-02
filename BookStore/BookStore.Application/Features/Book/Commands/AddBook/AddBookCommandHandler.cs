using BookStore.Application.Abstractions.Contracts.Interfaces;
using AutoMapper;
using BookStore.Domain.Entities;
using MediatR;

namespace BookStore.Application.Features.Book.Commands.AddBook
{
    public class AddBookCommandHandler : IRequestHandler<AddBookCommand>
    {
        private readonly IUnitOfWork unitOfWork; 
        private readonly IMapper mapper;

        public AddBookCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task Handle(AddBookCommand request, CancellationToken cancellationToken)
        {
            var bookEntity = mapper.Map<BookEntity>(request.book);
            bookEntity.BookFakeName = request.bookFakeName;
            bookEntity.CategoryGuid = new Guid(request.book.CategotyGuid);

            await unitOfWork.BooksRepository.CreateAsync(bookEntity);
            await unitOfWork.SaveChangesAsync();
        }
    }
}
