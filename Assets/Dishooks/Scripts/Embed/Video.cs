using Newtonsoft.Json;

namespace Dishooks.Embeds
{
#nullable enable
    [JsonObject(MemberSerialization.OptIn)]
    public class Video
    {
        /// <summary>
        /// The source url of video.
        /// </summary>
        [JsonProperty("url")] 
        public string Url { get; set; }
        
        /// <summary>
        /// A proxied url of the video.
        /// </summary>
        [JsonProperty("proxy_url")]
        public string? ProxyUrl { get; set; }
        
        /// <summary>
        /// The height of the video.
        /// </summary>
        [JsonProperty("height")]
        public int? Height { get; set; }
        
        /// <summary>
        /// The width of the video.
        /// </summary>
        [JsonProperty("width")]
        public int? Width { get; set; }
        
        public Video(string url)
        {
            Url = url;
        }
        
        public Video(string url, int height, int width)
        {
            Url = url;
            Height = height;
            Width = width;
        }
    }
}