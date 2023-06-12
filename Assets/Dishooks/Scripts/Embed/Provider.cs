using Newtonsoft.Json;

namespace Dishooks.Embeds
{
#nullable enable
    [JsonObject(MemberSerialization.OptIn)]
    public class Provider
    {
        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonProperty("url")]
        public string? Url { get; set; }
        
        public Provider()
        {
        }
        
        public Provider(string name, string url)
        {
            Name = name;
            Url = url;
        }
    }
}