using Auth.Application.PipelineBehaviors;
using FluentValidation;

namespace Auth.Application.Features.User.Commands.RegisterUser
{
    public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
    {
        public RegisterUserCommandValidator()
        {
            RuleFor(u => u.UserDataRequest.FirstName)
                .NotNull()
                .NotEmpty()
                .WithMessage(ValidationMessages.FieldIsRequiredMessage);
            
            RuleFor(u => u.UserDataRequest.LastName)
                .NotNull()
                .NotEmpty()
                .WithMessage(ValidationMessages.FieldIsRequiredMessage);

            RuleFor(u => u.UserDataRequest.Email)
                .NotNull()
                .NotEmpty()
                .WithMessage(ValidationMessages.FieldIsRequiredMessage);

            RuleFor(u => u.UserDataRequest.Email)
                .Must(IsValidEmail)
                .WithMessage(ValidationMessages.InvalidEmailMessage);
            
            RuleFor(u => u.UserDataRequest.Password)
                .NotEmpty()
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
