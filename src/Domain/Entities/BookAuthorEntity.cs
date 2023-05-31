
namespace Domain.Entities
{
    public class BookAuthorEntity
    {
        public Guid Guid { get; set; }
        public Guid BookGuid { get; set; }
        public Guid AuthorGuid { get; set; }
    }
}
