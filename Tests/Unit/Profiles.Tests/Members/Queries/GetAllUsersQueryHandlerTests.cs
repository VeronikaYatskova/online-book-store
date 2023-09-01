using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using Moq;
using OnlineBookStore.Exceptions.Exceptions;
using Profiles.Application.DTOs.Response;
using Profiles.Application.Features.Users.Queries.GetAllUsers;
using Profiles.Application.Interfaces.Repositories;
using Profiles.Domain.Entities;
using Profiles.Tests.Members.Data;
using Xunit;

namespace Profiles.Tests.Members.Queries
{
    public class GetAllUsersQueryHandlerTests
    {
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly GetUsersQueryHandler _handler;

        public GetAllUsersQueryHandlerTests()
        {
            _mockUserRepository = new ();
            _mockMapper = new ();

            _handler = new GetUsersQueryHandler(
                _mockUserRepository.Object,
                _mockMapper.Object
            );
        }

        [Fact]
        public async Task GetUsers_UsersExist_ShouldReturnListOfUsers()
        {
            // Arrange
            var repositoryResponse = GetAllUsersQueryHandlerTestData.RepositoryResponse;
            
            var handlerResponse = GetAllUsersQueryHandlerTestData.HandlerResponse;

            var query = new GetUsersQuery();

            _mockUserRepository.Setup(r => r.GetAllUsersAsync())
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
        public async Task GetUsers_UsersDoesNotExist_ShouldThrowNotFoundException()
        {
            // Arrange
            var query = new GetUsersQuery();

            _mockUserRepository.Setup(r => r.GetAllUsersAsync())
                .ThrowsAsync(new NotFoundException(ExceptionMessages.UsersNotFoundMessage));

            // Act
            var act = async () => await _handler.Handle(query, default);
            
            // Assert
            await act.Should().ThrowAsync<NotFoundException>();
        }
    }
}
