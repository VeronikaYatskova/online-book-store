namespace BookStore.Domain.Entities
{
    public class UserFavoriteBookEntity
    {
        public Guid? Id { get; set; }
        public Guid BookId { get; set; }
        public Guid UserId { get; set; }
    }
}

