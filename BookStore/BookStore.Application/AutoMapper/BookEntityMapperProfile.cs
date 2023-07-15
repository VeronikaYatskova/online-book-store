using BookStore.Application.DTOs.Request;
using BookStore.Application.DTOs.Response;
using AutoMapper;
using BookStore.Domain.Entities;

namespace BookStore.Application.AutoMapper
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
