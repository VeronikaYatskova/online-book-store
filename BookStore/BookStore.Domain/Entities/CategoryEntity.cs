namespace BookStore.Domain.Entities
{
    public class CategoryEntity
    {
        public Guid CategoryGuid { get; set; }
        public string CategoryName { get; set; } = default!;

        public IEnumerable<BookEntity> Books { get; set; } = default!;
    }
}
