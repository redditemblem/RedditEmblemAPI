using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Output
{
    public class Unit
    {
        public Unit()
        {
            this.Stats = new Dictionary<string, ModifiedStatValue>();
            this.Inventory = new List<Item>();
        }

        public string Name { get; set; }
        public string SpriteURL { get; set; }
        public Coordinate Coordinates { get; set; }
        public Dictionary<string, ModifiedStatValue> Stats { get; set; }
        public IList<Item> Inventory { get; set; }
    }
}
