using System;
using Dishooks.Embeds;
using UnityEngine;

namespace Dishooks
{
    /// <summary>
    /// Creating Webhook items is the suggested way to use Dishooks. They have full support for embeds, custom usernames and avatar images.
    /// </summary>
    public class DishooksExample_Advanced : MonoBehaviour
    {
        /// <summary>
        /// An example of how to send a complex embed.
        /// This method is called from the "Canvas/Button_Embed" button in the example scene.
        /// </summary>
        public void SendEmbed()
        {
            Embed embed = new Embed
            {
                Color = Color.magenta, // Optional - Default is #202225
                Author = new Author("Fabian", "https://media.discordapp.net/attachments/501452852364050443/924359470316912710/avatar.png", "https://www.google.com/"), // Displayed at the top of the embed
                Title = "Welcome to Dishooks!",
                Description = "This is the description of an example embed. Feel free to use this as a template for your own embeds!",
                Thumbnail = new Thumbnail("https://i.imgur.com/dZw7B9p.png"), // Displayed at the top right corner of the embed
                Fields = new[]
                {
                    new Field("Fields", "Discord embeds supports up to 20 fields, which are displayed in the order they are added."),
                    new Field("Formatting fields", "By default, fields are displayed in a list with a title and a value. You can also use inline fields, which are displayed inline with the title and value."),
                    new Field("Fields with multiple lines", "This field has multiple lines!\nThis is line 2!\nThis is line 3!")
                },
                Image = new Image("https://i.imgur.com/bm9qoxz.png"), // An image of a cat displayed under all fields
                Footer = new Footer("Dishooks", "https://i.imgur.com/YM0HrlO.png"), // Displayed in the bottom left corner of the embed.
                Timestamp = DateTime.Now //Displayed to the right of the footer
            };

            Webhook webhook = new Webhook
            {
                URL = Dishook.DefaultUrl,
                Username = "Webhook!",
                AvatarUrl = "https://media.discordapp.net/attachments/501452852364050443/924359470316912710/avatar.png",
                Content = "Hello World!",
                Embeds = new[]
                {
                    embed
                }
            };

            webhook.Send();
        }
        
        /// <summary>
        /// This is the code used to generate the messages shown in the Unity Asset Store promo art.
        /// </summary>
        public void PromoEmbed()
        {
            // Create the webhook
            Webhook webhook = new Webhook
            {
                URL = Dishook.DefaultUrl,
                Username = "Dishooks!",
                AvatarUrl = "https://i.imgur.com/Ukqoy8s.png"
            };
            
            // Create an embed
            Embed embed = new Embed
            {
                Color = Color.magenta,
                Author = new Author("Fabian", "https://i.imgur.com/k5lNAWS.png"), 
                Title = "Welcome to Dishooks!",
                Description = "Create complex rich embeds with ease.",
                Fields = new []
                {
                    new Field("Everything supported!", "Fields, images, texts, footers, timestamps, etc."),
                }
            };
            
            // Attach the embed to the webhook
            webhook.AddEmbed(embed);
            
            // Send the webhook to Discord
            webhook.Send();
            
            Dishook.Send("Hello World!");
            
            Dishook.Send("Another message, but with unique properties!", 
                "Fabian", "https://i.imgur.com/k5lNAWS.png");
        }
    }
}