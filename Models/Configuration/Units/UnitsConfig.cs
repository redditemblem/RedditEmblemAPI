using RedditEmblemAPI.Models.Configuration.Common;
using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Configuration.Units
{
    public class UnitsConfig
    {
        public UnitsConfig()
        {
            this.TextFields = new List<int>();
        }

        //Required Fields
        public WorksheetQuery WorksheetQuery { get; set; }
        public int UnitName { get; set; }
        public int SpriteURL { get; set; }
        public int Class { get; set; }
        public int Coordinates { get; set; }
        public int Experience { get; set; }
        public IList<ModifiedNamedStatConfig> Stats { get; set; }
        public InventoryConfig Inventory { get; set; }
        public SkillListConfig Skills { get; set; }

        //Optional fields
        public IList<int> TextFields { get; set; }
        public bool HasMoved { get; set; } = false;
    }
}
