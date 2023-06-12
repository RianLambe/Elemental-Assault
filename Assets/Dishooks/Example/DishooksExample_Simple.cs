using UnityEngine;
using UnityEngine.UI;

namespace Dishooks
{
    /// <summary>
    /// This script contains examples of how to send simple messages to Discord.
    /// This is enough for most people, but if you want to send more complex messages it is recommended to use the <see cref="Webhook"/> script instead.
    /// </summary>
    public class DishooksExample_Simple : MonoBehaviour
    {
        [SerializeField] private InputField _username;
        [SerializeField] private InputField _profilePicture;

        /// <summary>
        /// Sends a good morning message with the default configuration, and logs the response.
        /// This is called from the "Canvas/Button_Good morning" button in the example scene, and can be set/changed in <see cref="Dishook"/>.
        /// </summary>
        public void Morning()
        {
            Dishook.Send("Good morning everyone!");
        }

        /// <summary>
        /// This method is called from the "Canvas/Form/Custom Message" input field in the example scene.
        /// It sends a message input by the user to the default channel.
        /// </summary>
        public void SendCustom(string message)
        {
            string username = _username.text;
            string profilePicture = _profilePicture.text;

            // Dishooks will replace null values with the default values.
            if (profilePicture == "") profilePicture = null;
            if (username == "") username = null;

            Dishook.Send(message, username, profilePicture);
        }
    }
}