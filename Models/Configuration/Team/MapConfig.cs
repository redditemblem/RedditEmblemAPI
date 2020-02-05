using RedditEmblemAPI.Models.Configuration.Common;

namespace RedditEmblemAPI.Models.Configuration.Team
{
    public class MapConfig
    {
        //Required
        public WorksheetQuery WorksheetQuery { get; set; }
        public int MapSwitch { get; set; }
        public int MapURL { get; set; }
        public MapConstantsConfig Constants { get; set; }

        //Optional
        public int ChapterPostLink { get; set; } = -1;
    }
}