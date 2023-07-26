using FluentValidation;
using MediatR;
using Profiles.Application.Interfaces.Repositories;
using Profiles.Domain.Exceptions;

namespace Profiles.Application.Features.Users.Commands.DeleteUser
{
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand>
    {
        private readonly IUserRepository userRepository;
        private readonly IValidator<DeleteUserCommand> validator;

        public DeleteUserCommandHandler(IUserRepository userRepository, IValidator<DeleteUserCommand> validator)
        {
            this.userRepository = userRepository;
            this.validator = validator;
        }

        public async Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            await validator.ValidateAndThrowAsync(request);
            
            var userId = Guid.Parse(request.UserData.UserId);
            var userExist = await userRepository.GetUserByIdAsync(userId) ??
                throw new NotFoundException(ExceptionMessages.UserNotFoundByIdMessage);

            await userRepository.DeleteUserAsync(userId);
        }
    }
}
