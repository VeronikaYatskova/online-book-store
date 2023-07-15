using BookStore.Application.DTOs.Response;
using AutoMapper;
using BookStore.Domain.Entities;

namespace BookStore.Application.AutoMapper
{
    public class CategoryEntityMapperProfile : Profile
    {
        public CategoryEntityMapperProfile()
        {
            CreateMap<CategoryEntity, CategoryResponse>();
        }
    }
}