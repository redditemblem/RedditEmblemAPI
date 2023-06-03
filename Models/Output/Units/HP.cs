using Newtonsoft.Json;
using RedditEmblemAPI.Models.Configuration.Units;
using RedditEmblemAPI.Services.Helpers;
using System;
using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Output.Units
{
    /// <summary>
    /// Object representing a <c>Unit</c>'s HP stats. 
    /// </summary>
    public class HP
    {
        /// <summary>
        /// The current number of hit points a unit has.
        /// </summary>
        public int Current { get; set; }

        /// <summary>
        /// The maximum number of hit points a unit has.
        /// </summary>
        public int Maximum { get; set; }

        /// <summary>
        /// The percentage of hit points the unit has remaining.
        /// </summary>
        public decimal Percentage { get { return Math.Round((decimal)this.Current / this.Maximum, 2) * 100; } }

        /// <summary>
        /// The difference between <c>Maximum</c> and <c>Current</c> with a minimum possible value of 0.
        /// </summary>
        [JsonIgnore]
        public int Difference { get { return Math.Max(0, this.Maximum - this.Current); } }

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        public HP(IEnumerable<string> data, HPConfig config)
        {
            this.Current = DataParser.Int_Positive(data, config.Current, "Current HP");
            this.Maximum = DataParser.Int_NonZeroPositive(data, config.Maximum, "Maximum HP");
        }

        #endregion
    }
}
