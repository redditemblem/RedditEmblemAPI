using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Output
{
    public class Skill
    {
        public Skill()
        {
            this.TextFields = new List<string>();
        }

        public string Name { get; set; }
        public string SpriteURL { get; set; }
        public IList<string> TextFields { get; set; }
}
}
