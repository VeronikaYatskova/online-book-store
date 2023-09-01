using System;
using System.Threading.Tasks;
using FluentAssertions;
using FluentValidation;
using Moq;
using OnlineBookStore.Exceptions.Exceptions;
using Profiles.Application.DTOs.Request;
using Profiles.Application.Features.Users.Commands.DeleteUser;
using Profiles.Application.Interfaces.Repositories;
using Profiles.Domain.Entities;
using Xunit;

namespace Profiles.Tests.Members.Commands
{
    public class DeleteUserCommandHandlerTests
    {
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly Mock<IValidator<DeleteUserCommand>> _mockValidator;
        private readonly DeleteUserCommandHandler _handler;

        public DeleteUserCommandHandlerTests()
        {
            _mockUserRepository = new ();
            _mockValidator = new ();
            _handler = new DeleteUserCommandHandler(_mockUserRepository.Object, _mockValidator.Object);
        }

        [Theory]
        [InlineData("78d5379d-24f1-4cc7-88ba-9e8a75439b91")]
        [InlineData("bf80f346-2d28-47cb-a4ba-aa56b89e65c9")]
        [InlineData("542447bb-7ff6-4cde-9cde-89e483af799e")]
        [InlineData("de154b51-aaed-4335-a4f8-925fa5b4e753")]
        public async Task DeleteUser_UserExist_ShouldSucceed(string userId)
        {
            // Arrange
            var deleteUserRequest = new DeleteUserRequest { UserId = userId };
            
            var userToDelete = new User 
            {
                Id = Guid.Parse(userId),
                Email = "user@gmail.com",
                FirstName = "user",
                LastName = "user",
                RoleId = Guid.NewGuid(),
            };

            var command = new DeleteUserCommand(deleteUserRequest);

            _mockUserRepository.Setup(u => u.GetUserByIdAsync(Guid.Parse(deleteUserRequest.UserId)))
                .ReturnsAsync(userToDelete);
            _mockUserRepository.Setup(u => u.DeleteUserAsync(userToDelete.Id))
                .Verifiable(Times.Once);
                
            // Act
            var act = async () => await _handler.Handle(command, default);

            // Assert
            await act.Should().NotThrowAsync<NotFoundException>();
        }

        [Theory]
        [InlineData("339d1ee6-e326-4919-bd8f-d5f45d1d7a3d")]
        [InlineData("9478e21c-43c2-486c-aa1a-3dac96353b80")]
        [InlineData("d3bef149-e43c-4899-80cd-f93cdf743504")]
        [InlineData("c8389ec5-c314-490c-a9eb-eb0b16b9e8c6")]
        public async Task DeleteUser_UserDoesNotExist_ShouldThrowNotFoundException(string userId)
        {
            // Arrange
            var deleteUserRequest = new DeleteUserRequest { UserId = userId };
            
            var command = new DeleteUserCommand(deleteUserRequest);

            _mockUserRepository.Setup(u => u.GetUserByIdAsync(Guid.Parse(deleteUserRequest.UserId)))
                .ThrowsAsync(new NotFoundException(ExceptionMessages.UserNotFoundByIdMessage));

            // Act
            var act = async () => await _handler.Handle(command, default);

            // Assert
            await act.Should().ThrowAsync<NotFoundException>();
        }
    }
}
