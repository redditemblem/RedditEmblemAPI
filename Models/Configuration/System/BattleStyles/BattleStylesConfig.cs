using Newtonsoft.Json;
using RedditEmblemAPI.Models.Configuration.Common;
using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Configuration.System.BattleStyles
{
    /// <summary>
    /// Container class for deserialized JSON <c>"BattleStyles"</c> object data.
    /// </summary>
    public class BattleStylesConfig : IQueryable
    {
        #region Required Fields

        [JsonRequired]
        public Query Query { get; set; }

        /// <summary>
        /// Required. Cell index of a battle style's name value.
        /// </summary>
        [JsonRequired]
        public int Name { get; set; }

        #endregion

        #region Optional Fields

        /// <summary>
        /// Optional. Cell index for a battle style's sprite.
        /// </summary>
        public int SpriteURL { get; set; } = -1;

        /// <summary>
        /// Optional. List of cell indexes for a battle style's text fields.
        /// </summary>
        public List<int> TextFields { get; set; } = new List<int>();

        #endregion
    }
}
