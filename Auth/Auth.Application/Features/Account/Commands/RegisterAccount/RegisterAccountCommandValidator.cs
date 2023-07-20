using Auth.Application.PipelineBehaviors;
using FluentValidation;

namespace Auth.Application.Features.Account.Commands.RegisterAccount
{
    public class RegisterAccountCommandValidator : AbstractValidator<RegisterAccountCommand>
    {
        public RegisterAccountCommandValidator()
        {
            RuleFor(u => u.RegisterAccountDataRequest.Email)
                .NotNull()
                .NotEmpty()
                .WithMessage(ValidationMessages.FieldIsRequiredMessage);

            RuleFor(u => u.RegisterAccountDataRequest.Email)
                .Must(IsValidEmail)
                .WithMessage(ValidationMessages.InvalidEmailMessage);
            
            RuleFor(u => u.RegisterAccountDataRequest.Password)
                .NotEmpty()
                .Must(IsValidPassword)
                .WithMessage(ValidationMessages.InvalidPasswordMessage);
            
            RuleFor(u => u.RegisterAccountDataRequest.ReEnteredPassword)
                .NotEmpty()
                .Must((user, reEnteredPassword) => 
                    ReEnteredPasswordIsUnique(
                        user.RegisterAccountDataRequest.Password, reEnteredPassword))
                .WithMessage(ValidationMessages.ReEnteredPasswordDoesntCoincideWithPasswordMessage);
        }

        private bool ReEnteredPasswordIsUnique(string password, string reEnteredPassword)
        {
            return object.Equals(password, reEnteredPassword);
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
