using FluentValidation;

namespace BookStore.Application.Features.Book.Commands.AddBookToFavorite
{
    public class AddBookToFavoriteCommandValidator : AbstractValidator<AddBookToFavoriteCommand>
    {
        public AddBookToFavoriteCommandValidator()
        {
            RuleFor(fb => fb.userId).NotEmpty();
            RuleFor(fb => fb.bookId).NotEmpty();
        }
    }
}