using Newtonsoft.Json;
using RedditEmblemAPI.Models.Configuration.Units;
using RedditEmblemAPI.Services.Helpers;
using System;
using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Output.Units
{
    #region Interface

    /// <inheritdoc cref="HealthPoints"/>
    public interface IHealthPoints
    {
        /// <inheritdoc cref="HealthPoints.Current"/>
        int Current { get; }

        /// <inheritdoc cref="HealthPoints.Maximum"/>
        int Maximum { get; }

        /// <inheritdoc cref="HealthPoints.Percentage"/>
        decimal Percentage { get; }

        /// <inheritdoc cref="HealthPoints.Difference"/>
        int Difference { get; }

        /// <inheritdoc cref="HealthPoints.RemainingBars"/>
        int RemainingBars { get; }
    }

    #endregion Interface

    /// <summary>
    /// Object representing a <c>Unit</c>'s HP stats. 
    /// </summary>
    public readonly struct HealthPoints : IHealthPoints
    {
        /// <summary>
        /// The current number of hit points a unit has.
        /// </summary>
        public int Current { get; }

        /// <summary>
        /// The maximum number of hit points a unit has.
        /// </summary>
        public int Maximum { get; }

        /// <summary>
        /// The percentage of hit points the unit has remaining.
        /// </summary>
        public decimal Percentage { get { return Math.Round((decimal)this.Current / this.Maximum, 2) * 100; } }

        /// <summary>
        /// The difference between <c>Maximum</c> and <c>Current</c> with a minimum possible value of 0.
        /// </summary>
        [JsonIgnore]
        public int Difference { get { return Math.Max(0, this.Maximum - this.Current); } }

        /// <summary>
        /// The number of remaining bonus hit point bars the unit possesses.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int RemainingBars { get; }

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        public HealthPoints(IEnumerable<string> data, HPConfig config)
        {
            this.Current = DataParser.Int_Positive(data, config.Current, "Current HP");
            this.Maximum = DataParser.Int_NonZeroPositive(data, config.Maximum, "Maximum HP");
            this.RemainingBars = DataParser.OptionalInt_Positive(data, config.RemainingBars, "Remaining HP Bars");
        }

        #endregion
    }
}
