using Newtonsoft.Json;

namespace Dishooks.Embeds
{
#nullable enable
    [JsonObject(MemberSerialization.OptIn)]
    public class Author
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("url")]
        public string? Url { get; set; }

        [JsonProperty("icon_url")]
        public string? IconUrl { get; set; }
        
        [JsonProperty("proxy_icon_url")]
        public string? ProxyIconUrl { get; set; }
        
        public Author(string name)
        {
            Name = name;
        }
        
        public Author(string name, string iconUrl)
        {
            Name = name;
            IconUrl = iconUrl;
        }
        
        public Author(string name, string iconUrl, string url)
        {
            Name = name;
            IconUrl = iconUrl;
            Url = url;
        }
    }
}