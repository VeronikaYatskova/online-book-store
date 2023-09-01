using System;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using FluentValidation;
using Moq;
using OnlineBookStore.Exceptions.Exceptions;
using Profiles.Application.DTOs.Request;
using Profiles.Application.Features.Users.Commands.EditUser;
using Profiles.Application.Interfaces.Repositories;
using Profiles.Domain.Entities;
using Profiles.Tests.Members.Data;
using Xunit;

namespace Profiles.Tests.Members.Commands
{
    public class EditUserCommandHandlerTests
    {
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IValidator<EditUserCommand>> _mockValidator;
        private readonly EditUserCommandHandler _handler;

        public EditUserCommandHandlerTests()
        {
            _mockUserRepository = new ();
            _mockMapper = new ();
            _mockValidator = new ();

            _handler = new EditUserCommandHandler(
                _mockUserRepository.Object,
                _mockMapper.Object,
                _mockValidator.Object);
        }

        [Theory]
        [ClassData(typeof(EditUserCommandValidTestData))]
        public async Task UpdateUser_UserExist_ShouldSucceed(EditUserRequest editUserRequest)
        {
            // Arrange
            var userToUpdate = new User
            {
                Id = Guid.Parse(editUserRequest.Id),
                Email = "user",
                FirstName = "user",
                LastName = "user",
                RoleId =  Guid.Parse(editUserRequest.Id),
            };

            var mapperResult = new User
            {
                Id = Guid.Parse(editUserRequest.Id),
                Email = editUserRequest.Email,
                FirstName = editUserRequest.FirstName,
                LastName = editUserRequest.LastName,
                RoleId =  Guid.Parse(editUserRequest.Id),
            };

            var command = new EditUserCommand(editUserRequest);

            _mockMapper.Setup(m => m.Map<User>(editUserRequest))
                .Returns(mapperResult);
            _mockUserRepository.Setup(u => u.GetUserByIdAsync(userToUpdate.Id))
                .ReturnsAsync(userToUpdate);
            _mockUserRepository.Setup(u => u.UpdateUserAsync(userToUpdate)) // ???
                .Verifiable(Times.Once);
            
            // Act
            var act = async () => await _handler.Handle(command, default);

            // Arrange
            await act.Should().NotThrowAsync();
        }

        [Theory]
        [ClassData(typeof(EditUserCommandUserDoesNotExistTestData))]
        public async Task UpdateUser_UserDoesNotExist_ShouldThrowNotFoundException(EditUserRequest editUserRequest)
        {
            // Arrange
            var userToUpdate = new User
            {
                Id = Guid.Parse(editUserRequest.Id),
                Email = "user",
                FirstName = "user",
                LastName = "user",
                RoleId =  Guid.Parse(editUserRequest.Id),
            };

            var mapperResult = new User
            {
                Id = Guid.Parse(editUserRequest.Id),
                Email = editUserRequest.Email,
                FirstName = editUserRequest.FirstName,
                LastName = editUserRequest.LastName,
                RoleId =  Guid.Parse(editUserRequest.Id),
            };

            var command = new EditUserCommand(editUserRequest);

            _mockMapper.Setup(m => m.Map<User>(editUserRequest))
                .Returns(mapperResult);
            _mockUserRepository.Setup(u => u.GetUserByIdAsync(userToUpdate.Id))
                .ThrowsAsync(new NotFoundException(ExceptionMessages.UserNotFoundByIdMessage));
            
            // Act
            var act = async () => await _handler.Handle(command, default);

            // Arrange
            await act.Should().ThrowAsync<NotFoundException>();
        }
    }
}
