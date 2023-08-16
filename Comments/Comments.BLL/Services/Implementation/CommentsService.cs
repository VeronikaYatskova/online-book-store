using AutoMapper;
using Comments.BLL.DTOs.Request;
using Comments.BLL.DTOs.Response;
using Comments.BLL.Exceptions;
using Comments.BLL.Services.Interfaces;
using Comments.DAL.Entities;
using Comments.DAL.Repositories.Interfaces;
using OnlineBookStore.Exceptions.Exceptions;

namespace Comments.BLL.Services.Implementation
{
    public class CommentsService : ICommentsService
    {
        private readonly ICommentsRepository _commentsRepository;
        private readonly IMapper _mapper;

        public CommentsService(ICommentsRepository commentsRepository, IMapper mapper)
        {
            _commentsRepository = commentsRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GetCommentByIdResponse>> GetCommentsByBookIdAsync(string bookId)
        {
            var comments = await _commentsRepository.GetAllAsync(b => b.BookId == bookId) ??
                throw new NotFoundException(ExceptionMessages.BookCommentsNotFoundMessage);

            var commentsResponse = _mapper.Map<IEnumerable<GetCommentByIdResponse>>(comments);

            return commentsResponse;
        }

        public async Task AddCommentAsync(AddCommentRequest addCommentRequest)
        {
            var commentEntity = _mapper.Map<Comment>(addCommentRequest);

            commentEntity.Date = DateTime.UtcNow;
            commentEntity.UserId = "fbc02566-87cb-4ac1-a85f-bb339bae4672"; // there should be and id of loged in user

            await _commentsRepository.AddAsync(commentEntity);
        }

        public async Task UpdateCommentAsync(UpdateCommentRequest updateCommentRequest)
        {
            var commentEntity = _mapper.Map<Comment>(updateCommentRequest);
            var oldComment = await _commentsRepository.GetByConditionAsync(c => c.Id == commentEntity.Id);
            
            commentEntity.Date = oldComment.Date;
            commentEntity.UserId = oldComment.UserId;
            commentEntity.BookId = oldComment.BookId;
            
            await _commentsRepository.UpdateAsync(commentEntity);
        }

        public async Task DeleteCommentAsync(string id)
        {
            await _commentsRepository.DeleteAsync(id);
        }
    }
}
