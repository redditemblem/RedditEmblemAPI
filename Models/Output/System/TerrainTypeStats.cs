using Newtonsoft.Json;
using RedditEmblemAPI.Models.Configuration.System.TerrainTypes;
using RedditEmblemAPI.Services.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RedditEmblemAPI.Models.Output.System
{
    #region Interface

    /// <inheritdoc cref="TerrainTypeStats"/>
    public interface ITerrainTypeStats
    {
        /// <inheritdoc cref="TerrainTypeStats.AffiliationGroupings"/>
        List<int> AffiliationGroupings { get; set; }

        /// <inheritdoc cref="TerrainTypeStats.IsDefaultGroup"/>
        bool IsDefaultGroup { get; }

        /// <inheritdoc cref="TerrainTypeStats.HPModifier"/>
        int HPModifier { get; set; }

        /// <inheritdoc cref="TerrainTypeStats.CombatStatModifiers"/>
        IDictionary<string, int> CombatStatModifiers { get; set; }

        /// <inheritdoc cref="TerrainTypeStats.StatModifiers"/>
        IDictionary<string, int> StatModifiers { get; set; }

        /// <inheritdoc cref="TerrainTypeStats.MovementCosts"/>
        IDictionary<string, int> MovementCosts { get; set; }
    }

    #endregion Interface

    public class TerrainTypeStats : ITerrainTypeStats
    {
        #region Attributes

        /// <summary>
        /// The affiliation groupings this stats group applies to.
        /// </summary>
        [JsonIgnore]
        public List<int> AffiliationGroupings { get; set; }

        /// <summary>
        /// Returns true if this group has no <c>AffiliationGroupings</c> tied to it.
        /// </summary>
        [JsonIgnore]
        public bool IsDefaultGroup { get { return !this.AffiliationGroupings.Any(); } }

        /// <summary>
        /// The value by which the terrain type modifies a unit's HP. Assumed to be a percentage.
        /// </summary>
        public int HPModifier { get; set; }

        /// <summary>
        /// List of combat stat modifiers applied by the terrain type.
        /// </summary>
        public IDictionary<string, int> CombatStatModifiers { get; set; }

        /// <summary>
        /// List of stat modifiers applied by the terrain type.
        /// </summary>
        public IDictionary<string, int> StatModifiers { get; set; }

        /// <summary>
        /// List of movement costs for the terrain type.
        /// </summary>
        public IDictionary<string, int> MovementCosts { get; set; }

        #region JSON Serialization

        /// <summary>
        /// The names of the affiliations in the groupings from <c>this.AffiliationGroupings</c>.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        private List<string> AffiliationNames { get; set; }

        #endregion JSON Serialization

        #endregion Attributes

        public TerrainTypeStats(TerrainTypeStatsConfig config, IEnumerable<string> data, IDictionary<string, IAffiliation> affiliations) 
        {
            this.AffiliationGroupings = DataParser.List_IntCSV(data, config.AffiliationGroupings, "Affiliation Groupings", true);
            this.AffiliationNames = GetAffiliationGroupingNames(affiliations);

            this.HPModifier = DataParser.OptionalInt_Any(data, config.HPModifier, "HP Modifier");
            this.CombatStatModifiers = DataParser.NamedStatDictionary_OptionalInt_Any(config.CombatStatModifiers, data);
            this.StatModifiers = DataParser.NamedStatDictionary_OptionalInt_Any(config.StatModifiers, data);
            this.MovementCosts = DataParser.NamedStatDictionary_Int_NonZeroPositive(config.MovementCosts, data, "{0} Movement Cost");
        }

        private List<string> GetAffiliationGroupingNames(IDictionary<string, IAffiliation> affiliations)
        {
            List<string> names = new List<string>();
            foreach(int grouping in this.AffiliationGroupings)
            {
                bool foundMatch = false;
                foreach(IAffiliation aff in affiliations.Where(a => a.Value.Grouping == grouping).Select(a => a.Value))
                {
                    if (!names.Contains(aff.Name))
                    {
                        names.Add(aff.Name);
                        foundMatch = true;
                    }
                }

                if (!foundMatch)
                    throw new Exception($"No affiliations are included in affiliation grouping \"{grouping}\".");
            }

            if (!names.Any())
                return null;
            
            names.Sort();
            return names;
        }
    }
}
