using BookStore.Application.DTOs.Response;
using AutoMapper;
using BookStore.Domain.Entities;

namespace BookStore.Application.Profiles
{
    public class CategoryEntityProfile : Profile
    {
        public CategoryEntityProfile()
        {
            CreateMap<CategoryEntity, CategoryResponse>();
        }
    }
}
