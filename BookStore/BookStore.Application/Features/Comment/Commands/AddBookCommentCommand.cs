using BookStore.Application.DTOs.Request;
using MediatR;

namespace BookStore.Application.Features.Comment.Commands
{
    public record AddBookCommentCommand(BookCommentDto CommentData) : IRequest;
}