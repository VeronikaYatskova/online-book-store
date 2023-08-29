namespace BookStore.Domain.Entities
{
    public class BookEntity
    {
        public Guid BookGuid { get; set; }
        public string BookName { get; set; } = default!;
        public string? BookFakeName { get; set; } = default;
        public string? BookCoverFakeName { get; set; } = default!;
        public string ISBN10 { get; set; } = default!;
        public string ISBN13 { get; set; } = default!;
        public int PagesCount { get; set; }
        public string PublishYear { get; set; } = default!;
        public string Language { get; set; } = default!;

        public Guid PublisherGuid { get; set; }
        public User Publisher { get; set; } = default!;

        public Guid CategoryGuid { get; set; }
        public CategoryEntity Category { get; set; } = default!;

        public IEnumerable<UserBookEntity>? UserBooks { get; set; }
        public IEnumerable<BookAuthorEntity> BookAuthors { get; set; } = default!;
    }
}
