namespace Auth.Domain.Models
{
    public class RefreshToken
    {
        public string Token { get; set; } = default!;
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public DateTime Expired { get; set; }
    }
}
