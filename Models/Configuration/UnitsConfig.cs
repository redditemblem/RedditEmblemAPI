using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Configuration
{
    public class UnitsConfig
    {
        public WorksheetQuery WorksheetQuery;
        public int UnitName;
        public int SpriteURL;
        public int[] TextFields;
        public int Class;
        public int Coordinates;
        public int Experience;
        public IList<StatConfig> Stats;
        public InventoryConfig Inventory;
    }
}
