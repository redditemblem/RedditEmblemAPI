using RedditEmblemAPI.Models.Configuration.Units;
using RedditEmblemAPI.Services.Helpers;
using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Output.Units
{
    /// <summary>
    /// Container for storing simple data about how to render a unit.
    /// </summary>
    public class UnitSpriteData
    {
        #region Attributes

        /// <summary>
        /// The sprite image URL for the unit.
        /// </summary>
        public string SpriteURL { get; private set; }

        /// <summary>
        /// The portrait image URL for the unit.
        /// </summary>
        public string PortraitURL { get; private set; }

        /// <summary>
        /// Flag indicating whether or not a unit's turn has been processed.
        /// </summary>
        public bool HasMoved { get; private set; }

        /// <summary>
        /// Hex code for the unit's sprite aura.
        /// </summary>
        public string Aura { get; set; }

        #endregion Attributes

        /// <summary>
        /// Constructor.
        /// </summary>
        public UnitSpriteData(UnitsConfig config, List<string> data)
        {
            this.SpriteURL = DataParser.String_URL(data, config.SpriteURL, "Sprite URL");
            this.PortraitURL = DataParser.OptionalString_URL(data, config.PortraitURL, "Portrait URL");
            this.HasMoved = DataParser.OptionalBoolean_YesNo(data, config.HasMoved, "Has Moved");
            this.Aura = string.Empty;
        }
    }
}
