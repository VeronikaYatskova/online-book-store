using Auth.Application.DTOs.Request;
using Auth.Application.DTOs.Response;
using Auth.Application.Features.User.Commands.RegisterUser;
using Auth.Domain.Models;
using AutoMapper;
using OnlineBookStore.Messages;

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
