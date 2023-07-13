using Application.DTOs.Request;
using Application.DTOs.Response;
using AutoMapper;
using Domain.Entities;

namespace Application.AutoMapper
{
    public class BookEntityMapperProfile : Profile
    {
        public BookEntityMapperProfile()
        {
            CreateMap<BookEntity, BookDto>();
            CreateMap<AddBookDto, BookDto>();
            CreateMap<AddBookDto, BookEntity>();
        }
    }
}
