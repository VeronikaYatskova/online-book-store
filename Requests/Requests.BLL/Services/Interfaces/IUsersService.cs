using Requests.BLL.DTOs.Requests;

namespace Requests.BLL.Services.Interfaces
{
    public interface IUsersService
    {
        Task AddUserAsync(AddUserRequest addUserRequest);
    }
}