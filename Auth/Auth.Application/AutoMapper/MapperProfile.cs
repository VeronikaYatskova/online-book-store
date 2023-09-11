using Auth.Application.DTOs.Request;
using Auth.Application.DTOs.Response;
using Auth.Domain.Models;
using AutoMapper;
using OnlineBookStore.Messages.Models.Messages;

namespace Auth.Application.AutoMapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<User, GetUsersResponse>()
                .ForMember(u => u.UserGuid,
                x => x.MapFrom(ur => ur.Id.ToString()));
            CreateMap<LoginUserRequest, User>();
            CreateMap<RegisterUserRequest, User>();
            CreateMap<User, UserRegisteredMessage>();
            CreateMap<RegisterUserRequest, UserRegisteredMessage>();
        }
    }
}
