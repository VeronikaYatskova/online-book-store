namespace BookStore.Domain.Entities
{
    public class AuthorEntity
    {
        public Guid AuthorGuid { get; set; }
        public string AuthorName { get; set; } = default!;
        public string AuthorLastName { get; set; } = default!;
    }
}