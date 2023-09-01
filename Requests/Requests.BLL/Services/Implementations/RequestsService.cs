using AutoMapper;
using MassTransit;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<RequestsService> _logger;

        public RequestsService(
            IRequestsRepository requestsRepository,
            IBlobStorageService blobStorageService,
            IMapper mapper,
            IUserRepository userRepository,
            IPublishEndpoint publishEndpoint,
            IRequestClient<BookPublishingMessage> requestClient,
            ILogger<RequestsService> logger)
        {
            _requestsRepository = requestsRepository;
            _mapper = mapper;
            _userRepository = userRepository;
            _publishEndpoint = publishEndpoint;
            _blobStorageService = blobStorageService;
            _requestClient = requestClient;
            _logger = logger;
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
            _logger.LogInformation($"{addRequestDto.UserId} {addRequestDto.PublisherId}");

            var request = _mapper.Map<Request>(addRequestDto);
            
            var fileFakeName = Guid.NewGuid();
            var fileExtension = Path.GetExtension(addRequestDto.File.FileName);

            request.BookFakeName = string.Format("{0}{1}", fileFakeName, fileExtension);

            await _requestsRepository.AddAsync(request);

            await _blobStorageService.UploadAsync(addRequestDto.File, request.BookFakeName);

            var fakePublisher = new User
            {
                Id = "64d8e3c9b5eb70d5f5958b22",
                Email = "fakepublisher@gmail.com",
                RoleId = "02a27bd4-960f-4f30-9c79-51be15e219b5"
            };

            await _userRepository.AddUserAsync(fakePublisher);

            var publisher = await _userRepository.GetByConditionAsync(c => c.Id == addRequestDto.PublisherId) ??
                throw new ArgumentNullException("publisher is null");

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
