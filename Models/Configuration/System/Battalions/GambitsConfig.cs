using Newtonsoft.Json;
using RedditEmblemAPI.Models.Configuration.Common;
using System;

namespace RedditEmblemAPI.Models.Configuration.System.Battalions
{
    /// <summary>
    /// Container class for deserialized JSON <c>"Gambits"</c> object data.
    /// </summary>
    public class GambitsConfig : MultiQueryable
    {
        #region Required Fields

        /// <summary>
        /// Required. Location of a gambit's max uses value.
        /// </summary>
        [JsonRequired]
        public (int, int) MaxUses { get; set; }

        /// <summary>
        /// Required. Container object for gambit's range configuration.
        /// </summary>
        [JsonRequired]
        public GambitRangeConfig Range { get; set; }

        /// <summary>
        /// Required. Collection of the gambit's stats.
        /// </summary>
        [JsonRequired]
        public NamedStatConfig[] Stats { get; set; }

        #endregion Required Fields

        #region Optional Fields

        /// <summary>
        /// Optional. Location of the gambit's icon sprite URL.
        /// </summary>
        public (int, int) SpriteURL { get; set; } = (-1, -1);

        /// <summary>
        /// Optional. Collection of locations of the names of the unit stats (ex. Str/Mag/etc) that the gambit uses.
        /// </summary>
        public (int, int)[] UtilizedStats { get; set; } = Array.Empty<(int, int)>();

        /// <summary>
        /// Optional. Collection of locations of any text information about the gambit to display.
        /// </summary>
        public (int, int)[] TextFields { get; set; } = Array.Empty<(int, int)>();

        #endregion Optional Fields
    }
}
