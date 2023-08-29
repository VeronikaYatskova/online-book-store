
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Requests.DAL.Models
{
    public class Request
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = default!;
        public string UserId { get; set; } = default!;
        public string PublisherId { get; set; } = default!;
        public string BookFakeName { get; set; } = default!;
        public string? BookCoverFakeName { get; set; }
        public bool IsApproved { get; set; } = false;
    }
}
