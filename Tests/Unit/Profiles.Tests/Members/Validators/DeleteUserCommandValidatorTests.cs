using System.Threading.Tasks;
using FluentValidation.TestHelper;
using Profiles.Application.DTOs.Request;
using Profiles.Application.Features.Users.Commands.DeleteUser;
using Xunit;

namespace Profiles.Tests.Members.Validators
{
    public class DeleteUserCommandValidatorTests
    {
        private readonly DeleteUserCommandValidator _validator;

        public DeleteUserCommandValidatorTests()
        {
            _validator = new DeleteUserCommandValidator();
        }

        [Theory]
        [InlineData("6fbbb951-9af0-4aa1-be3b-4a77797614fc")]
        [InlineData("152d652b-50ae-4db2-82cc-d733bf1ee2bd")]
        [InlineData("696e13cb-59de-4611-873a-5f11d61fc5f0")]
        [InlineData("de8e8ffd-a6c1-45d9-b1b9-437a85fd141e")]
        public async Task Should_not_have_error_when_userId_is_specified(string userId)
        {
            // Arrange
            var deleteUserRequest = new DeleteUserRequest { UserId = userId };
            var command = new DeleteUserCommand(deleteUserRequest);

            // Act
            var validationResult = _validator.TestValidate(command);

            // Assert
            validationResult.ShouldNotHaveValidationErrorFor(u => u.UserData.UserId);
        }

        [Theory]
        [InlineData("")]
        public async Task Should_have_error_when_userId_is_not_specified(string userId)
        {
            // Arrange
            var deleteUserRequest = new DeleteUserRequest { UserId = userId };
            var command = new DeleteUserCommand(deleteUserRequest);

            // Act
            var validationResult = _validator.TestValidate(command);

            // Assert
            validationResult.ShouldHaveValidationErrorFor(u => u.UserData.UserId);
        }
    }
}
