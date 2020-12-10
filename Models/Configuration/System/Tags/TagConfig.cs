using Newtonsoft.Json;
using RedditEmblemAPI.Models.Configuration.Common;

namespace RedditEmblemAPI.Models.Configuration.System.Tags
{
    public class TagConfig
    {
        #region Required Fields

        [JsonRequired]
        public Query Query { get; set; }

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

        #endregion
    }
}
