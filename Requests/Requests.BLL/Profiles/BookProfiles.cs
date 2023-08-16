using AutoMapper;
using OnlineBookStore.Messages.Models.Messages;
using Requests.BLL.DTOs.Requests;

namespace Requests.BLL.Profiles
{
    public class BookProfiles : Profile
    {
        public BookProfiles()
        {
            CreateMap<AddBookDto, BookPublishingMessage>();
        }
    }
}