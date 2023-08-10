using AutoMapper;
using Requests.BLL.DTOs.Requests;
using AuthProfilesServices.Communication.Models;
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
