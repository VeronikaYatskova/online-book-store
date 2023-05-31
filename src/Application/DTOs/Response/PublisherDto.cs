using Domain.Entities;

namespace Application.DTOs.Response
{
    public class PublisherDto
    {
        public Guid PublisherGuid { get; set; }
        public string PublisherName { get; set; } = default!;

        public virtual IList<BookEntity> Books { get; set; } = default!;
    }
}