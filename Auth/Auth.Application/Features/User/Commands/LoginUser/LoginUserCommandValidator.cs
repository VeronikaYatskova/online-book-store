using Auth.Application.PipelineBehaviors;
using FluentValidation;

namespace Auth.Application.Features.User.Commands.LoginUser
{
    public class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
    {
        public LoginUserCommandValidator()
        {
            RuleFor(u => u.LoginUserData.Email)
                .NotEmpty()
                .WithMessage(ValidationMessages.InvalidEmailMessage);

            RuleFor(u => u.LoginUserData.Email)
                .Must(IsValidEmail)
                .WithMessage(ValidationMessages.InvalidEmailMessage);
            
            RuleFor(u => u.LoginUserData.Password)
                .NotEmpty()
                .WithMessage(ValidationMessages.InvalidPasswordMessage);
        }

        private bool IsValidEmail(string email)
        {
            return email.Contains("@");
        }
    }
}
