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
        private readonly IAwsS3Service _awsS3Service;

        public DeleteBookCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IAwsS3Service awsS3Service)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _awsS3Service = awsS3Service;
        }

        public async Task Handle(DeleteBookCommand request, CancellationToken cancellationToken)
        {
            var bookId = Guid.Parse(request.BookId);
            var bookToDelete = await _unitOfWork.BooksRepository
                .FindByConditionAsync(b => b.BookGuid == bookId) ??
                    throw new NotFoundException(ExceptionMessages.BookNotFound);;

            _unitOfWork.BooksRepository.Delete(bookToDelete);
            
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
