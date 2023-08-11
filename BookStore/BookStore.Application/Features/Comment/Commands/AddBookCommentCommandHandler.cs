using AutoMapper;
using BookStoreCommentsServices.Communication.Models;
using MassTransit;
using MediatR;

namespace BookStore.Application.Features.Comment.Commands
{
    public class AddBookCommentCommandHandler : IRequestHandler<AddBookCommentCommand>
    {
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IMapper _mapper;

        public AddBookCommentCommandHandler(IPublishEndpoint publishEndpoint, IMapper mapper)
        {
            _publishEndpoint = publishEndpoint;
            _mapper = mapper;
        }

        public async Task Handle(AddBookCommentCommand request, CancellationToken cancellationToken)
        {
            var commentAddedMessage = _mapper.Map<CommentAddedMessage>(request.CommentData);
            
            commentAddedMessage.Date = DateTime.UtcNow;
            commentAddedMessage.UserId = "fbc02566-87cb-4ac1-a85f-bb339bae4672";

            await _publishEndpoint.Publish(commentAddedMessage);
        }
    }
}
