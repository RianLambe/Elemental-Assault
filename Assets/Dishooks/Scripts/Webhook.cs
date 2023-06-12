using System;
using System.Linq;
using Dishooks.Embeds;
using Newtonsoft.Json;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Dishooks
{
#nullable enable
    [JsonObject(MemberSerialization.OptIn)]
    public class Webhook
    {
        /// <summary>
        /// The message contents (up to 2000 characters)
        /// </summary>
        [JsonProperty("content")]
        public string? Content { get; set; }
        
        /// <summary>
        /// Override the default username of the webhook
        /// </summary>
        [JsonProperty("username")]
        public string? Username { get; set; }
        
        /// <summary>
        /// Override the default avatar of the webhook
        /// </summary>
        [JsonProperty("avatar_url")]
        public string? AvatarUrl { get; set; }

        /// <summary>
        /// True if this is a TTS message
        /// </summary>
        [JsonProperty("tts")]
        public bool? Tts { get; set; } = true;
        
        /// <summary>
        /// Embedded rich content, max 10.
        /// </summary>
        [JsonProperty("embeds")]
        public Embed[]? Embeds { get; set; }
        
        /// <summary>
        /// The URL to the webhook
        /// </summary>
        public string URL { get; set; }
        
        public Webhook()
        {
            URL = "";
        }
        
        public Webhook(string url)
        {
            URL = url;
        }

        public Webhook(DishookItem item) : this()
        {
            SetConfiguration(item);
        }

        /// <summary>
        /// Serialize the webhook to a JSON string
        /// </summary>
        public override string ToString()
        {
            if(Content == null && Embeds == null)
            {
                throw new ArgumentException("Webhook must have either content or embeds");
            }
            
            return JsonConvert.SerializeObject(this, Formatting.None, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                DateFormatHandling = DateFormatHandling.IsoDateFormat
            });
        }

        /// <summary>
        /// Send this webhook to Discord
        /// </summary>
        public void Send()
        {
            // Creating an instance of MonoBehaviour is required to use Unity's Coroutines, which are used to send the webhook. 
            // It is however not the most elegant solution. 
            Dishook dishook = new GameObject("Webhook").AddComponent<Dishook>();
            dishook.StartCoroutine(Dishook.Post(URL, ToString()));
            Object.Destroy(dishook, 30);
        }

        /// <summary>
        /// Adds an embed to the webhook. Max 10.
        /// </summary>
        public void AddEmbed(Embed embed)
        {
            Embeds = Embeds == null ? new[] {embed} : Embeds.Append(embed).ToArray();
            
            if(Embeds.Length > 10)
            {
                Debug.LogWarning("Webhook can only have 10 embeds. The last one(s) will be ignored by Discord.");
            }
        }
        
        public void SetConfiguration(DishookItem item)
        {
            Username = item.Username;
            AvatarUrl = item.AvatarUrl;
            URL = item.URL;
        }
    }
}