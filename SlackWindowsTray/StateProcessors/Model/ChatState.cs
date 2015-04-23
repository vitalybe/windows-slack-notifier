using Newtonsoft.Json;

namespace SlackWindowsTray
{
    public class ChatState
    {
        // Calculated locally
        public string Name { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Id { get; set; }
        
        [JsonProperty(PropertyName = "unread")]
        public bool Unread { get; set; }

        [JsonProperty(PropertyName = "mention")]
        public bool Mention { get; set; }
    }
}