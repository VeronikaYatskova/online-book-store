using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using Moq;
using OnlineBookStore.Exceptions.Exceptions;
using Profiles.Application.DTOs.Response;
using Profiles.Application.Features.Users.Queries.GetAuthors;
using Profiles.Application.Interfaces.Repositories;
using Profiles.Tests.Members.Data;
using Xunit;

namespace Profiles.Tests.Members.Queries
{
    public class GetAuthorsQueryHandlerTests
    {
        private readonly Mock<IAuthorRepository> _mockAuthorRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly GetAuthorsQueryHandler _handler;

        public GetAuthorsQueryHandlerTests()
        {
            _mockAuthorRepository = new ();
            _mockMapper = new ();

            _handler = new GetAuthorsQueryHandler(
                _mockAuthorRepository.Object,
                _mockMapper.Object
            );
        }

        [Fact]
        public async Task GetAuthors_AuthorsExist_ShouldReturnListOfUsers()
        {
            // Arrange
            var repositoryResponse = GetAuthorsQueryHandlerTestData.RepositoryResponse;
            
            var handlerResponse = GetAuthorsQueryHandlerTestData.HandlerResponse;

            var query = new GetAuthorsQuery();

            _mockAuthorRepository.Setup(r => r.GetAuthorsAsync())
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
        public async Task GetAuthors_AuthorsDoNotExist_ShouldThrowNotFoundException()
        {
            // Arrange
            var query = new GetAuthorsQuery();

            _mockAuthorRepository.Setup(r => r.GetAuthorsAsync())
                .ThrowsAsync(new NotFoundException(ExceptionMessages.AuthorsNotFoundMessage));
            
            // Act
            var act = async () => await _handler.Handle(query, default);
            
            // Assert
            await act.Should().ThrowAsync<NotFoundException>();
        }
    }
}
