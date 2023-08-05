namespace Auth.Domain.Exceptions
{
    public class ExceptionMessages
    {
        public const string UserNotFoundMessage = "User is not found.";
        public const string NoUsersMessage = "No users.";
        public const string UserAlreadyExistsMessage = "User with the same email already exists.";
        public const string RoleNotFoundMessage = "User with this role does not exist.";
        public const string WrongPasswordMessage = "Wrong password.";
        public const string InvalidRefreshTokenMessage = "Invalid refresh token.";
        public const string TokenExpiredMessage = "Token is expired.";
    }
}
