using Application.DTOs.Request;
using Application.DTOs.Response;
using AutoMapper;
using Domain.Entities;

namespace Application.AutoMapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<BookEntity, BookDto>();
            CreateMap<PublisherEntity, PublisherDto>();
            CreateMap<CategoryEntity, CategoryResponse>();
            CreateMap<AddBookDto, BookDto>();
            CreateMap<AddBookDto, BookEntity>();
        }
    }
}
