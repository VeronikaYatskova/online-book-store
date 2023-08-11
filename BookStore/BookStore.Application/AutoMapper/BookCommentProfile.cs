using AutoMapper;
using BookStore.Application.DTOs.Request;
using BookStoreCommentsServices.Communication.Models;

namespace BookStore.Application.AutoMapper
{
    public class BookCommentProfile : Profile
    {
        public BookCommentProfile()
        {
            CreateMap<BookCommentDto, CommentAddedMessage>();
        }
    }
}
