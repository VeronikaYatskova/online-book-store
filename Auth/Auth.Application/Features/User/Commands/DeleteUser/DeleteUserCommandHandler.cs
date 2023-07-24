using Auth.Application.Abstractions.Interfaces.Repositories;
using Auth.Domain.Exceptions;
using MediatR;

namespace Auth.Application.Features.User.Commands.DeleteUser
{
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand>
    {
        private readonly IUserRepository userRepository;

        public DeleteUserCommandHandler(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public async Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var user = await userRepository.FindUserByAsync(u => u.Email == request.Email) ??
                throw new NotFoundException(ExceptionMessages.UserNotFoundMessage);

            userRepository.DeleteUser(user);
            await userRepository.SaveUserChangesAsync();
        }
    }
}
