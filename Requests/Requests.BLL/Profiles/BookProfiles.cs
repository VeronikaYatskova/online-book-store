using AutoMapper;
using Requests.BLL.DTOs.Requests;
using RequestsBookStore.Communication.Models;

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