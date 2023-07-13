using Application.DTOs.Response;
using AutoMapper;
using Domain.Entities;

namespace Application.AutoMapper
{
    public class PublisherEntityMapperProfile : Profile
    {
        public PublisherEntityMapperProfile()
        {
            CreateMap<PublisherEntity, PublisherDto>();        
        }
    }
}
