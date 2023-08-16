using AutoMapper;
using OnlineBookStore.Messages.Models.Messages;
using Profiles.Application.DTOs.Request;
using Profiles.Application.DTOs.Response;
using Profiles.Domain.Entities;

namespace Profiles.Application.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, GetUsersResponse>();
            CreateMap<AddUserRequest, User>();
            CreateMap<EditUserRequest, User>()
                .ForMember(u => u.Id, a => a.MapFrom(au => Guid.Parse(au.Id)));
            CreateMap<UserRegisteredMessage, User>();
        }
    }
}
