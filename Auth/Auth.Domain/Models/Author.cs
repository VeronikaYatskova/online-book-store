namespace Auth.Domain.Models
{
    public class Author
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public string Surname { get; set; } = default!;

        public Guid AccountDataId { get; set; }
        public virtual AccountData AccountData { get; set; } = default!;
    }
}
