namespace Requests.BLL.DTOs.Requests
{
    public class AddUserRequest
    {
        public string Email { get; set; } = default!;
        public string RoleId { get; set; } = default!;
    }
}
