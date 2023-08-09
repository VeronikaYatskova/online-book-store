using AutoMapper;
using MassTransit;
using Requests.BLL.DTOs.Requests;
using Requests.BLL.DTOs.Responses;
using Requests.BLL.Services.Interfaces;
using Requests.DAL.Models;
using Requests.DAL.Repositories.Interfaces;
using RequestsEmailServices.Communication.Models;

namespace Requests.BLL.Services.Implementations
{
    public class RequestsService : IRequestsService
    {
        private readonly IRequestsRepository _requestsRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IPublishEndpoint _publishEndpoint;

        public RequestsService(
            IRequestsRepository requestsRepository,
            IMapper mapper,
            IUserRepository userRepository,
            IPublishEndpoint publishEndpoint)
        {
            _requestsRepository = requestsRepository;
            _mapper = mapper;
            _userRepository = userRepository;
            _publishEndpoint = publishEndpoint;
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

            var publisher = await _userRepository.GetByConditionAsync(c => c.Id == addRequestDto.PublisherId);
            
            var requestCreatedMessage = _mapper.Map<RequestCreatedMessage>(request);
            requestCreatedMessage.PublisherEmail = publisher.Email;

            await _publishEndpoint.Publish(requestCreatedMessage);
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
