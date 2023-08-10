namespace Auth.Domain.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = default!;

        public byte[]? PasswordHash { get; set; }
        public byte[]? PasswordSalt { get; set; }

        public string? RefreshToken { get; set; }
        public DateTime? TokenCreated { get; set; }
        public DateTime? TokenExpires { get; set; }

        public Guid RoleId { get; set; }
        public virtual UserRole Role { get; set; } = default!;
        
        public string? SocialId { get; set; }
        public string? AuthVia { get; set; }
    }
}
