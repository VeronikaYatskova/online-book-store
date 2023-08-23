using System.Threading.Tasks;
using FluentValidation.TestHelper;
using Profiles.Application.DTOs.Request;
using Profiles.Application.Features.Users.Commands.EditUser;
using Profiles.Tests.Members.Data;
using Xunit;

namespace Profiles.Tests.Members.Validators
{
    public class EditUserCommandValidatorTests
    {
        private readonly EditUserCommandValidator _validator;

        public EditUserCommandValidatorTests()
        {
            _validator = new EditUserCommandValidator();
        }

        [Theory]
        [ClassData(typeof(EditUserCommandValidTestData))]
        public async Task Should_not_have_error_when_data_is_specified_and_valid(EditUserRequest editUserRequest)
        {
            // Arrange
            var command = new EditUserCommand(editUserRequest);

            // Act
            var validationResult = _validator.TestValidate(command);

            // Assert
            validationResult.ShouldNotHaveValidationErrorFor(u => u.UserData);
        }

        [Theory]
        [ClassData(typeof(EditUserCommandInvalidTestData))]
        public async Task Should_have_error_when_one_of_the_fields_is_empty(EditUserRequest editUserRequest)
        {
            // Arrange
            var command = new EditUserCommand(editUserRequest);

            // Act
            var validationResult = _validator.TestValidate(command);

            // Assert
            validationResult.ShouldHaveAnyValidationError();
        }
    }
}
