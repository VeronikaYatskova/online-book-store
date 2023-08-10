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
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<EditUserCommand> _validator;

        public EditUserCommandHandler(
            IUserRepository userRepository, 
            IMapper mapper,
            IValidator<EditUserCommand> validator)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task Handle(EditUserCommand request, CancellationToken cancellationToken)
        {
            await _validator.ValidateAndThrowAsync(request);

            var user = _mapper.Map<User>(request.UserData);
            var userExist = await _userRepository.GetUserByIdAsync(user.Id) ??
                throw new NotFoundException(ExceptionMessages.UserNotFoundByIdMessage);  

            await _userRepository.UpdateUserAsync(user);
        }
    }
}
