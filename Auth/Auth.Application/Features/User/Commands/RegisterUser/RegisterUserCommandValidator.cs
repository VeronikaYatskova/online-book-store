using Auth.Application.PipelineBehaviors;
using FluentValidation;

namespace Auth.Application.Features.User.Commands.RegisterUser
{
    public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
    {
        public RegisterUserCommandValidator()
        {
            RuleFor(a => a.UserDataRequest.Name)
                .NotEmpty()
                .WithMessage(ValidationMessages.FieldIsRequiredMessage);
        }
    }
}
