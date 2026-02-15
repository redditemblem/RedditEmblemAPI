using Newtonsoft.Json;
using RedditEmblemAPI.Models.Configuration.Units;
using RedditEmblemAPI.Models.Output.System;
using RedditEmblemAPI.Services.Helpers;
using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Output.Units
{
    #region Interface

    /// <inheritdoc cref="UnitBattalion"/>
    public interface IUnitBattalion
    {
        /// <inheritdoc cref="UnitBattalion.BattalionObj"/>
        IBattalion BattalionObj { get; }

        /// <inheritdoc cref="UnitBattalion.Endurance"/>
        int Endurance { get; }

        /// <inheritdoc cref="UnitBattalion.GambitUses"/>
        int GambitUses { get; }
    }

    #endregion Interface

    /// <summary>
    /// Container object for storing data about a unit's battalion.
    /// </summary>
    public class UnitBattalion : IUnitBattalion
    {
        #region Attributes

        /// <summary>
        /// The <c>Battalion</c> object represented by this unit's instance.
        /// </summary>
        [JsonIgnore]
        public IBattalion BattalionObj { get; }

        /// <summary>
        /// The endurance the battalion has remaining.
        /// </summary>
        public int Endurance { get; }

        /// <summary>
        /// The number of remaining uses the battalion's gambit has remaining.
        /// </summary>
        public int GambitUses { get; }

        #region JSON Serialization Only

        [JsonProperty]
        private string Name { get { return this.BattalionObj?.Name; } }

        [JsonProperty]
        private string GambitName { get { return this.BattalionObj?.GambitObj?.Name; } }

        #endregion JSON Serialization Only

        #endregion Attributes

        public UnitBattalion(UnitBattalionConfig config, IEnumerable<string> data, IDictionary<string, IBattalion> battalions)
        {
            string name = DataParser.String(data, config.Battalion, "Battalion");
            this.BattalionObj = Battalion.MatchName(battalions, name);

            this.Endurance = DataParser.Int_Positive(data, config.Endurance, "Battalion Endurance");
            this.GambitUses = DataParser.Int_Positive(data, config.GambitUses, "Gambit Uses");
        }
    }
}
