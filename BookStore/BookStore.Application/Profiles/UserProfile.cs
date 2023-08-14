using AuthProfilesServices.Communication.Models;
using AutoMapper;
using BookStore.Domain.Entities;

namespace BookStore.Application.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserRegisteredMessage, User>();
        }
    }
}