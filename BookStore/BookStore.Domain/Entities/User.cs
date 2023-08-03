namespace BookStore.Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = default!;
        public Guid RoleId { get; set; } = default!;

        public IEnumerable<UserBookEntity>? UserBooks { get; set; }
        public IEnumerable<BookEntity>? PublisherBooks { get; set; }
        public IEnumerable<BookAuthorEntity> BookAuthors { get; set; } = default!;
    }
}
