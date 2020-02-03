using RedditEmblemAPI.Models.Configuration.Common;
using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Configuration.Units
{
    public class UnitsConfig
    {
        public UnitsConfig()
        {
            this.Stats = new List<ModifiedNamedStatConfig>();
            this.Inventory = new InventoryConfig();
            this.Skills = new SkillListConfig();

            this.TextFields = new List<int>();
        }

        //Required Fields
        public WorksheetQuery WorksheetQuery { get; set; }
        public int UnitName { get; set; }
        public int SpriteURL { get; set; }
        public int Coordinates { get; set; }
        public int Level { get; set; }
        public int Class { get; set; }
        public int Affiliation { get; set; }
        public int Experience { get; set; }
        public int CurrentHP { get; set; }
        public int MaxHP { get; set; }
        public IList<ModifiedNamedStatConfig> Stats { get; set; }
        public InventoryConfig Inventory { get; set; }
        public SkillListConfig Skills { get; set; }

        //Optional fields
        public IList<int> TextFields { get; set; }
        public bool HasMoved { get; set; } = false;
        public int Tags { get; set; } = -1;
    }
}
