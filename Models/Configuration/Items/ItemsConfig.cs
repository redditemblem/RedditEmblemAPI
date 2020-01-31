using RedditEmblemAPI.Models.Configuration.Common;
using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Configuration.Items
{
    public class ItemsConfig
    {
        public WorksheetQuery WorksheetQuery;
        public int ItemName;
        public int SpriteURL;
        public int Category;
        public int WeaponRank;
        public int UtilizedStat;
        public int Uses;
        public IList<NamedStatConfig> Stats;
        public RangeConfig Range;
        public int[] TextFields;
    }
}