namespace Auth.Domain.Models
{
    public class User
    {
        public Guid UserGuid { get; set; }
        public string Name { get; set; } = default!;
        public string Email { get; set; } = default!;
        public byte[]? PasswordHash { get; set; }

        public byte[]? PasswordSalt { get; set; }

        public Guid RoleId { get; set; }
        public virtual UserRole Role { get; set; } = default!;

        public string? RefreshToken { get; set; }

        public DateTime? TokenCreated { get; set; }

        public DateTime? TokenExpires { get; set; }

        public string? SocialId { get; set; }
        public string? AuthVia { get; set; }
    }
}
