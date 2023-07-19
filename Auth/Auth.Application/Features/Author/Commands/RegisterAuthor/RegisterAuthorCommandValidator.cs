using Auth.Application.PipelineBehaviors;
using FluentValidation;

namespace Auth.Application.Features.Author.Commands.RegisterAuthor
{
    public class RegisterAuthorCommandValidator : AbstractValidator<RegisterAuthorCommand>
    {
        public RegisterAuthorCommandValidator()
        {
            RuleFor(a => a.AuthorDataRequest.Name)
                .NotEmpty()
                .WithMessage(ValidationMessages.FieldIsRequiredMessage);

            RuleFor(a => a.AuthorDataRequest.Surname)
                .NotEmpty()
                .WithMessage(ValidationMessages.FieldIsRequiredMessage);
        }                
    }
}
