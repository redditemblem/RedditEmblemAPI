using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Configuration
{
    public class StatConfig
    {
        public string Name;
        public int BaseValue;
        public IList<ModifierConfig> Modifiers;
    }
}
