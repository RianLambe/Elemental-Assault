using UnityEngine;

namespace Dishooks
{
    /// <summary>
    /// This script contains examples of how to sort your webhook configurations with Dishook items.
    /// A Dishook item is a container for all data needed to send a webhook - an username, avatar link and channel link.
    /// You can easily set them up in the editor and keep them sorted in folders, but they can also be created with scripts.
    /// This is recommended if you have a lot of webhooks and you want to keep them organized.
    /// </summary>
    public class DishooksExample_Item : MonoBehaviour
    {
        //These two are assigned in the Editor, on the GameObject named "WebhookManager".
        [SerializeField] private DishookItem _achievementLog;
        [SerializeField] private DishookItem _serverStatus;

        //This one is created at runtime.
        private DishookItem _moderationLog;

        private void Start()
        {
            // Create a new DishookItem
            _moderationLog = ScriptableObject.CreateInstance<DishookItem>();
            _moderationLog.name = "Moderation log";
            _moderationLog.AvatarUrl = "https://i.imgur.com/rccp2uP.png";
            _moderationLog.URL = "https://discordapp.com/api/webhooks/721675223841374228/fzRfJkuLvyrmN0caW3y5vU0_lVI-yeXWZ7Td8eBL2Yjm4n9s5l04mp0mbZ6CDWbxMpAI";
        }

        /// <summary>
        /// This is an example of how to send a webhook with a predefined Webhook configuration and message.
        /// This method is called from the "Canvas/Button_Webhook Item" button in the example scene.
        /// </summary>
        public void ServerUp()
        {
            Dishook.Send("The server is online.", _serverStatus);
        }

        /// <summary>
        /// An example of how to send a webhook with a message template, but to a predefined Webhook configuration.
        /// </summary>
        private void UnlockAchievement(string user, string achievement)
        {
            Webhook webhook = new Webhook(_achievementLog)
            {
                Content = $"{user} just unlocked {achievement}, congratulations!"
            };
            
            webhook.Send();
        }
    }
}