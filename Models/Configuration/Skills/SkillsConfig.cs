using RedditEmblemAPI.Models.Configuration.Common;

namespace RedditEmblemAPI.Models.Configuration.Skills
{
    public class SkillsConfig
    {
        public WorksheetQuery WorksheetQuery { get; set; }
        public int SkillName { get; set; }
        public int SpriteURL { get; set; }
        public int[] TextFields { get; set; }
    }
}
