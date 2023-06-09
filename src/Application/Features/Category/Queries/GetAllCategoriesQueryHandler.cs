using Application.Abstractions.Contracts.Interfaces;
using Application.DTOs.Response;
using AutoMapper;
using MediatR;

namespace Application.Features.Category.Queries
{
    public class GetAllCategoriesQueryHandler : IRequestHandler<GetAllCategoriesQuery, IEnumerable<CategoryResponse>>
    {
        private readonly IMapper mapper;
        private readonly IUnitOfWork unitOfWork;

        public GetAllCategoriesQueryHandler(IMapper mapper, IUnitOfWork unitOfWork)
        {
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<CategoryResponse>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
        {
            var categories = await unitOfWork.CategoryRepository.GetCategoriesAsync();

            if (categories is null)
            {
                 throw Exceptions.Exceptions.CategoriesListIsEmpty;
            }

            return mapper.Map<IEnumerable<CategoryResponse>>(categories);
        }
    }
}
