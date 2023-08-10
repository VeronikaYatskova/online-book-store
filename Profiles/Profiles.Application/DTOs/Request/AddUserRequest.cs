namespace Profiles.Application.DTOs.Request
{
    public class AddUserRequest
    {
        public string Email { get; set; } = default!;
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
    }
}
