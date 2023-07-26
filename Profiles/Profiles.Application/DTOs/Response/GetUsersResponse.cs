namespace Profiles.Application.DTOs.Response
{
    public class GetUsersResponse
    {
        public string Id { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;  
    }
}