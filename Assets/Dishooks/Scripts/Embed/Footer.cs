using Newtonsoft.Json;

namespace Dishooks.Embeds
{
#nullable enable
    [JsonObject(MemberSerialization.OptIn)]
    public class Footer
    {
        /// <summary>
        /// The footer text.
        /// </summary>
        [JsonProperty("text")] 
        public string Text { get; set; }
        
        /// <summary>
        /// The url of footer icon (only supports http(s) and attachments).
        /// </summary>
        [JsonProperty("icon_url")] 
        public string? IconUrl { get; set; }
        
        /// <summary>
        /// A proxied url of the footer icon.
        /// </summary>
        [JsonProperty("proxy_icon_url")] 
        public string? ProxyIconUrl { get; set; }
        
        public Footer(string text)
        {
            Text = text;
        }
        
        public Footer(string text, string iconUrl)
        {
            Text = text;
            IconUrl = iconUrl;
        }
    }
}