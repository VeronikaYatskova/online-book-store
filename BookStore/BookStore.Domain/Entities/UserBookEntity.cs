namespace BookStore.Domain.Entities
{
    public class UserBookEntity
    {
        public Guid? Id { get; set; }
        public Guid BookId { get; set; }
        public BookEntity? Book { get; set; }
        public Guid UserId { get; set; }
        public User? User { get; set; }
    }
}

