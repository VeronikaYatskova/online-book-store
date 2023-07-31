using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Comments.DAL.Entities
{
    public class Comment
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = default!;
        public string BookId { get; set; } = default!;
        public string UserId { get; set; } = default!;
        public string Text { get; set; } = default!;
    }
}
