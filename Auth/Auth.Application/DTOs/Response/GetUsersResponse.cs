namespace Auth.Application.DTOs.Response
{
    public class GetUsersResponse
    {
        public Guid UserGuid { get; set; }
        public string Name { get; set; } = default!;
        public string Email { get; set; } = default!;
    }
}
