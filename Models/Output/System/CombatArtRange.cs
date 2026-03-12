using RedditEmblemAPI.Helpers;
using RedditEmblemAPI.Models.Configuration.System.CombatArts;
using RedditEmblemAPI.Models.Exceptions.Validation;
using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Output.System
{
    #region Interface

    /// <inheritdoc cref="CombatArtRange"/>
    public interface ICombatArtRange
    {
        /// <inheritdoc cref="CombatArtRange.Minimum"/>
        int Minimum { get; }

        /// <inheritdoc cref="CombatArtRange.Maximum"/>
        int Maximum { get; }
    }

    #endregion Interface

    /// <summary>
    /// Object representing an <c>CombatArt</c>'s range.
    /// </summary>
    public class CombatArtRange : ICombatArtRange
    {
        /// <summary>
        /// The minimum number of tiles a combat art can reach.
        /// </summary>
        public int Minimum { get; private set; }

        /// <summary>
        /// The maximum number of tiles a combat art can reach.
        /// </summary>
        public int Maximum { get; private set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <exception cref="MinimumGreaterThanMaximumException"></exception>
        public CombatArtRange(CombatArtRangeConfig config, IEnumerable<string> data)
        {
            this.Minimum = DataParser.OptionalInt_Positive(data, config.Minimum, "Minimum Range");
            this.Maximum = DataParser.OptionalInt_Positive(data, config.Maximum, "Maximum Range");

            if (this.Minimum == 0 && this.Maximum > 0)
                throw new ItemRangeMinimumNotSetException("Minimum Range", "Maximum Range");
            if (this.Maximum == 0 && this.Minimum > 0)
                throw new ItemRangeMinimumNotSetException("Maximum Range", "Minimum Range");
            if (this.Minimum > this.Maximum)
                throw new MinimumGreaterThanMaximumException("Minimum Range", "Maximum Range");
        }
    }
}
