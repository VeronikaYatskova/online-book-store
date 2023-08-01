using Auth.Application.Abstractions.Interfaces.Repositories;
using Auth.Domain.Exceptions;
using MediatR;

using UserEntity = Auth.Domain.Models.User;

namespace Auth.Application.Features.User.Commands.DeleteUser
{
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand>
    {
        private readonly IRepository<UserEntity> _userRepository;

        public DeleteUserCommandHandler(IRepository<UserEntity> userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.FindByConditionAsync(u => u.Email == request.Email)! ??
                throw new NotFoundException(ExceptionMessages.UserNotFoundMessage);

            _userRepository.Delete(user!);
            await _userRepository.SaveChangesAsync();
        }
    }
}
