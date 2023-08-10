using FluentValidation;
using Profiles.Application.PipelineBehaviour;

namespace Profiles.Application.Features.Users.Commands.AddUser
{
    public class AddUserCommandValidator : AbstractValidator<AddUserCommand>
    {
        public AddUserCommandValidator()
        {
            RuleFor(u => u.UserData.Email)
                .NotEmpty()
                .WithMessage(ValidationMessages.FieldIsRequiredMessage);

            RuleFor(u => u.UserData.FirstName)
                .NotEmpty()
                .WithMessage(ValidationMessages.FieldIsRequiredMessage);

            RuleFor(u => u.UserData.LastName)
                .NotEmpty()
                .WithMessage(ValidationMessages.FieldIsRequiredMessage);            
        }
    }
}
