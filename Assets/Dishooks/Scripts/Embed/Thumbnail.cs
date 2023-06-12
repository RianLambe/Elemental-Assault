using Newtonsoft.Json;

namespace Dishooks.Embeds
{
#nullable enable
    [JsonObject(MemberSerialization.OptIn)]
    public class Thumbnail
    {
        /// <summary>
        /// The source url of image (only supports http(s) and attachments).
        /// </summary>
        [JsonProperty("url")] 
        public string Url { get; set; }
        
        /// <summary>
        /// A proxied url of the image.
        /// </summary>
        [JsonProperty("proxy_url")]
        public string? ProxyUrl { get; set; }
        
        /// <summary>
        /// The height of the image.
        /// </summary>
        [JsonProperty("height")]
        public int? Height { get; set; }
        
        /// <summary>
        /// The width of the image.
        /// </summary>
        [JsonProperty("width")]
        public int? Width { get; set; }

        public Thumbnail(string url)
        {
            Url = url;
        }
        
        public Thumbnail(string url, int height, int width)
        {
            Url = url;
            Height = height;
            Width = width;
        }
    }
}