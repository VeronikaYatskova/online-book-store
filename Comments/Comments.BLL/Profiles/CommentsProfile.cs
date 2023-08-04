using AutoMapper;
using Comments.BLL.DTOs.Request;
using Comments.BLL.DTOs.Response;
using Comments.DAL.Entities;

namespace Comments.BLL.Profiles
{
    public class CommentsProfile : Profile
    {
        public CommentsProfile()
        {
            CreateMap<Comment, GetCommentByIdResponse>()
                .ForMember(c => c.Date, 
                    m => m.MapFrom(c => c.Date.ToShortDateString()));

            CreateMap<AddCommentRequest, Comment>();
            CreateMap<UpdateCommentRequest, Comment>(); 
        }
    }
}
