using BookStore.Application.DTOs.Response;
using AutoMapper;
using BookStore.Domain.Entities;

namespace BookStore.Application.AutoMapper
{
    public class PublisherEntityMapperProfile : Profile
    {
        public PublisherEntityMapperProfile()
        {
            CreateMap<PublisherEntity, PublisherDto>();        
        }
    }
}
