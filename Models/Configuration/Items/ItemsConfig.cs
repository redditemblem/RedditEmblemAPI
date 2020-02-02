using RedditEmblemAPI.Models.Configuration.Common;
using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Configuration.Items
{
    public class ItemsConfig
    {
        public ItemsConfig()
        {
            this.Stats = new List<NamedStatConfig>();
            this.EquippedStatModifiers = new List<NamedStatConfig>();
            this.InventoryStatModifiers = new List<NamedStatConfig>();
            this.TextFields = new List<int>();
        }

        //Required fields
        public WorksheetQuery WorksheetQuery { get; set; }
        public int ItemName { get; set; }
        public int Category { get; set; }
        public int WeaponRank { get; set; }
        public int UtilizedStat { get; set; }
        public int Uses { get; set; }
        public IList<NamedStatConfig> Stats { get; set; }
        public RangeConfig Range { get; set; }


        //Optional fields
        public int SpriteURL { get; set; } = -1;
        public IList<NamedStatConfig> EquippedStatModifiers { get; set; }
        public IList<NamedStatConfig> InventoryStatModifiers { get; set; }
        public IList<int> TextFields { get; set; }
    }
}