using FluentValidation;

namespace BookStore.Application.Features.Book.Commands.AddBook
{
    public class AddBookCommandValidator : AbstractValidator<AddBookCommand>
    {
        public AddBookCommandValidator()
        {
            RuleFor(b => b.BookData.BookName).NotNull();
            // RuleFor(b => b.book.ISBN10).Custom((value, context) => 
            // {
            //     if (isValidIsbn(value))
            //     {
            //         context.AddFailure("ISBN10 is incorrect.");
            //     }
            // });
            // RuleFor(b => b.book.ISBN13).Custom((value, context) => 
            // {
            //     if (isValidIsbn(value))
            //     {
            //         context.AddFailure("ISBN13 is incorrect.");
            //     }
            // });
            RuleFor(b => b.BookData.PagesCount).NotNull().GreaterThan(0);
            RuleFor(b => b.BookData.PublishYear).NotNull();
            RuleFor(b => b.BookData.PublisherGuid).NotNull();
            RuleFor(b => b.BookData.CategoryGuid).NotNull();
            RuleFor(b => b.BookData.Language).NotEmpty();
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
