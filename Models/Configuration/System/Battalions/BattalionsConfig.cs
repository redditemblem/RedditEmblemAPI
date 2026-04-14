using Newtonsoft.Json;
using RedditEmblemAPI.Models.Configuration.Common;
using System;

namespace RedditEmblemAPI.Models.Configuration.System.Battalions
{
    /// <summary>
    /// Container class for deserialized JSON <c>"Battalions"</c> object data.
    /// </summary>
    public class BattalionsConfig : MultiQueryable
    {
        #region Required Fields

        /// <summary>
        /// Required. Location of a battalion's gambit name value.
        /// </summary>
        [JsonRequired]
        public (int, int) Gambit { get; set; }

        /// <summary>
        /// Required. Location of a battalion's max endurance value.
        /// </summary>
        [JsonRequired]
        public (int, int) MaxEndurance { get; set; }

        /// <summary>
        /// Required. Collection of the battalion's stats.
        /// </summary>
        [JsonRequired]
        public NamedStatConfig[] Stats { get; set; }

        #endregion Required Fields

        #region Optional Fields

        /// <summary>
        /// Optional. Location of the battalion's icon sprite URL.
        /// </summary>
        public (int, int) SpriteURL { get; set; } = (-1, -1);

        /// <summary>
        /// Optional. Location of the authority (or other) rank required to use this battalion.
        /// </summary>
        public (int, int) Rank { get; set; } = (-1, -1);

        /// <summary>
        /// Optional. Collection of modifiers that should be applied to the unit's general stats when this battalion is equipped.
        /// </summary>
        public NamedStatConfig[] StatModifiers { get; set; } = Array.Empty<NamedStatConfig>();

        /// <summary>
        /// Optional. Collection of locations for any text information about the battalion to display.
        /// </summary>
        public (int, int)[] TextFields { get; set; } = Array.Empty<(int, int)>();

        #endregion Optional Fields
    }
}
