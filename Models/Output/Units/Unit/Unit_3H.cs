using Newtonsoft.Json;
using RedditEmblemAPI.Models.Configuration.Units;
using RedditEmblemAPI.Models.Output.System;
using RedditEmblemAPI.Models.Output.System.Match;
using RedditEmblemAPI.Services.Helpers;
using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Output.Units
{
    #region Interface
    
    /// <inheritdoc cref="Unit"/>
    public partial interface IUnit
    {
        /// <inheritdoc cref="Unit.CombatArts"/>
        List<ICombatArt> CombatArts { get; }

        /// <inheritdoc cref="Unit.Battalion"/>
        IUnitBattalion Battalion { get; }

        /// <inheritdoc cref="Unit.Adjutants"/>
        List<IAdjutant> Adjutants { get; }
    }
    
    #endregion Interface

    //Partial class for handling mechanics from Fire Emblem: Three Houses.
    public partial class Unit : IUnit
    {
        #region Attributes

        /// <summary>
        /// List of the combat arts the unit possesses.
        /// </summary>
        [JsonProperty(ItemConverterType = typeof(MatchableNameConverter))]
        public List<ICombatArt> CombatArts { get; private set; }

        /// <summary>
        /// Container for information about a unit's battalion.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public IUnitBattalion Battalion { get; private set; }

        /// <summary>
        /// The unit's adjutants.
        /// </summary>
        [JsonProperty(ItemConverterType = typeof(MatchableNameConverter))]
        public List<IAdjutant> Adjutants { get; private set; }

        #endregion Attributes

        /// <summary>
        /// Partial constructor. Builds combat arts, battalion, and adjutants.
        /// </summary>
        /// <remarks>
        /// Depends on the following being built beforehand:
        /// <list type="bullet">
        /// <item>Stats</item>
        /// </list>
        /// </remarks>
        public void Constructor_Unit_3H(UnitsConfig config, IEnumerable<string> data, SystemInfo system)
        {
            this.CombatArts = BuildCombatArts(data, config.CombatArts, system.CombatArts);
            this.Battalion = BuildBattalion(data, config.Battalion, system.Battalions);
            this.Adjutants = BuildAdjutants(data, config.Adjutants, system.Adjutants);
        }

        #region Build Functions

        /// <summary>
        /// Iterates through the values in <paramref name="data"/> at <paramref name="indexes"/> and attempts to match them to an <c>ICombatArt</c> from <paramref name="combatArts"/>.
        /// </summary>
        private List<ICombatArt> BuildCombatArts(IEnumerable<string> data, List<int> indexes, IDictionary<string, ICombatArt> combatArts)
        {
            List<string> names = DataParser.List_Strings(data, indexes);
            return CombatArt.MatchNames(combatArts, names);
        }

        /// <summary>
        /// Builds and returns the unit's battalion.
        /// </summary>
        /// <remarks>
        /// Depends on the following being built beforehand:
        /// <list type="bullet">
        /// <item>Stats</item>
        /// </list>
        /// </remarks>
        private IUnitBattalion BuildBattalion(IEnumerable<string> data, UnitBattalionConfig config, IDictionary<string, IBattalion> battalions)
        {
            if (config == null) return null;

            string name = DataParser.OptionalString(data, config.Battalion, "Battalion");
            if (string.IsNullOrEmpty(name)) return null;

            IUnitBattalion battalion = new UnitBattalion(config, data, battalions);
            this.Stats.ApplyGeneralStatModifiers(battalion.BattalionObj.StatModifiers, battalion.BattalionObj.Name);

            return battalion;
        }

        /// <summary>
        /// Iterates through the values in <paramref name="data"/> at <paramref name="indexes"/> and attempts to match them to a <c>IAdjutant</c> from <paramref name="adjutants"/>.
        /// </summary>
        /// <remarks>
        /// Dependent on Stats.
        /// </remarks>
        private List<IAdjutant> BuildAdjutants(IEnumerable<string> data, List<int> indexes, IDictionary<string, IAdjutant> adjutants)
        {
            List<string> names = DataParser.List_Strings(data, indexes);
            List<IAdjutant> matches = Adjutant.MatchNames(adjutants, names);

            foreach (IAdjutant adjutant in matches)
            {
                this.Stats.ApplyCombatStatModifiers(adjutant.CombatStatModifiers, adjutant.Name);
                this.Stats.ApplyGeneralStatModifiers(adjutant.StatModifiers, adjutant.Name);
            }

            return matches;
        }

        #endregion Build Functions
    }
}
