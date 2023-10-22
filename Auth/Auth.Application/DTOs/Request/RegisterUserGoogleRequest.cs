namespace Auth.Application.DTOs.Request
{
    public class RegisterUserGoogleRequest
    {
        public string Name { get; set; } = default!;
        public string Email { get; set; } = default!;  
        public string Password { get; set; } = default!;   
    }
}
