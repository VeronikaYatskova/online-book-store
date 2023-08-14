using BookStore.Application.DTOs.Request;
using BookStore.Application.DTOs.Response;
using AutoMapper;
using BookStore.Domain.Entities;
using RequestsBookStore.Communication.Models;

namespace BookStore.Application.AutoMapper
{
    public class BookEntityMapperProfile : Profile
    {
        public BookEntityMapperProfile()
        {
            CreateMap<BookEntity, BookDto>()
                .ForMember(b => b.Category,
                    b => b.MapFrom(b => b.Category.CategoryGuid.ToString()))
                .ForMember(b => b.Publisher,
                    b => b.MapFrom(b => b.Publisher.Id.ToString()))
                .ForMember(b => b.Authors,
                    b => b.MapFrom(b => b.BookAuthors.Select(a => a.AuthorGuid.ToString()).ToArray()));
            
            CreateMap<AddBookDto, BookDto>();

            CreateMap<AddBookDto, BookEntity>()
                .ForMember(b => b.CategoryGuid,
                    m => m.MapFrom(ab => Guid.Parse(ab.CategoryGuid)))
                .ForMember(b => b.PublisherGuid,
                    m => m.MapFrom(ab => Guid.Parse(ab.PublisherGuid)));

            CreateMap<BookPublishingMessage, BookEntity>()
                .ForMember(b => b.CategoryGuid,
                    m => m.MapFrom(ab => Guid.Parse(ab.CategoryGuid)))
                .ForMember(b => b.PublisherGuid,
                    m => m.MapFrom(ab => Guid.Parse(ab.PublisherGuid)));
        }
    }
}
