using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Requests.DAL.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = default!;
        
        [BsonElement("email")]
        public string Email { get; set; } = default!;
        
        [BsonElement("roleId")]
        public string RoleId { get; set; } = default!;
    }
}
