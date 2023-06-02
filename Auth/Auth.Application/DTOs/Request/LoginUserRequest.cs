namespace Auth.Application.DTOs.Request
{
    public class LoginUserRequest
    {
        public string Email { get; set; } = default!;
        public string Password { get; set; } = default!;
    }
}