using Newtonsoft.Json;
using OnlineBookStore.Messages.Models.Messages;

namespace AzureFunctions.Models
{
    public class QueueMessage
    {
        [JsonProperty("message")]
        public BookRecommendationMessage Message { get; set; } = default!;
    }
}