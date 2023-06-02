namespace Auth.Domain.Exceptions
{
    public class Exceptions
    {
        public static NotFoundException UserNotFound = new("User is not found.");
        public static NotFoundException NoUsers = new("No users.");
        public static AlreadyExistsException UserAlreadyExists = new("User already exists.");
        public static UserNotRegisteredException UserNotRegistered = new("User is not registered.");
    }
}
