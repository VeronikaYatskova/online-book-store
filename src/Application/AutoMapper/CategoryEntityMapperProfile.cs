using Application.DTOs.Response;
using AutoMapper;
using Domain.Entities;

namespace Application.AutoMapper
{
    public class CategoryEntityMapperProfile : Profile
    {
        public CategoryEntityMapperProfile()
        {
            CreateMap<CategoryEntity, CategoryResponse>();
        }
    }
}