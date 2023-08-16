using AutoMapper;
using Comments.DAL.Entities;
using Comments.DAL.Repositories.Interfaces;
using MassTransit;
using Microsoft.Extensions.Logging;
using OnlineBookStore.Messages.Models.Messages;

namespace Comments.BLL.Consumers
{
    public class CommentAddedConsumer : IConsumer<CommentAddedMessage>
    {
        private readonly ICommentsRepository _commentsRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CommentAddedConsumer> _logger;

        public CommentAddedConsumer(ICommentsRepository commentsRepository, IMapper mapper, ILogger<CommentAddedConsumer> logger)
        {
            _commentsRepository = commentsRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<CommentAddedMessage> context)
        {
            _logger.LogInformation("message received");

            var commentEntity = _mapper.Map<Comment>(context.Message);

            await _commentsRepository.AddAsync(commentEntity);
        }
    }
}
