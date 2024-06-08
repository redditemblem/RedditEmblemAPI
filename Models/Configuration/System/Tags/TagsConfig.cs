using Newtonsoft.Json;
using RedditEmblemAPI.Models.Configuration.Common;
using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Configuration.System.Tags
{
    public class TagsConfig : IMultiQueryable
    {
        #region Required Fields

        [JsonRequired]
        public IEnumerable<Query> Queries { get; set; }

        /// <summary>
        /// Required. Cell index for a tag's name value.
        /// </summary>
        [JsonRequired]
        public int Name { get; set; }

        #endregion

        #region Optional Fields

        /// <summary>
        /// Optional. Cell index for a tag's sprite.
        /// </summary>
        public int SpriteURL { get; set; } = -1;

        /// <summary>
        /// Optional. Cell index for a tag's Show On Unit flag.
        /// </summary>
        public int ShowOnUnit { get; set; } = -1;

        /// <summary>
        /// Optional. Cell index for a tag's unit aura hex.
        /// </summary>
        public int UnitAura { get; set; } = -1;

        #endregion
    }
}