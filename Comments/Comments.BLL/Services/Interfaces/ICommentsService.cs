using Comments.BLL.DTOs.Request;
using Comments.BLL.DTOs.Response;

namespace Comments.BLL.Services.Interfaces
{
    public interface ICommentsService
    {
        Task<IEnumerable<GetCommentByIdResponse>> GetCommentsByBookIdAsync(string bookId);
        Task AddCommentAsync(AddCommentRequest addCommentRequest);
        Task DeleteCommentAsync(string id);
        Task UpdateCommentAsync(UpdateCommentRequest updateCommentRequest);
    }
}
