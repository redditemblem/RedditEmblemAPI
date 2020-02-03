using RedditEmblemAPI.Models.Common;
using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Output
{
    public class Unit
    {
        public Unit()
        {
            this.TextFields = new List<string>();
            this.Tags = new List<string>();
            this.Stats = new Dictionary<string, ModifiedStatValue>();
            this.Inventory = new List<Item>();
            this.Skills = new List<Skill>();
        }

        public string Name { get; set; }
        public string SpriteURL { get; set; }
        public IList<string> TextFields { get; set; }
        public Coordinate Coordinates { get; set; }
        public int Level { get; set; }
        public string Class { get; set; }
        public string Affiliation { get; set; }
        public int Experience { get; set; }
        public HP HP { get; set; }
        public IList<string> Tags { get; set; }
        public Dictionary<string, ModifiedStatValue> Stats { get; set; }
        public IList<Item> Inventory { get; set; }
        public IList<Skill> Skills { get; set; }
    }
}
