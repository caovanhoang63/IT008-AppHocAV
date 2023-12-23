using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace IT008_AppHocAV.Models.GPT
{
    public class GptRequest
    {

            [JsonPropertyName("model")]
            public string Model { get; set; } = "gpt-3.5-turbo";
            [JsonPropertyName("max_tokens")]
            public int MaxTokens { get; set; } = 4000;
            [JsonPropertyName("messages")]
            public List<RequestMessage> Messages { get; set; }
    }
    
    public class RequestMessage
    {
        [JsonPropertyName("role")]
        public string Role { get; set; }
        [JsonPropertyName("content")]
        public string Content { get; set; }
    }
}