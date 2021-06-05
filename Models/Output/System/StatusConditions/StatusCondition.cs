using Newtonsoft.Json;
using RedditEmblemAPI.Models.Configuration.System.Statuses;
using RedditEmblemAPI.Models.Exceptions.Processing;
using RedditEmblemAPI.Models.Exceptions.Unmatched;
using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.System.StatusConditions.Effects;
using RedditEmblemAPI.Services.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RedditEmblemAPI.Models.Output.System.StatusConditions
{
    /// <summary>
    /// Object representing a status condition in a team's system.
    /// </summary>
    public class StatusCondition
    {
        #region Attributes

        /// <summary>
        /// Flag indicating whether or not this status was found on a unit. Used to minify the output JSON.
        /// </summary>
        [JsonIgnore]
        public bool Matched { get; set; }

        /// <summary>
        /// The name of the status condition.
        /// </summary>
        public string Name { get; set; }

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
        public IList<string> TextFields { get; set; }

        /// <summary>
        /// The effect the status condition applies, if any.
        /// </summary>
        [JsonIgnore]
        public StatusConditionEffect Effect { get; set; }

        #endregion

        /// <summary>
        /// Constructor.
        /// </summary>
        public StatusCondition(StatusConditionConfig config, IList<string> data)
        {
            this.Name = ParseHelper.SafeStringParse(data, config.Name, "Name", true);
            this.SpriteURL = ParseHelper.SafeStringParse(data, config.SpriteURL, "Sprite URL", false);
            this.Type = ParseStatusConditionType(data, config.Type);
            this.Turns = ParseHelper.OptionalInt_NonZeroPositive(data, config.Turns, "Turns", 0);
            this.TextFields = ParseHelper.StringListParse(data, config.TextFields);

            //Check if status condition effects are configured
            if (config.Effect != null)
                this.Effect = BuildStatusConditionEffect(ParseHelper.SafeStringParse(data, config.Effect.Type, "Status Condition Effect Type", false),
                                                         ParseHelper.StringListParse(data, config.Effect.Parameters, true));
            else this.Effect = null;
        }

        /// <summary>
        /// Matches the value in <paramref name="data"/> at <paramref name="index"/> to a <c>StatusType</c> enum.
        /// </summary>
        /// <exception cref="UnmatchedStatusConditionTypeException"></exception>
        private StatusConditionType ParseStatusConditionType(IList<string> data, int index)
        {
            string name = ParseHelper.SafeStringParse(data, index, "Type", true);
            switch (name)
            {
                case "Positive": return StatusConditionType.Positive;
                case "Negative": return StatusConditionType.Negative;
                case "Neutral": return StatusConditionType.Neutral;
                default: throw new UnmatchedStatusConditionTypeException(name);
            }
        }

        private StatusConditionEffect BuildStatusConditionEffect(string effectType, IList<string> parameters)
        {
            if (string.IsNullOrEmpty(effectType))
                return null;

            switch (effectType)
            {
                case "OverrideMovement": return new OverrideMovementEffect(parameters);
                case "PreventAllItemUse": return new PreventAllItemUseEffect(parameters);
                case "PreventUtilStatItemUse": return new PreventUtilStatItemUseEffect(parameters);
                case "CombatStatModifier": return new CombatStatModifierEffect(parameters);
                case "StatModifier": return new StatModifierEffect(parameters);
                case "RemoveTag": return new RemoveTagEffect(parameters);
            }

            throw new UnmatchedStatusConditionEffectException(effectType);
        }

        #region Static Functions

        public static IDictionary<string, StatusCondition> BuildDictionary(StatusConditionConfig config)
        {
            IDictionary<string, StatusCondition> statusConditions = new Dictionary<string, StatusCondition>();

            foreach (IList<object> row in config.Query.Data)
            {
                try
                {
                    IList<string> stat = row.Select(r => r.ToString()).ToList();
                    string name = ParseHelper.SafeStringParse(stat, config.Name, "Name", false);
                    if (string.IsNullOrEmpty(name)) continue;

                    if (!statusConditions.TryAdd(name, new StatusCondition(config, stat)))
                        throw new NonUniqueObjectNameException("status condition");
                }
                catch (Exception ex)
                {
                    throw new StatusConditionProcessingException((row.ElementAtOrDefault(config.Name) ?? string.Empty).ToString(), ex);
                }
            }

            return statusConditions;
        }

        #endregion
    }

    public enum StatusConditionType
    {
        Positive = 0,
        Negative = 1,
        Neutral = 2
    }
}
