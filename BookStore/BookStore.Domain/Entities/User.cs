namespace BookStore.Domain.Entities
{
    public class User
    {
        public Guid UserGuid { get; set; }
        public string Email { get; set; } = default!;
        public byte[]? PasswordHash { get; set; }

        public byte[]? PasswordSalt { get; set; }

        public string Role { get; set; } = default!;

        public string? RefreshToken { get; set; }

        public DateTime? TokenCreated { get; set; }

        public DateTime? TokenExpires { get; set; }

        public string? SocialId { get; set; }
        public string? AuthVia { get; set; }

        public virtual ICollection<BookEntity>? FavoriteBooks { get; set; }
    }
}
