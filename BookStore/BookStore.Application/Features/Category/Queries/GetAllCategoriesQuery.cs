using BookStore.Application.DTOs.Response;
using MediatR;

namespace BookStore.Application.Features.Category.Queries
{
    public record GetAllCategoriesQuery() : IRequest<IEnumerable<CategoryResponse>>;
}