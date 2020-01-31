using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Configuration.Common
{
    public class ModifiedNamedStatConfig
    {
        public string Name;
        public int BaseValue;
        public IList<NamedStatConfig> Modifiers;
    }
}
