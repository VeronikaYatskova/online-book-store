using AutoMapper;
using BookStore.Application.DTOs.Request;
using OnlineBookStore.Messages.Models.Messages;

namespace BookStore.Application.Profiles
{
    public class BookCommentProfile : Profile
    {
        public BookCommentProfile()
        {
            CreateMap<BookCommentDto, CommentAddedMessage>();
        }
    }
}
