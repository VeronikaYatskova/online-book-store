using BookStore.Application.Abstractions.Contracts.Interfaces;
using AutoMapper;
using BookStore.Domain.Entities;
using MediatR;

namespace BookStore.Application.Features.Book.Commands.AddBook
{
    public class AddBookCommandHandler : IRequestHandler<AddBookCommand>
    {
        private readonly IUnitOfWork _unitOfWork; 
        private readonly IMapper _mapper;

        public AddBookCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task Handle(AddBookCommand request, CancellationToken cancellationToken)
        {
            var bookEntity = _mapper.Map<BookEntity>(request.BookData);
            bookEntity.BookFakeName = request.BookFakeName;
            bookEntity.PublisherGuid = new Guid(request.BookData.PublisherGuid);

            await _unitOfWork.BooksRepository.CreateAsync(bookEntity);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
