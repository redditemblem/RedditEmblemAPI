using RedditEmblemAPI.Models.Configuration.Items;
using RedditEmblemAPI.Models.Configuration.Skills;
using RedditEmblemAPI.Models.Configuration.System;
using RedditEmblemAPI.Models.Configuration.Team;
using RedditEmblemAPI.Models.Configuration.Units;

namespace RedditEmblemAPI.Models.Configuration
{
    public class JSONConfiguration
    {
        public TeamConfig Team { get; set; }
        public SystemConfig System { get; set; }
        public UnitsConfig Units { get; set; }
        public ItemsConfig Items { get; set; }
        public SkillsConfig Skills { get; set; }
    }
}