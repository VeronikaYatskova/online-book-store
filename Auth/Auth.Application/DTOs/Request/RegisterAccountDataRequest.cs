namespace Auth.Application.DTOs.Request
{
    public class RegisterAccountDataRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string ReEnteredPassword { get; set; } = string.Empty;
    }
}
