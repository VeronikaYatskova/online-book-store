using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using Moq;
using OnlineBookStore.Exceptions.Exceptions;
using Profiles.Application.DTOs.Response;
using Profiles.Application.Features.Users.Queries.GetPublishers;
using Profiles.Application.Interfaces.Repositories;
using Profiles.Tests.Members.Data;
using Xunit;

namespace Profiles.Tests.Members.Queries
{
    public class GetPublishersQueryHandlerTests
    {
        private readonly Mock<IPublisherRepository> _mockPublisherRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly GetPublishersQueryHandler _handler;

        public GetPublishersQueryHandlerTests()
        {
            _mockPublisherRepository = new ();
            _mockMapper = new ();

            _handler = new GetPublishersQueryHandler(
                _mockPublisherRepository.Object,
                _mockMapper.Object
            );
        }

        [Fact]
        public async Task GetPublishers_PublishersExist_ShouldReturnListOfUsers()
        {
            // Arrange
            var repositoryResponse = GetPublishersQueryHandlerTestData.RepositoryResponse;
            
            var handlerResponse = GetPublishersQueryHandlerTestData.HandlerResponse;

            var query = new GetPublishersQuery();

            _mockPublisherRepository.Setup(r => r.GetPublishersAsync())
                .ReturnsAsync(repositoryResponse);
            
            _mockMapper.Setup(m => m.Map<IEnumerable<GetUsersResponse>>(repositoryResponse))
                .Returns(handlerResponse);

            // Act
            var act = await _handler.Handle(query, default);
            
            // Assert
            act.Should().NotBeNullOrEmpty();
            act.Should().AllBeAssignableTo<GetUsersResponse>();
            act.Should().BeEquivalentTo(handlerResponse);
        }

        [Fact]
        public async Task GetPublishers_PublishersDoNotExist_ShouldThrowNotFoundException()
        {
            // Arrange
            var query = new GetPublishersQuery();

            _mockPublisherRepository.Setup(r => r.GetPublishersAsync())
                .ThrowsAsync(new NotFoundException(ExceptionMessages.PublishersNotFoundMessage));

            // Act
            var act = async () => await _handler.Handle(query, default);
            
            // Assert
            await act.Should().ThrowAsync<NotFoundException>();
        }        
    }
}
