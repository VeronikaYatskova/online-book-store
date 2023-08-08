using AutoMapper;
using Requests.BLL.DTOs.Requests;
using Requests.BLL.DTOs.Responses;
using Requests.DAL.Models;

namespace Requests.BLL.Profiles
{
    public class RequestProfile : Profile
    {
        public RequestProfile()
        {
            CreateMap<AddRequestDto, Request>();
            CreateMap<Request, GetRequestsDto>();
        }
    }
}
