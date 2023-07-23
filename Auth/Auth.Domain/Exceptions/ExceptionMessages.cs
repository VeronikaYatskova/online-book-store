namespace Auth.Domain.Exceptions
{
    public class ExceptionMessages
    {
        public const string UserNotFoundMessage = "User is not found.";
        public const string NoUsersMessage = "No users.";
        public const string UserAlreadyExistsMessage = "User with the same email already exists.";
        public const string UserNotRegisteredMessage = "User is not registered.";
        public const string RoleNotFoundMessage = "User with this role does not exist.";
    }
}