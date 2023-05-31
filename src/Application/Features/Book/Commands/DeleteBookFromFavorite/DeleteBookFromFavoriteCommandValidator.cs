using FluentValidation;

namespace Application.Features.Book.Commands.DeleteBookFromFavorite
{
    public class DeleteBookFromFavoriteCommandValidator : AbstractValidator<DeleteBookFromFavoriteCommand>
    {
        public DeleteBookFromFavoriteCommandValidator()
        {
            RuleFor(fb => fb.userId).NotEmpty();
            RuleFor(fb => fb.bookId).NotEmpty();
        }
    }
}