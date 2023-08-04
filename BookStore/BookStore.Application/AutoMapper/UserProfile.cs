using AuthProfilesServices.Communication.Models;
using AutoMapper;
using BookStore.Domain.Entities;

namespace BookStore.Application.AutoMapper
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserRegisteredMessage, User>();
        }
    }
}