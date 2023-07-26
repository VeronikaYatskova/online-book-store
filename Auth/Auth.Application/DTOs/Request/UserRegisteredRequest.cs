namespace Auth.Application.DTOs.Request
{
    public class UserRegisteredRequest
    {
        public string Event { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
    }
}
