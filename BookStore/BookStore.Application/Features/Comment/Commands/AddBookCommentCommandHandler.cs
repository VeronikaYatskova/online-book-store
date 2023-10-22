using AutoMapper;
using BookStore.Application.Abstractions.Contracts.Interfaces;
using MassTransit;
using MediatR;
using OnlineBookStore.Messages.Models.Messages;

namespace BookStore.Application.Features.Comment.Commands
{
    public class AddBookCommentCommandHandler : IRequestHandler<AddBookCommentCommand>
    {
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;

        public AddBookCommentCommandHandler(
            IPublishEndpoint publishEndpoint,
            IMapper mapper, 
            ITokenService tokenService)
        {
            _publishEndpoint = publishEndpoint;
            _mapper = mapper;
            _tokenService = tokenService;
        }

        public async Task Handle(AddBookCommentCommand request, CancellationToken cancellationToken)
        {
            var commentAddedMessage = _mapper.Map<CommentAddedMessage>(request.CommentData);
            
            commentAddedMessage.Date = DateTime.UtcNow;
            commentAddedMessage.UserId = await _tokenService.GetUserIdFromTokenAsync();
            
            await _publishEndpoint.Publish(commentAddedMessage);
        }
    }
}
