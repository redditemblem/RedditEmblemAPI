using Newtonsoft.Json;
using RedditEmblemAPI.Models.Configuration.Common;
using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Configuration.System.Adjutants
{
    /// <summary>
    /// Container class for deserialized JSON <c>"Adjutants"</c> object data.
    /// </summary>
    public class AdjutantsConfig : IMultiQueryable
    {
        #region Required Fields

        [JsonRequired]
        public IEnumerable<Query> Queries { get; set; }

        /// <summary>
        /// Required. Cell index of an adjutant's name value.
        /// </summary>
        [JsonRequired]
        public int Name { get; set; }

        #endregion Required Fields

        #region Optional Fields

        /// <summary>
        /// Optional. Cell index of an adjutant's sprite image URL value.
        /// </summary>
        public int SpriteURL { get; set; } = -1;

        /// <summary>
        /// Optional. Any modifiers that should be applied to the unit's combat stats when this adjutant is equipped.
        /// </summary>
        public List<NamedStatConfig> CombatStatModifiers { get; set; } = new List<NamedStatConfig>();

        /// <summary>
        /// Optional. Any modifiers that should be applied to the unit's general stats when this adjutant is equipped.
        /// </summary>
        public List<NamedStatConfig> StatModifiers { get; set; } = new List<NamedStatConfig>();

        /// <summary>
        /// Optional. List of cell indexes for an adjutant's text fields.
        /// </summary>
        public List<int> TextFields { get; set; } = new List<int>();

        #endregion Optional Fields
    }
}
