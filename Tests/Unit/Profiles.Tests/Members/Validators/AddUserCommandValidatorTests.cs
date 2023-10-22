using FluentValidation.TestHelper;
using Profiles.Application.DTOs.Request;
using Profiles.Application.Features.Users.Commands.AddUser;
using Profiles.Application.PipelineBehaviour;
using Xunit;

namespace Profiles.Tests.Members.Validators
{
    public class AddUserCommandValidatorTests
    {
        private readonly AddUserCommandValidator _validator;

        public AddUserCommandValidatorTests()
        {
            _validator = new AddUserCommandValidator();
        }

        [Theory]
        [InlineData("user1@gmail.com", "Veronika", "Veronika")]
        [InlineData("user2@mail.com", "User1", "User1User1")]
        [InlineData("user3@gmail.com", "User2", "User2")]
        [InlineData("user4@mail.com", "User3User3", "User3User3User3User3")]
        public void Should_not_have_error_when_email_is_specified(string email, string firstName, string lastName)
        {
            // Arrange
            var userRequest = new AddUserRequest()
            {
                Email = email,
                FirstName = firstName,
                LastName = lastName,
            };
            
            var command = new AddUserCommand(userRequest);

            // Act
            var validationResult = _validator.TestValidate(command);

            // Assert
            validationResult.ShouldNotHaveValidationErrorFor(u => u.UserData.Email);
        }

        [Theory]
        [InlineData("", "Veronika", "Veronika")]
        [InlineData("user2mail.com", "User1", "User1User1")]
        [InlineData("mail.com", "User2", "User2")]
        [InlineData("umail.com", "User3User3", "User3User3User3User3")]
        public void Should_have_error_when_email_is_invalid(string email, string firstName, string lastName)
        {
            // Arrange
            var userRequest = new AddUserRequest()
            {
                Email = email,
                FirstName = firstName,
                LastName = lastName,
            };
            
            var command = new AddUserCommand(userRequest);

            // Act
            var validationResult = _validator.TestValidate(command);

            // Assert
            validationResult.ShouldHaveValidationErrorFor(u => u.UserData.Email)
                .WithErrorMessage(ValidationMessages.InvalidEmailMessage);
        }
        
        [Theory]
        [InlineData("", "Veronika", "Veronika")]
        [InlineData("user2mail.com", "", "User1User1")]
        [InlineData("mail.com", "", "")]
        [InlineData("", "", "")]
        public void Should_have_error_when_one_of_the_fields_is_empty(string email, string firstName, string lastName)
        {
            // Arrange
            var userRequest = new AddUserRequest()
            {
                Email = email,
                FirstName = firstName,
                LastName = lastName,
            };
            
            var command = new AddUserCommand(userRequest);

            // Act
            var validationResult = _validator.TestValidate(command);

            // Assert
            validationResult.ShouldHaveAnyValidationError()
                .WithErrorMessage(ValidationMessages.FieldIsRequiredMessage);
        }
    }
}
