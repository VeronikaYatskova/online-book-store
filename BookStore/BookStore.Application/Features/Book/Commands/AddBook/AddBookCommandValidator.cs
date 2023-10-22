using BookStore.Application.PipelineBehaviors;
using FluentValidation;

namespace BookStore.Application.Features.Book.Commands.AddBook
{
    public class AddBookCommandValidator : AbstractValidator<AddBookCommand>
    {
        public AddBookCommandValidator()
        {
            RuleFor(b => b.BookData.BookName)
                .NotNull()
                .WithMessage(ValidationMessages.FieldIsRequiredMessage);
            
            // RuleFor(b => b.BookData.ISBN10).Custom((value, context) => 
            // {
            //     if (isValidIsbn(value))
            //     {
            //         context.AddFailure("ISBN10 is incorrect.");
            //     }
            // });

            // RuleFor(b => b.BookData.ISBN13).Custom((value, context) => 
            // {
            //     if (isValidIsbn(value))
            //     {
            //         context.AddFailure("ISBN13 is incorrect.");
            //     }
            // });

            RuleFor(b => b.BookData.PagesCount)
                .NotNull()
                .WithMessage(ValidationMessages.FieldIsRequiredMessage);

            RuleFor(b => b.BookData.PagesCount)
                .GreaterThan(0)
                .WithMessage(ValidationMessages.InvalidPageCountMessage);

            RuleFor(b => b.BookData.PublishYear)
                .NotNull()
                .WithMessage(ValidationMessages.FieldIsRequiredMessage);

            RuleFor(b => b.BookData.PublisherGuid)
                .NotNull()
                .WithMessage(ValidationMessages.FieldIsRequiredMessage);

            RuleFor(b => b.BookData.CategoryGuid)
                .NotNull()
                .WithMessage(ValidationMessages.FieldIsRequiredMessage);
            
            RuleFor(b => b.BookData.Language)
                .NotEmpty()
                .WithMessage(ValidationMessages.FieldIsRequiredMessage);
        }

        private bool isValidIsbn(string isbn)
        {
            int n = isbn.Length;
            if (n != 10)
                return false;
    
            int sum = 0;
            for (int i = 0; i < 9; i++)
            {
                int digit = isbn[i] - '0';
                
                if (0 > digit || 9 < digit)
                    return false;
                    
                sum += (digit * (10 - i));
            }
    
            char last = isbn[9];
            if (last != 'X' && (last < '0' 
                            || last > '9'))
                return false;
    
            sum += ((last == 'X') ? 10 :
                            (last - '0'));

            return (sum % 11 == 0);
        }
    }
}
