using AutoMapper;
using Requests.BLL.DTOs.Requests;
using Requests.BLL.Services.Interfaces;
using Requests.DAL.Models;
using Requests.DAL.Repositories.Interfaces;

namespace Requests.BLL.Services.Implementations
{
    public class UsersService : IUsersService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UsersService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task AddUserAsync(AddUserRequest addUserRequest)
        {
            var user = _mapper.Map<User>(addUserRequest);
            await _userRepository.AddUserAsync(user);
        }
    }
}
