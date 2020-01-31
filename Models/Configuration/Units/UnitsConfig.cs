using RedditEmblemAPI.Models.Configuration.Common;
using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Configuration.Units
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
        public IList<ModifiedNamedStatConfig> Stats;
        public InventoryConfig Inventory;
    }
}
