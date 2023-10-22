using AutoMapper;
using FluentValidation;
using MediatR;
using Profiles.Application.Interfaces.Repositories;
using Profiles.Domain.Entities;

namespace Profiles.Application.Features.Users.Commands.AddUser
{
    public class AddUserCommandHandler : IRequestHandler<AddUserCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<AddUserCommand> _validator;

        public AddUserCommandHandler(
            IUserRepository userRepository, 
            IMapper mapper,
            IValidator<AddUserCommand> validator)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task Handle(AddUserCommand request, CancellationToken cancellationToken)
        {
            await _validator.ValidateAndThrowAsync(request);

            var user = _mapper.Map<User>(request.UserData);
            
            await _userRepository.AddUserAsync(user);
        }
    }
}
