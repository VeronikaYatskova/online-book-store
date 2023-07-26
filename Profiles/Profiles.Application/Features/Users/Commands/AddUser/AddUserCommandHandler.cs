using AutoMapper;
using FluentValidation;
using MediatR;
using Profiles.Application.Interfaces.Repositories;
using Profiles.Domain.Entities;

namespace Profiles.Application.Features.Users.Commands.AddUser
{
    public class AddUserCommandHandler : IRequestHandler<AddUserCommand>
    {
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;
        private readonly IValidator<AddUserCommand> validator;

        public AddUserCommandHandler(
            IUserRepository userRepository, 
            IMapper mapper,
            IValidator<AddUserCommand> validator)
        {
            this.userRepository = userRepository;
            this.mapper = mapper;
            this.validator = validator;
        }

        public async Task Handle(AddUserCommand request, CancellationToken cancellationToken)
        {
            await validator.ValidateAndThrowAsync(request);

            var user = mapper.Map<User>(request.UserData);
            
            await userRepository.AddUserAsync(user);
        }
    }
}
