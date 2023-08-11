using BookStore.Application.Abstractions.Contracts.Interfaces;
using BookStore.Application.Exceptions;
using AutoMapper;
using MediatR;

namespace BookStore.Application.Features.Book.Commands.DeleteBook
{
    public class DeleteBookCommandHandler : IRequestHandler<DeleteBookCommand>
    {
        private readonly IUnitOfWork _unitOfWork; 
        private readonly IMapper _mapper;
        private readonly IAzureService _azureService;

        public DeleteBookCommandHandler(
            IUnitOfWork unitOfWork, 
            IMapper mapper, 
            IAzureService azureService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _azureService = azureService;
        }

        public async Task Handle(DeleteBookCommand request, CancellationToken cancellationToken)
        {
            var bookId = Guid.Parse(request.BookId);
            var bookToDelete = await _unitOfWork.BooksRepository
                .FindByConditionAsync(b => b.BookGuid == bookId) ??
                    throw new NotFoundException(ExceptionMessages.BookNotFoundMessage);;

            await _azureService.DeleteAsync(bookToDelete.BookFakeName!);

            _unitOfWork.BooksRepository.Delete(bookToDelete);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
