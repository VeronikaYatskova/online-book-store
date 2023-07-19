namespace Auth.Application.PipelineBehaviors
{
    public class ValidationMessages
    {
        public const string InvalidEmailMessage = "Check your email. It is required."; 
        public const string EmailAlreadyExistsMessage = "Someone already uses this email.";
        public const string InvalidPasswordMessage = "Password is required.";
        public const string InvalidReEnteredPasswordMessage = "Please, reenter the password";
        public const string ReEnteredPasswordDoesntCoincideWithPasswordMessage = "The passwords you have entered do not coincide";
    }
}
