using RedditEmblemAPI.Models.Configuration.System.TileObjects;
using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Services.Helpers;
using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Output.System
{
    /// <summary>
    /// Object representing an <c>TileObject</c>'s range.
    /// </summary>
    public class TileObjectRange
    {
        /// <summary>
        /// The minimum number of tiles a terrain object can reach.
        /// </summary>
        public int Minimum { get; private set; }

        /// <summary>
        /// The maximum number of tiles a terrain object can reach.
        /// </summary>
        public int Maximum { get; private set; }

        /// <summary>
        /// Default constructor. Sets <c>Minimum</c> and <c>Maximum</c> to 0.
        /// </summary>
        public TileObjectRange()
        {
            this.Minimum = 0;
            this.Maximum = 0;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <exception cref="MinimumGreaterThanMaximumException"></exception>
        public TileObjectRange(TileObjectRangeConfig config, List<string> data)
        {
            this.Minimum = DataParser.OptionalInt_Positive(data, config.Minimum, "Minimum Range");
            this.Maximum = DataParser.OptionalInt_Positive(data, config.Maximum, "Maximum Range");

            if (this.Minimum > this.Maximum)
                throw new MinimumGreaterThanMaximumException("Minimum Range", "Maximum Range");
        }
    }
}