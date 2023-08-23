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
            var repositoryResponse = new List<User>
            {
                new User
                { 
                    Id = Guid.Parse("fecb4433-6eb2-4b74-8741-7c65320d43d5"),
                    Email = "user1@gmail.com",
                    FirstName = "user1",
                    LastName = "user1",
                },
                new User
                { 
                    Id = Guid.Parse("48d855bc-a2e0-49e9-a1e3-39c510fff686"),
                    Email = "user2@gmail.com",
                    FirstName = "user2",
                    LastName = "user2",
                },
                new User
                { 
                    Id = Guid.Parse("ebde04f3-e9df-4289-bb36-2f3d1d06425e"),
                    Email = "user3@gmail.com",
                    FirstName = "user3",
                    LastName = "user3"
                },
            };
            
            var handlerResponse = new List<GetUsersResponse>
            {
                new GetUsersResponse
                { 
                    Id = "fecb4433-6eb2-4b74-8741-7c65320d43d5",
                    Email = "user1@gmail.com",
                    FirstName = "user1",
                    LastName = "user1",
                },
                new GetUsersResponse
                { 
                    Id = "48d855bc-a2e0-49e9-a1e3-39c510fff686",
                    Email = "user2@gmail.com",
                    FirstName = "user2",
                    LastName = "user2",
                },
                new GetUsersResponse
                { 
                    Id = "ebde04f3-e9df-4289-bb36-2f3d1d06425e",
                    Email = "user3@gmail.com",
                    FirstName = "user3",
                    LastName = "user3"
                },
            };

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
            var repositoryResponse = default(List<User>);
            var handlerResponse = default(List<GetUsersResponse>);

            var query = new GetUsersQuery();

            _mockUserRepository.Setup(r => r.GetAllUsersAsync())
                .ReturnsAsync(repositoryResponse);
            
            _mockMapper.Setup(m => m.Map<IEnumerable<GetUsersResponse>?>(repositoryResponse))
                .Returns(handlerResponse);

            // Act
            var act = async () => await _handler.Handle(query, default);
            
            // Assert
            await act.Should().ThrowAsync<NotFoundException>();
        }
    }
}
