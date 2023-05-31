using System.Text.Json.Serialization;

namespace Domain.Entities
{
    public class CategoryEntity
    {
        public Guid CategoryGuid { get; set; }
        public string CategoryName { get; set; } = default!;

        [JsonIgnore]
        public virtual IList<BookEntity> Books { get; set; } = default!;
    }
}
