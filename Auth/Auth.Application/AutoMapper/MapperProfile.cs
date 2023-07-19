using Auth.Application.DTOs.Request;
using Auth.Application.DTOs.Response;
using Auth.Domain.Models;
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
            CreateMap<RegisterAccountDataRequest, AccountData>();
            CreateMap<UserDataRequest, User>();
            CreateMap<PublisherDataRequest, Publisher>();
            CreateMap<AuthorDataRequest, Author>();
        }
    }
}
