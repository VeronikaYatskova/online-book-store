using AutoMapper;
using Requests.BLL.DTOs.Requests;
using Requests.BLL.DTOs.Responses;
using Requests.DAL.Models;
using RequestsEmailServices.Communication.Models;

namespace Requests.BLL.Profiles
{
    public class RequestProfile : Profile
    {
        public RequestProfile()
        {
            CreateMap<Request, RequestCreatedMessage>();
            CreateMap<AddRequestDto, Request>();
            CreateMap<Request, GetRequestsDto>();
        }
    }
}
