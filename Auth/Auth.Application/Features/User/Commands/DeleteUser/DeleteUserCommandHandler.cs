using Auth.Application.Abstractions.Interfaces.Repositories;
using OnlineBookStore.Exceptions.Exceptions;
using MassTransit;
using MediatR;
using Auth.Domain.Exceptions;
using OnlineBookStore.Messages.Models.Messages;

using UserEntity = Auth.Domain.Models.User;

namespace Auth.Application.Features.User.Commands.DeleteUser
{
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand>
    {
        private readonly IRepository<UserEntity> _userRepository;
        private readonly IPublishEndpoint _publishEndpoint;

        public DeleteUserCommandHandler(
            IRepository<UserEntity> userRepository, 
            IPublishEndpoint publishEndpoint)
        {
            _userRepository = userRepository;
            _publishEndpoint = publishEndpoint;
        }

        public async Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.FindByConditionAsync(u => u.Email == request.Email)! ??
                throw new NotFoundException(ExceptionMessages.UserNotFoundMessage);

            _userRepository.Delete(user!);
            await _userRepository.SaveChangesAsync();

            await _publishEndpoint.Publish(new UserDeletedMessage { UserId = user.Id.ToString() });
        }
    }
}
