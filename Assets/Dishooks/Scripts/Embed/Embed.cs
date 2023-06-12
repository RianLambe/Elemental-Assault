using System;
using Newtonsoft.Json;
using UnityEngine;

namespace Dishooks.Embeds
{
#nullable enable
    [JsonObject(MemberSerialization.OptIn)]
    public class Embed
    {
        #region  Embed Fields

        /// <summary>
        /// Title of the embed
        /// </summary>
        [JsonProperty("title")]
        public string? Title { get; set; }

        /// <summary>
        /// Type of embed (always "rich" for webhook embeds)
        /// </summary>
        [JsonProperty("type")]
        public string? Type { get; set; } = "rich";

        /// <summary>
        /// Description of the embed
        /// </summary>
        [JsonProperty("description")]
        public string? Description { get; set; }

        /// <summary>
        /// Url of the embed
        /// </summary>
        [JsonProperty("url")]
        public string? Url { get; set; }

        /// <summary>
        /// Timestamp of the embed content.
        /// </summary>
        [JsonProperty("timestamp")]
        public DateTime? Timestamp { get; set; }
        
        /// <summary>
        /// Color code of the embed. Default is #202225 (dark grey) if not set.
        /// </summary>
        public Color? Color { get; set; }
        
        [JsonProperty("color")]
        private int? _colorInt { get
        {
            if(Color != null)
#pragma warning disable CS8629
                return ((int) (Color?.r * 255) << 16) + ((int) (Color?.g * 255) << 8) + (int) (Color?.r * 255);
#pragma warning restore CS8629
            return null;
        } }

        [JsonProperty("footer")]
        public Footer? Footer { get; set; }
        
        [JsonProperty("image")]
        public Image? Image { get; set; }
        
        [JsonProperty("thumbnail")]
        public Thumbnail? Thumbnail { get; set; }
        
        [JsonProperty("video")]
        public Video? Video { get; set; }
        
        [JsonProperty("provider")]
        public Provider? Provider { get; set; }
        
        [JsonProperty("author")]
        public Author? Author { get; set; }
        
        [JsonProperty("fields")]
        public Field[]? Fields { get; set; }
        
        #endregion
        
        #region Constructors
        public Embed()
        {
        }
        
        public Embed(string title)
        {
            Title = title;
        }
        
        public Embed(string title, string description)
        {
            Title = title;
            Description = description;
        }
        
        public Embed(string title, string description, string url)
        {
            Title = title;
            Description = description;
            Url = url;
        }
        
        public Embed(string title, string description, string url, Color color)
        {
            Title = title;
            Description = description;
            Url = url;
            Color = color;
        }
        
        public Embed(string title, string description, string url, DateTime timestamp, Color color)
        {
            Title = title;
            Description = description;
            Url = url;
            Timestamp = timestamp;
            Color = color;
        }
        
        public Embed(string title, string description, string url, DateTime timestamp, Color color, Footer footer)
        {
            Title = title;
            Description = description;
            Url = url;
            Timestamp = timestamp;
            Color = color;
            Footer = footer;
        }

        public Embed(string title, string description, string url, DateTime timestamp, Color color, Image image)
        {
            Title = title;
            Description = description;
            Url = url;
            Timestamp = timestamp;
            Color = color;
            Image = image;
        }
        
        public Embed(string title, string description, string url, DateTime timestamp, Color color, Thumbnail thumbnail)
        {
            Title = title;
            Description = description;
            Url = url;
            Timestamp = timestamp;
            Color = color;
            Thumbnail = thumbnail;
        }
        
        public Embed(string title, string description, string url, DateTime timestamp, Color color, Video video)
        {
            Title = title;
            Description = description;
            Url = url;
            Timestamp = timestamp;
            Color = color;
            Video = video;
        }
        
        public Embed(string title, string description, string url, DateTime timestamp, Color color, Provider provider)
        {
            Title = title;
            Description = description;
            Url = url;
            Timestamp = timestamp;
            Color = color;
            Provider = provider;
        }
        
        
        public Embed(string title, string description, string url, DateTime timestamp, Color color, Author author)
        {
            Title = title;
            Description = description;
            Url = url;
            Timestamp = timestamp;
            Color = color;
            Author = author;
        }
        
        public Embed(string title, string description, string url, DateTime timestamp, Color color, Footer footer, Image image)
        {
            Title = title;
            Description = description;
            Url = url;
            Timestamp = timestamp;
            Color = color;
            Footer = footer;
            Image = image;
        }
        #endregion

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.None, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                DateFormatHandling = DateFormatHandling.IsoDateFormat
            });
        }

        public static string SerializeArray(Embed[] embed)
        {
            return JsonConvert.SerializeObject(embed, Formatting.None, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                DateFormatHandling = DateFormatHandling.IsoDateFormat
            });
        }
    }
}