using Requests.BLL.DTOs.Requests;
using Requests.BLL.DTOs.Responses;

namespace Requests.BLL.Services.Interfaces
{
    public interface IRequestsService
    {
        Task<IEnumerable<GetRequestsDto>> GetRequestsAsync();
        Task<GetRequestsDto> GetRequestByIdAsync(string requestId);
        Task AddRequestAsync(AddRequestDto addRequestDto);
        Task UpdateRequestAsync(UpdateRequestDto updateRequestDto);
        Task DeleteRequestAsync(DeleteRequestDto deleteRequestDto);
    }
}
