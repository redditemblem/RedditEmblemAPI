using RedditEmblemAPI.Models.Configuration.System.Battalions;
using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Services.Helpers;
using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Output.System
{
    #region Interface

    /// <inheritdoc cref="GambitRange"/>
    public interface IGambitRange
    {
        /// <inheritdoc cref="GambitRange.Minimum"/>
        int Minimum { get; }

        /// <inheritdoc cref="GambitRange.Maximum"/>
        int Maximum { get; }
    }

    #endregion Interface

    /// <summary>
    /// Object representing an <c>Gambit</c>'s range.
    /// </summary>
    public class GambitRange : IGambitRange
    {
        /// <summary>
        /// The minimum number of tiles a gambit can reach.
        /// </summary>
        public int Minimum { get; private set; }

        /// <summary>
        /// The maximum number of tiles a gambit can reach.
        /// </summary>
        public int Maximum { get; private set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <exception cref="MinimumGreaterThanMaximumException"></exception>
        public GambitRange(GambitRangeConfig config, IEnumerable<string> data)
        {
            this.Minimum = DataParser.Int_Positive(data, config.Minimum, "Minimum Range");
            this.Maximum = DataParser.Int_Positive(data, config.Maximum, "Maximum Range");

            if (this.Minimum > this.Maximum)
                throw new MinimumGreaterThanMaximumException("Minimum Range", "Maximum Range");
        }
    }
}