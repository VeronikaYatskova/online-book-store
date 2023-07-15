namespace BookStore.Domain.Entities
{
    public class BookEntity
    {
        public Guid BookGuid { get; set; }
        public string BookName { get; set; } = default!;
        public string? BookFakeName { get; set; } = default;
        public string ISBN10 { get; set; } = default!;
        public string ISBN13 { get; set; } = default!;
        public int PagesCount { get; set; }
        public string? BookPictureURL { get; set; }
        public string PublishYear { get; set; } = default!;
        public string Language { get; set; } = default!;

        public Guid PublisherGuid { get; set; }
        public virtual PublisherEntity Publisher { get; set; } = default!;

        public Guid CategoryGuid { get; set; }
        public virtual CategoryEntity Category { get; set; } = default!;

        public virtual ICollection<UserFavoriteBookEntity>? FavoriteBooks { get; set; }
    }
}
