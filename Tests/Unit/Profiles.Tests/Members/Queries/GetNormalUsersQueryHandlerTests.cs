using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using Moq;
using OnlineBookStore.Exceptions.Exceptions;
using Profiles.Application.DTOs.Response;
using Profiles.Application.Features.Users.Queries.GetNormalUsers;
using Profiles.Application.Interfaces.Repositories;
using Profiles.Tests.Members.Data;
using Xunit;

namespace Profiles.Tests.Members.Queries
{
    public class GetNormalUsersQueryHandlerTests
    {
        private readonly Mock<INormalUserRepository> _mockNormalUserRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly GetNormalUsersQueryHandler _handler;

        public GetNormalUsersQueryHandlerTests()
        {
            _mockNormalUserRepository = new ();
            _mockMapper = new ();

            _handler = new GetNormalUsersQueryHandler(
                _mockNormalUserRepository.Object,
                _mockMapper.Object
            );
        }

        [Fact]
        public async Task GetNormalUsers_UsersExist_ShouldReturnListOfUsers()
        {
            // Arrange
            var repositoryResponse = GetNormalUsersQueryHandlerTestData.RepositoryResponse;
            
            var handlerResponse = GetNormalUsersQueryHandlerTestData.HandlerResponse;

            var query = new GetNormalUsersQuery();

            _mockNormalUserRepository.Setup(r => r.GetNormalUsersAsync())
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
        public async Task GetNormalUsers_UsersDoNotExist_ShouldThrowNotFoundException()
        {
            // Arrange
            var query = new GetNormalUsersQuery();

            _mockNormalUserRepository.Setup(r => r.GetNormalUsersAsync())
                .ThrowsAsync(new NotFoundException(ExceptionMessages.UsersNotFoundMessage));
            
            // Act
            var act = async () => await _handler.Handle(query, default);
            
            // Assert
            await act.Should().ThrowAsync<NotFoundException>();
        }
    }
}
