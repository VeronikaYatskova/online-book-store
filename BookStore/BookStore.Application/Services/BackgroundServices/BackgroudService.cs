using AutoMapper;
using BookStore.Application.Abstractions.Contracts.Interfaces;
using BookStore.Domain.Entities;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using OnlineBookStore.Messages.Models.Messages;

namespace BookStore.Application.Services.BackgroundServices
{
    public class BackgroudService : IBackgroudService
    {
        private readonly ILogger<BackgroudService> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IPublishEndpoint _publishEndpoint;

        public BackgroudService(
            ILogger<BackgroudService> logger,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            IPublishEndpoint publishEndpoint)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _publishEndpoint = publishEndpoint;
        }

        public async Task SendDailyEmail()
        {
            _logger.LogInformation("Sending daily email");

            var usersToSendEmailTo = await _unitOfWork.UsersRepository
                .FindAllAsync(u => u.RoleId == UserRoleConstants.NormalUserRoleId);

            var books = await _unitOfWork.BooksRepository.FindAllAsync();
            var randomBookNumber = GenerateRandomNumber(books.Count());
            var recommendedBook = books.ElementAt(randomBookNumber);

            var link = $"https://localhost:7025/api/books/ids/{recommendedBook.BookGuid}";

            var recommendedBookEmail = new BookRecommendationMessage
            {
                SendTo = usersToSendEmailTo.Select(u => u.Email).ToArray(),
                Content = "Here is your daily book recomendation.",
                EmailTitle = "Daily book recommendation.",
                BookLink = link,
            };

            await _publishEndpoint.Publish(recommendedBookEmail);

            _logger.LogInformation("Sending daily email from host " + link);
        }

        private int GenerateRandomNumber(int length)
        {
            var randomNumberGenerator = new Random();
            
            return randomNumberGenerator.Next(0, length);
        }
    }
}
