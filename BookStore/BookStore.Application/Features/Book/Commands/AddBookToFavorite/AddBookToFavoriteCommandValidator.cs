using FluentValidation;

namespace BookStore.Application.Features.Book.Commands.AddBookToFavorite
{
    public class AddBookToFavoriteCommandValidator : AbstractValidator<AddBookToFavoriteCommand>
    {
        public AddBookToFavoriteCommandValidator()
        {
            RuleFor(fb => fb.UserId)
                .NotEmpty();

            RuleFor(fb => fb.BookId)
                .NotEmpty();
        }
    }
}
