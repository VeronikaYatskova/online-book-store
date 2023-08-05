using BookStore.Application.Abstractions.Contracts.Interfaces;
using BookStore.Application.DTOs.Response;
using BookStore.Domain.Exceptions;
using AutoMapper;
using MediatR;

namespace BookStore.Application.Features.Category.Queries
{
    public class GetAllCategoriesQueryHandler : IRequestHandler<GetAllCategoriesQuery, IEnumerable<CategoryResponse>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public GetAllCategoriesQueryHandler(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<CategoryResponse>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
        {
            var categories = await _unitOfWork.CategoryRepository.FindAllAsync() ??
                throw new NotFoundException(ExceptionMessages.CategoriesListIsEmptyMessage);;

            return _mapper.Map<IEnumerable<CategoryResponse>>(categories);
        }
    }
}
