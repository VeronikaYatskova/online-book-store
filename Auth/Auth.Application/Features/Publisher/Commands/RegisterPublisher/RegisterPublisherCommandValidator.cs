using Auth.Application.PipelineBehaviors;
using FluentValidation;

namespace Auth.Application.Features.Publisher.Commands.RegisterPublisher
{
    public class RegisterPublisherCommandValidator : AbstractValidator<RegisterPublisherCommand>
    {
        public RegisterPublisherCommandValidator()
        {
            RuleFor(a => a.PublisherDataRequest.Name)
                .NotEmpty()
                .WithMessage(ValidationMessages.FieldIsRequiredMessage);

            RuleFor(a => a.PublisherDataRequest.Surname)
                .NotEmpty()
                .WithMessage(ValidationMessages.FieldIsRequiredMessage);           
        }
    }
}
