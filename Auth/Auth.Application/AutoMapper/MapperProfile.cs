using Auth.Application.DTOs.Request;
using Auth.Application.DTOs.Response;
using Auth.Domain.Models;
using AuthProfilesServices.Communication.Models;
using AutoMapper;

namespace Auth.Application.AutoMapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<User, GetUsersResponse>();
            CreateMap<LoginUserRequest, User>();
            CreateMap<RegisterUserRequest, User>();
            CreateMap<User, UserRegisteredMessage>();
        }
    }
}
