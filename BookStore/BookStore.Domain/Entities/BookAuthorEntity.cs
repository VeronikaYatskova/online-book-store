
namespace BookStore.Domain.Entities
{
    public class BookAuthorEntity
    {
        public Guid Guid { get; set; }
        public Guid BookGuid { get; set; }
        public BookEntity? Book { get; set; }
        public Guid AuthorGuid { get; set; }
        public User? Author { get; set; }
    }
}
