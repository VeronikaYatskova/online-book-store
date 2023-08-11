
namespace Comments.DAL.Entities
{
    public class MongoDbSettings
    {
        public string CommentsCollectionName { get; set; } = default!;
        public string ConnectionString { get; set; } = default!;
        public string DatabaseName { get; set; } = default!;
    }
}
