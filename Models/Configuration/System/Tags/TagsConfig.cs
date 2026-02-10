using RedditEmblemAPI.Models.Configuration.Common;

namespace RedditEmblemAPI.Models.Configuration.System.Tags
{
    public class TagsConfig : MultiQueryable
    {
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