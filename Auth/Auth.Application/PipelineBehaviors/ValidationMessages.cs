namespace Auth.Application.PipelineBehaviors
{
    public class ValidationMessages
    {
        public const string InvalidEmailMessage = "You've entered an invalid email";
        public const string FieldIsRequiredMessage = "Field is required.";
        public const string EmailAlreadyExistsMessage = "Someone already uses this email.";
        public const string EmptyPasswordMessage = "Password is required.";
        public const string InvalidPasswordMessage = "Password must be greater than 6 and smaller than 15 symbols.";
        public const string InvalidReEnteredPasswordMessage = "Please, reenter the password";
        public const string ReEnteredPasswordDoesntCoincideWithPasswordMessage = "The passwords you have entered do not coincide";
    }
}
