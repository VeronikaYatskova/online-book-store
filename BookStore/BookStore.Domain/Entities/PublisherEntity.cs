using System.Text.Json.Serialization;

namespace BookStore.Domain.Entities
{
    public class PublisherEntity
    {
        public Guid PublisherGuid { get; set; }
        public string PublisherName { get; set; } = default!;

        [JsonIgnore]
        public virtual IList<BookEntity> Books { get; set; } = default!;
    }
}
