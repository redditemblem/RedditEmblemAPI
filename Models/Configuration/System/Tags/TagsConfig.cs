using RedditEmblemAPI.Models.Configuration.Common;

namespace RedditEmblemAPI.Models.Configuration.System.Tags
{
    public class TagsConfig : MultiQueryable
    {
        #region Optional Fields

        /// <summary>
        /// Optional. Location of a tag's sprite.
        /// </summary>
        public (int, int) SpriteURL { get; set; } = (-1, -1);

        /// <summary>
        /// Optional. Location of a tag's Show On Unit flag.
        /// </summary>
        public (int, int) ShowOnUnit { get; set; } = (-1, -1);

        /// <summary>
        /// Optional. Location of a tag's unit aura hex.
        /// </summary>
        public (int, int) UnitAura { get; set; } = (-1, -1);

        #endregion Optional Fields
    }
}