using Newtonsoft.Json;

namespace Dishooks.Embeds
{
#nullable enable
    [JsonObject(MemberSerialization.OptIn)]
    public class Field
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("value")] 
        public string Value { get; set; }

        [JsonProperty("inline")]
        public bool? Inline { get; set; }  
        
        public Field(string name, string value, bool inline = false)
        {
            Name = name;
            Value = value;
            Inline = inline;
        }
    }
}