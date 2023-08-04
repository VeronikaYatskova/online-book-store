using Comments.BLL.DTOs.Request;
using FluentValidation;

namespace Comments.BLL.Validators
{
    public class UpdateCommentRequestValidator : AbstractValidator<UpdateCommentRequest>
    {
        public UpdateCommentRequestValidator()
        {
            RuleFor(c => c.Id)
                .NotEmpty()
                .WithMessage(ValidationMessages.FieldIsRequeiredMessage);
            
            RuleFor(c => c.Text)
                .NotEmpty()
                .WithMessage(ValidationMessages.FieldIsRequeiredMessage);
        }
    }
}
