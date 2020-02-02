using RedditEmblemAPI.Models.Configuration.Common;
using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Configuration.Skills
{
    public class SkillsConfig
    {
        //Required fields
        public WorksheetQuery WorksheetQuery { get; set; }
        public int SkillName { get; set; }

        //Optional fields
        public int SpriteURL { get; set; } = -1;
        public IList<int> TextFields { get; set; }
    }
}
