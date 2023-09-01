using System;
using System.Collections.Generic;
using Profiles.Application.DTOs.Response;
using Profiles.Domain.Entities;

namespace Profiles.Tests.Members.Data
{
    public class GetPublishersQueryHandlerTestData
    {
        private static readonly List<User> repositoryResponse = new List<User>
        {
            new User
            { 
                Id = Guid.Parse("fecb4433-6eb2-4b74-8741-7c65320d43d5"),
                Email = "publisher1@gmail.com",
                FirstName = "publisher1",
                LastName = "publisher1",
            },
            new User
            { 
                Id = Guid.Parse("48d855bc-a2e0-49e9-a1e3-39c510fff686"),
                Email = "publisher2@gmail.com",
                FirstName = "publisher2",
                LastName = "publisher2",
            },
            new User
            { 
                Id = Guid.Parse("ebde04f3-e9df-4289-bb36-2f3d1d06425e"),
                Email = "publisher3@gmail.com",
                FirstName = "publisher3",
                LastName = "publisher3"
            },
        };

        private static readonly List<GetUsersResponse> handlerResponse = new List<GetUsersResponse>
        {
            new GetUsersResponse
            { 
                Id = "fecb4433-6eb2-4b74-8741-7c65320d43d5",
                Email = "publisher1@gmail.com",
                FirstName = "publisher1",
                LastName = "publisher1",
            },
            new GetUsersResponse
            { 
                Id = "48d855bc-a2e0-49e9-a1e3-39c510fff686",
                Email = "publisher2@gmail.com",
                FirstName = "publisher2",
                LastName = "publisher2",
            },
            new GetUsersResponse
            { 
                Id = "ebde04f3-e9df-4289-bb36-2f3d1d06425e",
                Email = "publisher3@gmail.com",
                FirstName = "publisher3",
                LastName = "publisher3"
            },
        };

        public static List<User> RepositoryResponse { get => repositoryResponse; }
        public static List<GetUsersResponse> HandlerResponse { get => handlerResponse; }
    }
}
