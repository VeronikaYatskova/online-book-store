namespace BookStore.Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = default!;
        public Guid RoleId { get; set; } = default!;

        public IEnumerable<BookEntity>? Books { get; set; }
    }
}
