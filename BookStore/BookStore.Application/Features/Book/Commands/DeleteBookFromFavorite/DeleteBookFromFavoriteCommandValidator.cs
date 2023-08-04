using FluentValidation;

namespace BookStore.Application.Features.Book.Commands.DeleteBookFromFavorite
{
    public class DeleteBookFromFavoriteCommandValidator : AbstractValidator<DeleteBookFromFavoriteCommand>
    {
        public DeleteBookFromFavoriteCommandValidator()
        {
            RuleFor(fb => fb.UserId).NotEmpty();
            RuleFor(fb => fb.BookId).NotEmpty();
        }
    }
}