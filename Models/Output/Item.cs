using Newtonsoft.Json;

namespace RedditEmblemAPI.Models.Output
{
    public class Item
    {
        public Item()
        {
            this.IsEquipped = false;
            this.IsDroppable = false;
        }

        public string Name;
        [JsonIgnore]
        public string OriginalName;
        public bool IsEquipped;
        public bool IsDroppable;
        public int Uses;
    }
}