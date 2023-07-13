namespace Auth.Domain.Models
{
    public class UserRole
    {
        public Guid UserRoleGuid { get; set; }
        public string Name { get; set; } = default!;

        public virtual List<User> Users { get; set; } = default!;
    }
}
