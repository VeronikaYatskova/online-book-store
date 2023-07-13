namespace Auth.Application.DTOs.Request
{
    public class RefreshTokenRequest
    {
        public string Token { get; set; } = default!; 
        public string RefreshToken { get; set; } = default!;        
    }
}
