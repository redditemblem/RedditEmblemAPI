using Newtonsoft.Json;
using RedditEmblemAPI.Models.Configuration.Units;
using RedditEmblemAPI.Models.Output.System;
using RedditEmblemAPI.Services.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace RedditEmblemAPI.Models.Output.Units
{
    //Partial class for handling mechanics from Fire Emblem: Three Houses.
    public partial class Unit
    {
        #region Attributes

        /// <summary>
        /// List of the combat arts the unit possesses.
        /// </summary>
        [JsonIgnore]
        public List<CombatArt> CombatArtsList { get; set; }

        /// <summary>
        /// Container for information about a unit's battalion.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public UnitBattalion Battalion { get; set; }

        /// <summary>
        /// The unit's adjutants.
        /// </summary>
        [JsonIgnore]
        public List<Adjutant> AdjutantList { get; set; }

        #region JSON Serialization

        /// <summary>
        /// Only for JSON serialization. A list of the unit's combat arts.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        private IEnumerable<string> CombatArts { get { return this.CombatArtsList.Any() ? this.CombatArtsList.Select(c => c.Name) : null; } }

        /// <summary>
        /// Only for JSON serialization. The unit's adjutants.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        private IEnumerable<string> Adjutants
        {
            get { return this.AdjutantList.Any() ? this.AdjutantList.Select(a => a.Name) : null; }
        }

        #endregion JSON Serialization

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
            this.CombatArtsList = BuildCombatArts(data, config.CombatArts, system.CombatArts);
            this.Battalion = BuildBattalion(data, config.Battalion, system.Battalions);
            this.AdjutantList = BuildAdjutants(data, config.Adjutants, system.Adjutants);
        }

        #region Build Functions

        /// <summary>
        /// Iterates through the values in <paramref name="data"/> at <paramref name="indexes"/> and attempts to match them to a <c>CombatArt</c> from <paramref name="combatArts"/>.
        /// </summary>
        private List<CombatArt> BuildCombatArts(IEnumerable<string> data, List<int> indexes, IDictionary<string, CombatArt> combatArts)
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
        private UnitBattalion BuildBattalion(IEnumerable<string> data, UnitBattalionConfig config, IDictionary<string, Battalion> battalions)
        {
            if (config == null) return null;

            string name = DataParser.OptionalString(data, config.Battalion, "Battalion");
            if (string.IsNullOrEmpty(name)) return null;

            UnitBattalion battalion = new UnitBattalion(config, data, battalions);
            this.Stats.ApplyGeneralStatModifiers(battalion.BattalionObj.StatModifiers, battalion.BattalionObj.Name);

            return battalion;
        }

        /// <summary>
        /// Iterates through the values in <paramref name="data"/> at <paramref name="indexes"/> and attempts to match them to a <c>Adjutant</c> from <paramref name="adjutants"/>.
        /// </summary>
        /// <remarks>
        /// Dependent on Stats.
        /// </remarks>
        private List<Adjutant> BuildAdjutants(IEnumerable<string> data, List<int> indexes, IDictionary<string, Adjutant> adjutants)
        {
            List<string> names = DataParser.List_Strings(data, indexes);
            List<Adjutant> matches = Adjutant.MatchNames(adjutants, names);

            foreach (Adjutant adjutant in matches)
            {
                this.Stats.ApplyCombatStatModifiers(adjutant.CombatStatModifiers, adjutant.Name);
                this.Stats.ApplyGeneralStatModifiers(adjutant.StatModifiers, adjutant.Name);
            }

            return matches;
        }

        #endregion Build Functions
    }
}
