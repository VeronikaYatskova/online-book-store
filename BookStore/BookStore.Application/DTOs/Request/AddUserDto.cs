namespace BookStore.Application.DTOs.Request
{
    public class AddUserDto
    {
        public Guid UserGuid { get; set; }
        public string Email { get; set; } = default!;
        public string Password { get; set; } = default!;
    }
}
