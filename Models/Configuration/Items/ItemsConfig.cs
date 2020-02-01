using RedditEmblemAPI.Models.Configuration.Common;
using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Configuration.Items
{
    public class ItemsConfig
    {
        public WorksheetQuery WorksheetQuery { get; set; }
        public int ItemName { get; set; }
        public int SpriteURL { get; set; }
        public int Category { get; set; }
        public int WeaponRank { get; set; }
        public int UtilizedStat { get; set; }
        public int Uses { get; set; }
        public IList<NamedStatConfig> Stats { get; set; }
        public RangeConfig Range { get; set; }
        public int[] TextFields { get; set; }
    }
}