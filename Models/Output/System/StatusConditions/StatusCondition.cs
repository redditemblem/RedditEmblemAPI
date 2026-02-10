using Newtonsoft.Json;
using RedditEmblemAPI.Models.Configuration.System.Statuses;
using RedditEmblemAPI.Models.Exceptions.Processing;
using RedditEmblemAPI.Models.Exceptions.Unmatched;
using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.System.Interfaces;
using RedditEmblemAPI.Models.Output.System.StatusConditions.Effects;
using RedditEmblemAPI.Services.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RedditEmblemAPI.Models.Output.System.StatusConditions
{
    #region Interface

    /// <inheritdoc cref="StatusCondition">
    public interface IStatusCondition : IMatchable
    {
        /// <inheritdoc cref="StatusCondition.SpriteURL">
        string SpriteURL { get; set; }

        /// <inheritdoc cref="StatusCondition.Type">
        StatusConditionType Type { get; set; }

        /// <inheritdoc cref="StatusCondition.Turns">
        int Turns { get; set; }

        /// <inheritdoc cref="StatusCondition.TextFields">
        List<string> TextFields { get; set; }

        /// <inheritdoc cref="StatusCondition.Effects">
        List<IStatusConditionEffect> Effects { get; set; }
    }

    #endregion Interface

    /// <summary>
    /// Object representing a status condition in a team's system.
    /// </summary>
    public class StatusCondition : Matchable, IStatusCondition
    {
        #region Attributes

        /// <summary>
        /// The sprite image URL for the status condition.
        /// </summary>
        public string SpriteURL { get; set; }

        /// <summary>
        /// The type of the status condition.
        /// </summary>
        public StatusConditionType Type { get; set; }

        /// <summary>
        /// The maximum number of turns the status condition will last.
        /// </summary>
        public int Turns { get; set; }

        /// <summary>
        /// List of the status condtion's text fields.
        /// </summary>
        public List<string> TextFields { get; set; }

        /// <summary>
        /// The effect the status condition applies, if any.
        /// </summary>
        [JsonIgnore]
        public List<IStatusConditionEffect> Effects { get; set; }

        #region JSON Serialization Only

        /// <summary>
        /// Flag indicating whether or not a status condition effect is configured on this status condition.
        /// </summary>
        [JsonProperty]
        private bool IsEffectConfigured { get { return this.Effects.Any(); } }

        #endregion JSON Serialization Only

        #endregion Attributes

        /// <summary>
        /// Constructor.
        /// </summary>
        public StatusCondition(StatusConditionConfig config, IEnumerable<string> data)
        {
            this.Name = DataParser.String(data, config.Name, "Name");
            this.SpriteURL = DataParser.OptionalString_URL(data, config.SpriteURL, "Sprite URL");
            this.Type = ParseStatusConditionType(data, config.Type);
            this.Turns = DataParser.OptionalInt_NonZeroPositive(data, config.Turns, "Turns", 0);
            this.TextFields = DataParser.List_Strings(data, config.TextFields);

            this.Effects = new List<IStatusConditionEffect>();
            foreach (StatusConditionEffectConfig effect in config.Effects)
            {
                string effectType = DataParser.OptionalString(data, effect.Type, "Status Condition Effect Type");
                List<string> effectParms = DataParser.List_Strings(data, effect.Parameters, true);

                if (!string.IsNullOrEmpty(effectType))
                    this.Effects.Add(BuildStatusConditionEffect(effectType, effectParms));
            }
        }

        /// <summary>
        /// Matches the value in <paramref name="data"/> at <paramref name="index"/> to a <c>StatusType</c> enum.
        /// </summary>
        /// <exception cref="UnmatchedStatusConditionTypeException"></exception>
        private StatusConditionType ParseStatusConditionType(IEnumerable<string> data, int index)
        {
            string name = DataParser.OptionalString(data, index, "Type");
            switch (name)
            {
                case "": return StatusConditionType.Unassigned;
                case "Positive": return StatusConditionType.Positive;
                case "Negative": return StatusConditionType.Negative;
                case "Neutral": return StatusConditionType.Neutral;
                default: throw new UnmatchedStatusConditionTypeException(name);
            }
        }

        private IStatusConditionEffect BuildStatusConditionEffect(string effectType, List<string> parameters)
        {
            switch (effectType)
            {
                case "OverrideMovement": return new OverrideMovementEffect(parameters);
                case "OverrideMovementType": return new OverrideMovementTypeEffect_Status(parameters);
                case "TerrainTypeMovementCostSet": return new TerrainTypeMovementCostSetEffect_Status(parameters);
                case "DoesNotBlockEnemyAffiliations": return new DoesNotBlockEnemyAffiliationsEffect(parameters);
                case "PreventAllItemUse": return new PreventAllItemUseEffect(parameters);
                case "PreventUtilStatItemUse": return new PreventUtilStatItemUseEffect(parameters);
                case "PreventCategoryItemUse": return new PreventCategoryItemUseEffect(parameters);
                case "CombatStatModifier": return new CombatStatModifierEffect(parameters);
                case "CombatStatModifierWithAdditionalStatMultiplier": return new CombatStatModifierWithAdditionalStatMultiplierEffect(parameters);
                case "StatModifier": return new StatModifierEffect(parameters);
                case "StatModifierWithAdditionalStatMultiplier": return new StatModifierWithAdditionalStatMultiplierEffect(parameters);
                case "AddTag": return new AddTagEffect(parameters);
                case "RemoveTag": return new RemoveTagEffect(parameters);
            }

            throw new UnmatchedStatusConditionEffectException(effectType);
        }

        #region Static Functions

        /// <summary>
        /// Iterates through the data in <paramref name="config"/>'s <c>Query</c> and builds an <c>IStatusCondition</c> from each valid row.
        /// </summary>
        /// <exception cref="StatusConditionProcessingException"></exception>
        public static IDictionary<string, IStatusCondition> BuildDictionary(StatusConditionConfig config)
        {
            IDictionary<string, IStatusCondition> statusConditions = new Dictionary<string, IStatusCondition>();
            if (config == null || config.Queries == null)
                return statusConditions;

            foreach (List<object> row in config.Queries.SelectMany(q => q.Data))
            {
                string name = string.Empty;
                try
                {
                    IEnumerable<string> stat = row.Select(r => r.ToString());
                    name = DataParser.OptionalString(stat, config.Name, "Name");
                    if (string.IsNullOrEmpty(name)) continue;

                    if (!statusConditions.TryAdd(name, new StatusCondition(config, stat)))
                        throw new NonUniqueObjectNameException("status condition");
                }
                catch (Exception ex)
                {
                    throw new StatusConditionProcessingException(name, ex);
                }
            }

            return statusConditions;
        }

        /// <summary>
        /// Matches each of the strings in <paramref name="names"/> to an <c>IStatusCondition</c> in <paramref name="statusConditions"/> and returns the matches as a list.
        /// </summary>
        /// <param name="flagAsMatched">If true, calls <c>IMatchable.FlagAsMatched</c> for all returned objects.</param>
        public static List<IStatusCondition> MatchNames(IDictionary<string, IStatusCondition> statusConditions, IEnumerable<string> names, bool flagAsMatched = true)
        {
            return names.Select(n => MatchName(statusConditions, n, flagAsMatched)).ToList();
        }

        /// <summary>
        /// Matches <paramref name="name"/> to an <c>IStatusCondition</c> in <paramref name="statusConditions"/> and returns it.
        /// </summary>
        /// <param name="flagAsMatched">If true, calls <c>IMatchable.FlagAsMatched</c> for the returned object.</param>
        /// <exception cref="UnmatchedStatusConditionException"></exception>
        public static IStatusCondition MatchName(IDictionary<string, IStatusCondition> statusConditions, string name, bool flagAsMatched = true)
        {
            IStatusCondition match;
            if (!statusConditions.TryGetValue(name, out match))
                throw new UnmatchedStatusConditionException(name);

            if (flagAsMatched) match.FlagAsMatched();

            return match;
        }

        #endregion Static Functions
    }

    public enum StatusConditionType
    {
        Unassigned = 0,
        Positive = 1,
        Negative = 2,
        Neutral = 3
    }
}
