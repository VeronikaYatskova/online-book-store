using AutoMapper;
using MassTransit;
using OnlineBookStore.Messages.Models.Messages;
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
        private readonly IUserRepository _userRepository;
        private readonly IBlobStorageService _blobStorageService;
        private readonly IMapper _mapper;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IRequestClient<BookPublishingMessage> _requestClient;

        public RequestsService(
            IRequestsRepository requestsRepository,
            IBlobStorageService blobStorageService,
            IMapper mapper,
            IUserRepository userRepository,
            IPublishEndpoint publishEndpoint,
            IRequestClient<BookPublishingMessage> requestClient)
        {
            _requestsRepository = requestsRepository;
            _mapper = mapper;
            _userRepository = userRepository;
            _publishEndpoint = publishEndpoint;
            _blobStorageService = blobStorageService;
            _requestClient = requestClient;
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

        public async Task<IEnumerable<GetRequestsDto>> GetPublishersRequests(string publisherId)
        {
            var requests = await _requestsRepository.GetByConditionAsync(r => r.PublisherId == publisherId);
            var requestsDto = _mapper.Map<IEnumerable<GetRequestsDto>>(requests);

            return requestsDto;
        }

        public async Task<IEnumerable<GetRequestsDto>> GetUsersRequests(string userId)
        {
            var requests = await _requestsRepository.GetByConditionAsync(r => r.UserId == userId);
            var requestsDto = _mapper.Map<IEnumerable<GetRequestsDto>>(requests);

            return requestsDto;
        }

        public async Task AddRequestAsync(AddRequestDto addRequestDto)
        {
            var request = _mapper.Map<Request>(addRequestDto);
            
            var fileFakeName = Guid.NewGuid();
            var fileExtension = Path.GetExtension(addRequestDto.File.FileName);

            request.BookFakeName = string.Format("{0}{1}", fileFakeName, fileExtension);

            await _requestsRepository.AddAsync(request);

            await _blobStorageService.UploadAsync(addRequestDto.File, request.BookFakeName);

            var publisher = await _userRepository.GetByConditionAsync(c => c.Id == addRequestDto.PublisherId);
            
            var requestCreatedMessage = _mapper.Map<RequestCreatedMessage>(request);
            requestCreatedMessage.PublisherEmail = publisher.Email;

            await _publishEndpoint.Publish(requestCreatedMessage);
        }

        public async Task UpdateRequestAsync(UpdateRequestDto updateRequestDto)
        {
            var requestToUpdate = await _requestsRepository
                .GetByConditionAsync(r => r.Id == updateRequestDto.RequestId);
            
            requestToUpdate.IsApproved = updateRequestDto.IsApproved;

            await _requestsRepository.UpdateAsync(requestToUpdate);
            
            var requestUpdatedMessage = _mapper.Map<RequestUpdatedMessage>(requestToUpdate);
            var user = await _userRepository.GetByConditionAsync(u => u.Id == requestToUpdate.UserId);

            requestUpdatedMessage.UserEmail = user.Email;
            
            await _publishEndpoint.Publish(requestUpdatedMessage);
        }

        public async Task DeleteRequestAsync(DeleteRequestDto deleteRequestDto)
        {
            await _requestsRepository.DeleteAsync(deleteRequestDto.RequestId);
        }

        public async Task PublishBookAsync(string requestId, AddBookDto addBookDto)
        {
            var request = await _requestsRepository.GetByConditionAsync(r => r.Id == requestId);
            
            var bookPublishingMessage = _mapper.Map<BookPublishingMessage>(addBookDto);
            bookPublishingMessage.BookFakeName = request.BookFakeName;
            
            var response = await _requestClient.GetResponse<BookPublishedMessage>(bookPublishingMessage);

            if (response.Message.StatusCode == 200)
            {
                await _requestsRepository.DeleteAsync(requestId);
            }
        }
    }
}
