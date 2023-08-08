namespace Requests.DAL.Models
{
    public class MongoDbSettings
    {
        public string RequestsCollectionName { get; set; } = default!;
        public string ConnectionString { get; set; } = default!;
        public string DatabaseName { get; set; } = default!;
    }
}
