namespace Auth.Domain.Models
{
    public class UserRole
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;

        public virtual List<AccountData> AccountsData { get; set; } = default!;
    }
}
