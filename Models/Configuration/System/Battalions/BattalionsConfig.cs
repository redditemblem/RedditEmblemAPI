using Newtonsoft.Json;
using RedditEmblemAPI.Models.Configuration.Common;
using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Configuration.System.Battalions
{
    /// <summary>
    /// Container class for deserialized JSON <c>"Battalions"</c> object data.
    /// </summary>
    public class BattalionsConfig
    {
        #region Required Fields

        [JsonRequired]
        public Query Query { get; set; }

        /// <summary>
        /// Required. Cell index of a battalion's name value.
        /// </summary>
        [JsonRequired]
        public int Name { get; set; }

        /// <summary>
        /// Required. The cell index of a battalion's gambit name value.
        /// </summary>
        [JsonRequired]
        public int Gambit { get; set; }

        /// <summary>
        /// Required. The cell index of a battalion's max endurance value.
        /// </summary>
        [JsonRequired]
        public int MaxEndurance { get; set; }

        /// <summary>
        /// Required. List of the battalion's stats.
        /// </summary>
        [JsonRequired]
        public List<NamedStatConfig> Stats { get; set; }


        #endregion Required Fields

        #region Optional Fields

        /// <summary>
        /// Optional. Cell index for the battalion's icon sprite URL.
        /// </summary>
        public int SpriteURL { get; set; } = -1;

        /// <summary>
        /// Optional. The authority (or other) rank required to use this battalion.
        /// </summary>
        public int Rank { get; set; } = -1;

        /// <summary>
        /// Optional. Any modifiers that should be applied to the unit's general stats when this battalion is equipped.
        /// </summary>
        public List<NamedStatConfig> StatModifiers { get; set; } = new List<NamedStatConfig>();

        /// <summary>
        /// Optional. List of cell indexes for any text information about the battalion to display.
        /// </summary>
        public List<int> TextFields { get; set; } = new List<int>();

        #endregion
    }
}
