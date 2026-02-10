using Newtonsoft.Json;
using RedditEmblemAPI.Models.Configuration.Common;
using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Configuration.System.Battalions
{
    /// <summary>
    /// Container class for deserialized JSON <c>"Gambits"</c> object data.
    /// </summary>
    public class GambitsConfig : MultiQueryable
    {
        #region Required Fields

        /// <summary>
        /// Required. Cell index of a gambit's max uses value.
        /// </summary>
        [JsonRequired]
        public int MaxUses { get; set; }

        /// <summary>
        /// Required. Container object for gambit's range configuration.
        /// </summary>
        [JsonRequired]
        public GambitRangeConfig Range { get; set; }

        /// <summary>
        /// Required. List of the gambit's stats.
        /// </summary>
        [JsonRequired]
        public List<NamedStatConfig> Stats { get; set; }

        #endregion Required Fields

        #region Optional Fields

        /// <summary>
        /// Optional. Cell index for the gambit's icon sprite URL.
        /// </summary>
        public int SpriteURL { get; set; } = -1;

        /// <summary>
        /// Optional. The unit stats (ex. Str/Mag/etc) that the gambit uses.
        /// </summary>
        public int UtilizedStats { get; set; } = -1;

        /// <summary>
        /// Optional. List of cell indexes for any text information about the gambit to display.
        /// </summary>
        public List<int> TextFields { get; set; } = new List<int>();

        #endregion Optional Fields

    }
}
