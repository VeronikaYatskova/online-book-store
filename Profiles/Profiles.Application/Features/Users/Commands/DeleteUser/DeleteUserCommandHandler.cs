using FluentValidation;
using MediatR;
using Profiles.Application.Interfaces.Repositories;
using Profiles.Domain.Exceptions;

namespace Profiles.Application.Features.Users.Commands.DeleteUser
{
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly IValidator<DeleteUserCommand> _validator;

        public DeleteUserCommandHandler(
            IUserRepository userRepository, 
            IValidator<DeleteUserCommand> validator)
        {
            _userRepository = userRepository;
            _validator = validator;
        }

        public async Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            await _validator.ValidateAndThrowAsync(request);
            
            var userId = Guid.Parse(request.UserData.UserId);
            var existedUser = await _userRepository.GetUserByIdAsync(userId) ??
                throw new NotFoundException(ExceptionMessages.UserNotFoundByIdMessage);

            await _userRepository.DeleteUserAsync(userId);
        }
    }
}
