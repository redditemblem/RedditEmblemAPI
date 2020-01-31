using RedditEmblemAPI.Models.Configuration.Items;
using RedditEmblemAPI.Models.Configuration.System;
using RedditEmblemAPI.Models.Configuration.Team;
using RedditEmblemAPI.Models.Configuration.Units;

namespace RedditEmblemAPI.Models.Configuration
{
    public class JSONConfiguration
    {
        public TeamConfig Team;
        public SystemConfig System;
        public UnitsConfig Units;
        public ItemsConfig Items;
    }
}