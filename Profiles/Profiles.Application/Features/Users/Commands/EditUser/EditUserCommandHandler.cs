using AutoMapper;
using FluentValidation;
using MediatR;
using Profiles.Application.Interfaces.Repositories;
using Profiles.Domain.Entities;
using Profiles.Domain.Exceptions;

namespace Profiles.Application.Features.Users.Commands.EditUser
{
    public class EditUserCommandHandler : IRequestHandler<EditUserCommand>
    {
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;
        private readonly IValidator<EditUserCommand> validator;

        public EditUserCommandHandler(
            IUserRepository userRepository, 
            IMapper mapper,
            IValidator<EditUserCommand> validator)
        {
            this.userRepository = userRepository;
            this.mapper = mapper;
            this.validator = validator;
        }

        public async Task Handle(EditUserCommand request, CancellationToken cancellationToken)
        {
            await validator.ValidateAndThrowAsync(request);

            var user = mapper.Map<User>(request.UserData);
            var userExist = await userRepository.GetUserByIdAsync(user.Id) ??
                throw new NotFoundException(ExceptionMessages.UserNotFoundByIdMessage);  

            await userRepository.UpdateUserAsync(user);
        }
    }
}
