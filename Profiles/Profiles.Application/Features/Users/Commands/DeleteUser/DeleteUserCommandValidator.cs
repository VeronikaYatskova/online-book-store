using FluentValidation;
using Profiles.Application.PipelineBehaviour;

namespace Profiles.Application.Features.Users.Commands.DeleteUser
{
    public class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
    {
        public DeleteUserCommandValidator()
        {
            RuleFor(u => u.UserData.UserId)
                .NotEmpty()
                .WithMessage(ValidationMessages.FieldIsRequiredMessage);
        }
    }
}
