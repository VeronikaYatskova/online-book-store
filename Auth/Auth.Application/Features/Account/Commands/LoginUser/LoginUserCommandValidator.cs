using Auth.Application.PipelineBehaviors;
using FluentValidation;

namespace Auth.Application.Features.Account.Commands.LoginUser
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
                .WithMessage(ValidationMessages.EmptyPasswordMessage);

            RuleFor(u => u.LoginUserData.Password)
                .Must(IsValidPassword)
                .WithMessage(ValidationMessages.InvalidPasswordMessage);
        }

        private bool IsValidEmail(string email)
        {
            return email.Contains("@");
        }

        private bool IsValidPassword(string password)
        {
            return password.Length >= 6 && password.Length <= 15;
        }
    }
}
