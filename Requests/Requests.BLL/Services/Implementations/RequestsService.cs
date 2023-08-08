using AutoMapper;
using Requests.BLL.DTOs.Requests;
using Requests.BLL.DTOs.Responses;
using Requests.BLL.Services.Interfaces;
using Requests.DAL.Models;
using Requests.DAL.Repositories.Interfaces;

namespace Requests.BLL.Services.Implementations
{
    public class RequestsService : IRequestsService
    {
        private readonly IRequestsRepository _requestsRepository;
        private readonly IMapper _mapper;

        public RequestsService(IRequestsRepository requestsRepository, IMapper mapper)
        {
            _requestsRepository = requestsRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GetRequestsDto>> GetRequestsAsync()
        {
            var requestDtos = await _requestsRepository.GetAllAsync();
            var requests = _mapper.Map<IEnumerable<GetRequestsDto>>(requestDtos);

            return requests; 
        }

        public async Task<GetRequestsDto> GetRequestByIdAsync(string requestId)
        {
            var request = await _requestsRepository.GetByConditionAsync(r => r.Id == requestId);
            var requestDto = _mapper.Map<GetRequestsDto>(request);

            return requestDto;
        }

        public async Task AddRequestAsync(AddRequestDto addRequestDto)
        {
            var request = _mapper.Map<Request>(addRequestDto);
            
            await _requestsRepository.AddAsync(request);
        }

        public async Task UpdateRequestAsync(UpdateRequestDto updateRequestDto)
        {
            var requestToUpdate = await _requestsRepository
                .GetByConditionAsync(r => r.Id == updateRequestDto.RequestId);
            
            requestToUpdate.IsApproved = true;

            await _requestsRepository.UpdateAsync(requestToUpdate);
        }

        public async Task DeleteRequestAsync(DeleteRequestDto deleteRequestDto)
        {
            await _requestsRepository.DeleteAsync(deleteRequestDto.RequestId);
        }
    }
}
