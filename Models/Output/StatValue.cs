using System.Collections.Generic;
using System.Linq;

namespace RedditEmblemAPI.Models.Output
{
    public class StatValue
    {
        public StatValue()
        {
            this.Modifiers = new Dictionary<string, int>();
        }

        public int BaseValue { get; set; }
        public int FinalValue { get { return this.BaseValue + this.Modifiers.Sum(m => m.Value); } }
        public Dictionary<string, int> Modifiers { get; set; }
    }
}