using System;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using Moq;
using OnlineBookStore.Exceptions.Exceptions;
using Profiles.Application.DTOs.Response;
using Profiles.Application.Features.Users.Queries.GetUserById;
using Profiles.Application.Interfaces.Repositories;
using Profiles.Domain.Entities;
using Xunit;

namespace Profiles.Tests.Members.Queries
{
    public class GetUserByIdQueryHandlerTests
    {
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly GetUserByIdQueryHandler _handler;

        public GetUserByIdQueryHandlerTests()
        {
            _mockUserRepository = new ();
            _mockMapper = new ();

            _handler = new GetUserByIdQueryHandler(
                _mockUserRepository.Object,
                _mockMapper.Object
            );
        }

        [Theory]
        [InlineData("8071b091-dd68-4fe1-a619-3dff7329a2e7")]
        [InlineData("21e99d1a-c3e3-47e2-bf4d-5494b9481cac")]
        [InlineData("05c0dd1f-779f-4c99-99a8-c7913e987824")]
        [InlineData("d06ee7db-85e5-4b29-b8d3-040c2c45fbee")]
        public async Task GetUserById_UserExist_ShouldReturnListOfUsers(string id)
        {
            // Arrange
            var guid = Guid.Parse(id);
            var repositoryResponse = new User
                { Id = guid, FirstName = "user", LastName = "user", Email = "user@gmail.com", RoleId = Guid.Parse("edeba8cc-f2a9-46da-a641-0e3d8aabdaf8") };
            
            var handlerResponse = new GetUsersResponse
                { Id = id, FirstName = "user", LastName = "user", Email = "user@gmail.com" };

            var query = new GetUserByIdQuery(id);

            _mockUserRepository.Setup(r => r.GetUserByIdAsync(guid))
                .ReturnsAsync(repositoryResponse);
            
            _mockMapper.Setup(m => m.Map<GetUsersResponse>(repositoryResponse))
                .Returns(handlerResponse);

            // Act
            var act = await _handler.Handle(query, default);
            
            // Assert
            act.Should().NotBeNull();
            act.Should().BeEquivalentTo(handlerResponse);
        }

        [Theory]
        [InlineData("617c44e7-387a-4ca8-ba51-fe53a5e1d1f7")]
        [InlineData("4bc4e51d-4033-4e96-822e-bde819107d72")]
        [InlineData("ee385edf-da32-47e6-9bb5-abf8f1de0f23")]
        [InlineData("57b5eec5-2060-4049-8586-299c6d6947e8")]
        public async Task GetUserById_UserDoesNotExist_ShouldThrowNotFoundException(string id)
        {
            // Arrange
            var query = new GetUserByIdQuery(id);

            _mockUserRepository.Setup(r => r.GetUserByIdAsync(Guid.Parse(id)))
                .ThrowsAsync(new NotFoundException(ExceptionMessages.UserNotFoundByIdMessage));
            
            // Act
            var act = async () => await _handler.Handle(query, default);
            
            // Assert
            await act.Should().ThrowAsync<NotFoundException>();
        }
    }
}
