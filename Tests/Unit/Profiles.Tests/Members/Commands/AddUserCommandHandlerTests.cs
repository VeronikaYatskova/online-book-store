using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using FluentValidation;
using Moq;
using Profiles.Application.DTOs.Request;
using Profiles.Application.Features.Users.Commands.AddUser;
using Profiles.Application.Interfaces.Repositories;
using Profiles.Domain.Entities;
using Xunit;

namespace Profiles.Tests.Members.Commands
{
    public class AddUserCommandHandlerTests
    {
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IValidator<AddUserCommand>> _mockValidator;
        private readonly AddUserCommandHandler _handler;

        public AddUserCommandHandlerTests()
        {
            _mockUserRepository = new ();
            _mockMapper = new ();
            _mockValidator = new ();

            _handler = new AddUserCommandHandler(
                _mockUserRepository.Object, 
                _mockMapper.Object, 
                _mockValidator.Object);
        }

        [Theory]
        [InlineData("user1@gmail.com", "Veronika", "Veronika")]
        [InlineData("user2@mail.com", "User1", "User1User1")]
        [InlineData("user3@gmail.com", "User2", "User2")]
        [InlineData("user4@mail.com", "User3User3", "User3User3User3User3")]
        public async Task AddUser_ValidData_ShouldSucceed(string email, string firstName, string lastName)
        {
            // Arrange
            var userRequest = new AddUserRequest()
            {
                Email = email,
                FirstName = firstName,
                LastName = lastName,
            };

            var userEntity = new User()
            {
                Email = userRequest.Email,
                FirstName = userRequest.FirstName,
                LastName = userRequest.LastName,
            };

            _mockMapper.Setup(m => m.Map<User>(userRequest))
                .Returns(userEntity);
            
            _mockUserRepository.Setup(x => x.AddUserAsync(userEntity));

            var command = new AddUserCommand(userRequest);

            // Act
            var act = async () => await _handler.Handle(command, default);
            
            // Assert
            await act.Should().NotThrowAsync();
        }
    }
}
