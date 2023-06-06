namespace Domain.Entities
{
    public class EmailData
    {
        public string EmailTo { get; set; } = default!;

        public BookEntity Book { get; set; } = default!;
    }
}
