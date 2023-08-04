using Comments.BLL.DTOs.Request;
using FluentValidation;

namespace Comments.BLL.Validators
{
    public class AddCommentRequestValidator : AbstractValidator<AddCommentRequest> 
    {
        public AddCommentRequestValidator()
        {
            RuleFor(c => c.BookId)
                .NotEmpty()
                .WithMessage(ValidationMessages.FieldIsRequeiredMessage);
            
            RuleFor(c => c.Text)
                .NotEmpty()
                .WithMessage(ValidationMessages.FieldIsRequeiredMessage);
        }
    }
}
