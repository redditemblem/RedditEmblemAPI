using RedditEmblemAPI.Helpers;
using RedditEmblemAPI.Models.Configuration.Units;
using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Output.Units
{
    #region Interface

    /// <inheritdoc cref="UnitSpriteData"/>
    public interface IUnitSpriteData
    {
        /// <inheritdoc cref="UnitSpriteData.SpriteURL"/>
        string SpriteURL { get; }

        /// <inheritdoc cref="UnitSpriteData.PortraitURL"/>
        string PortraitURL { get; }

        /// <inheritdoc cref="UnitSpriteData.HasMoved"/>
        bool HasMoved { get; }

        /// <inheritdoc cref="UnitSpriteData.Aura"/>
        string Aura { get; set; }
    }

    #endregion Interface

    /// <summary>
    /// Container for storing simple data about how to render a unit.
    /// </summary>
    public class UnitSpriteData : IUnitSpriteData
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
        public UnitSpriteData(UnitsConfig config, IEnumerable<string> data)
        {
            this.SpriteURL = DataParser.String_URL(data, config.SpriteURL, "Sprite URL");
            this.PortraitURL = DataParser.OptionalString_URL(data, config.PortraitURL, "Portrait URL");
            this.HasMoved = DataParser.OptionalBoolean_YesNo(data, config.HasMoved, "Has Moved");
            this.Aura = string.Empty;
        }
    }
}
