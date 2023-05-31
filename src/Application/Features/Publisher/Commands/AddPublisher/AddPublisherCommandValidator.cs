using FluentValidation;

namespace Application.Features.Publisher.Commands.AddPublisher
{
    public class AddPublisherCommandValidator :  AbstractValidator<AddPublisherCommand>
    {
        public AddPublisherCommandValidator()
        {
            RuleFor(p => p.publisher.PublisherName).NotEmpty();
        }
    }
}
