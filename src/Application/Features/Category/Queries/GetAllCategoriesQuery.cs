using Application.DTOs.Response;
using MediatR;

namespace Application.Features.Category.Queries
{
    public record GetAllCategoriesQuery() : IRequest<IEnumerable<CategoryResponse>>;
}