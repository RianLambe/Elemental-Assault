using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace Dishooks
{
    /// <summary>
    /// Contains simpler but less flexible wrapper methods for sending messages to Discord.
    /// Consider using the <see cref="Webhook"/> class instead, which this class uses internally.
    /// </summary>
    public class Dishook : MonoBehaviour
    {
        public const string DefaultName = "Dishooks";
        public const string DefaultAvatar = "https://i.imgur.com/rbSzmFv.png";

        /// <summary>
        /// The default channel to send messages to if none is specified.
        /// !!! Remember to change this to a channel you have created !!!
        /// </summary>
        public const string DefaultUrl = "https://discord.com/api/webhooks/721675223841374228/fzRfJkuLvyrmN0caW3y5vU0_lVI-yeXWZ7Td8eBL2Yjm4n9s5l04mp0mbZ6CDWbxMpAI";

        /// <summary>
        /// Sends a message to your channel.
        /// </summary>
        /// <param name="msg">The message you want to send. Default discord emotes and formatting supported.</param>
        /// <param name="name">Username that sends the webhook. Using default if null</param>
        /// <param name="avatar">Link to the senders profile picture. Using default if null.</param>
        /// <param name="url">Link to the webhook. Using default if null.</param>
        public static void Send(string msg, string name = null, string avatar = null, string url = null)
        {
            Webhook webhook = new Webhook
            {
                URL = url ?? DefaultUrl,
                Username = name ?? DefaultName,
                AvatarUrl = avatar ?? DefaultAvatar,
                Content = msg
            };

            webhook.Send();
        }

        /// <summary>
        /// Sends a message to your channel.
        /// </summary>
        /// <param name="msg">The message you want to send. Default discord emotes and formatting supported.</param>
        /// <param name="hook">Webhook item created earlier, see the readme file.</param>
        public static void Send(string msg, DishookItem hook)
        {
            Send(msg, hook.Username, hook.AvatarUrl, hook.URL);
        }

        public static IEnumerator Post(string url, string jsonString)
        {
            // Double check that the URL is set.
            if (url == "https://discord.com/api/webhooks/721675223841374228/fzRfJkuLvyrmN0caW3y5vU0_lVI-yeXWZ7Td8eBL2Yjm4n9s5l04mp0mbZ6CDWbxMpAI")
                Debug.LogError("Dishooks: You need to change the URL to your own webhook! See the readme file for more info.");
                
            UnityWebRequest request = new UnityWebRequest(url, "POST");
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonString);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            yield return request.SendWebRequest();
            
            string response = request.downloadHandler.text;
            int statusCode = (int)request.responseCode;
            switch (request.responseCode)
            {
                case 204:
                    Debug.Log($"Dishooks: Message sent successfully ({statusCode}).");
                    break;
                case 400:
                    if (response.Contains("avatar_url") && response.Contains("Scheme must be one of"))
                    {
                        Debug.LogError("Dishooks: Your avatar URL is invalid! Scheme must be one of http or https. Message not posted.");
                    }
                    else if(response.Contains("username") && response.Contains("Must be between 1 and 80 in length"))
                    {
                        Debug.LogError("Dishooks: Your username is invalid! Username must be between 1 and 80 characters. Message not posted.");
                    }
                    else if(response.Contains("content") && response.Contains("Must be 2000 or fewer in length"))
                    {
                        Debug.LogError("Dishooks: Your message is invalid! Content must be between 0 and 2000 characters. Message not posted.");
                    }
                    else
                    {
                        Debug.LogError($"Dishooks: Bad request ({statusCode}), webhook not sent.\nAdditional info: {response}");
                    }
                    break;
                case 401:
                    Debug.LogError($"Dishooks: Unauthorized ({statusCode}), your webhook URL is likely incorrect.\nAdditional info: {response}");
                    break;
                default:
                    Debug.LogWarning($"Dishooks: Unknown response code ({statusCode}), please report this to the developer.\nAdditional info: {response}");
                    break;
            }
        }
    }
}