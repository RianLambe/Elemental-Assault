using UnityEngine;
using UnityEngine.Serialization;

namespace Dishooks
{
    [CreateAssetMenu(fileName = "Webhook", menuName = "Dishooks/Webhook", order = 1)]
    [System.Serializable]
    public class DishookItem : ScriptableObject
    {
        [Header("Webhook configuration")]  
        
        [FormerlySerializedAs("username")] 
        public string Username;
        [FormerlySerializedAs("avatarLink")] 
        public string AvatarUrl;
        [FormerlySerializedAs("channelLink")] 
        public string URL;

        //Not used in any code - just as a personal note in the editor.
        [Header("Information")] [SerializeField] private new string name;
        [TextArea] [SerializeField] private string description;
    }
}