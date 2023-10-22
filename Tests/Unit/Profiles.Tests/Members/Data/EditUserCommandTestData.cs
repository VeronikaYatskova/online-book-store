using Profiles.Application.DTOs.Request;
using Xunit;

namespace Profiles.Tests.Members.Data
{
    public class EditUserCommandValidTestData : TheoryData<EditUserRequest>
    {
        public EditUserCommandValidTestData()
        {
            Add(new EditUserRequest { Id = "6fbbb951-9af0-4aa1-be3b-4a77797614fc", Email = "user1", FirstName = "user1", LastName = "user1" });
            Add(new EditUserRequest { Id = "152d652b-50ae-4db2-82cc-d733bf1ee2bd", Email = "user2", FirstName = "user2", LastName = "user2" });
            Add(new EditUserRequest { Id = "696e13cb-59de-4611-873a-5f11d61fc5f0", Email = "user3", FirstName = "user3", LastName = "user3" });
            Add(new EditUserRequest { Id = "de8e8ffd-a6c1-45d9-b1b9-437a85fd141e", Email = "user4", FirstName = "user4", LastName = "user4" });            
        }
    }

    public class EditUserCommandUserDoesNotExistTestData : TheoryData<EditUserRequest>
    {
        public EditUserCommandUserDoesNotExistTestData()
        {
            Add(new EditUserRequest { Id = "cf811fbc-82f5-4042-8423-8a5a1f1349a0", Email = "user1", FirstName = "user1", LastName = "user1" });
            Add(new EditUserRequest { Id = "2e2644de-5029-4ec4-ac69-8e62a3941a3b", Email = "user2", FirstName = "user2", LastName = "user2" });
            Add(new EditUserRequest { Id = "ab79b474-2d73-4a9a-8551-ae282a0f9459", Email = "user3", FirstName = "user3", LastName = "user3" });
            Add(new EditUserRequest { Id = "5f224737-74b4-4694-b4b4-17f37f25026b", Email = "user4", FirstName = "user4", LastName = "user4" });
        }
    }

    public class EditUserCommandInvalidTestData : TheoryData<EditUserRequest>
    {
        public EditUserCommandInvalidTestData()
        {
            Add(new EditUserRequest { Id = "", Email = "", FirstName = "", LastName = "" });
            Add(new EditUserRequest { Id = "2e2644de-5029-4ec4-ac69-8e62a3941a3b", Email = "user2", FirstName = "", LastName = "user2" });
            Add(new EditUserRequest { Id = "", Email = "user3", FirstName = "", LastName = "user3" });
            Add(new EditUserRequest { Id = "5f224737-74b4-4694-b4b4-17f37f25026b", Email = "", FirstName = "", LastName = "" });
        }
    }
}
