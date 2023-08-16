using AutoMapper;
using BookStore.Domain.Entities;
using OnlineBookStore.Messages.Models.Messages;

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