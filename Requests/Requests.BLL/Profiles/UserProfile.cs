using AutoMapper;
using OnlineBookStore.Messages.Models.Messages;
using Requests.BLL.DTOs.Requests;
using Requests.DAL.Models;

namespace Requests.BLL.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserRegisteredMessage, AddUserRequest>();
            CreateMap<AddUserRequest, User>();
        }
    }
}
