using Auth.Application.PipelineBehaviors;
using FluentValidation;

namespace Auth.Application.Features.User.Commands.LoginUser
{
    public class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
    {
        public LoginUserCommandValidator()
        {
            RuleFor(u => u.request.Email)
                .NotEmpty()
                .WithMessage(ValidationMessages.InvalidEmailMessage);

            RuleFor(u => u.request.Email)
                .Must(IsValidEmail)
                .WithMessage(ValidationMessages.InvalidEmailMessage);
            
            RuleFor(u => u.request.Password)
                .NotEmpty()
                .WithMessage(ValidationMessages.InvalidPasswordMessage);
        }

        private bool IsValidEmail(string email)
        {
            return email.Contains("@");
        }
    }
}
