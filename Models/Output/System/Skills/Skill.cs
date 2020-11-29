﻿using Newtonsoft.Json;
using RedditEmblemAPI.Models.Configuration.System.Skills;
using RedditEmblemAPI.Models.Exceptions.Unmatched;
using RedditEmblemAPI.Models.Output.System.Skills.Effects;
using RedditEmblemAPI.Services.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RedditEmblemAPI.Models.Output.System.Skills
{
    /// <summary>
    /// Object representing a Skill definition in the team's system. 
    /// </summary>
    public class Skill
    {
        /// <summary>
        /// Flag indicating whether or not this skill was found on a unit. Used to minify the output JSON.
        /// </summary>
        [JsonIgnore]
        public bool Matched { get; set; }

        /// <summary>
        /// The name of the skill.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The sprite image URL for the skill.
        /// </summary>
        public string SpriteURL { get; set; }

        /// <summary>
        /// List of the skill's text fields.
        /// </summary>
        public IList<string> TextFields { get; set; }

        /// <summary>
        /// The effect the skill applies, if any.
        /// </summary>
        [JsonIgnore]
        public ISkillEffect Effect { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        public Skill(SkillsConfig config, IList<string> data)
        {
            this.Name = ParseHelper.SafeStringParse(data, config.Name, "Name", true);
            this.SpriteURL = ParseHelper.SafeStringParse(data, config.SpriteURL, "Sprite URL", false);
            this.TextFields = ParseHelper.StringListParse(data, config.TextFields);
            this.Effect = BuildSkillEffect(ParseHelper.SafeStringParse(data, config.Effect.Type, "Skill Effect Type", false),
                                           ParseHelper.StringListParse(data, config.Effect.Parameters, true));
        }

        private ISkillEffect BuildSkillEffect(string effectType, IList<string> parameters)
        {
            if (string.IsNullOrEmpty(effectType))
                return null;

            switch (effectType)
            {
                //Stat Modifier Effects
                case "StatModifier": return new StatModifierEffect(parameters);
                case "CombatStatModifier": return new CombatStatModifierEffect(parameters);
                //Equipped Item Modifier Effects
                case "EquippedCombatStatModifier": return new EquippedItemCombatStatModifierEffect(parameters);
                case "EquippedStatModifier": return new EquippedStatModifierEffect(parameters);
                //Terrain Type Modifier Effects
                case "TerrainTypeCombatStatModifer": return new TerrainTypeCombatStatModiferEffect(parameters);
                case "TerrainTypeStatModifer": return new TerrainTypeStatModiferEffect(parameters);
                //Unit/Item Range Modifier Effects
                case "TerrainTypeMovementCostModifier": return new TerrainTypeMovementCostModifierEffect(parameters);
                case "TerrainTypeMovementCostSet": return new TerrainTypeMovementCostSetEffect(parameters);
                case "ItemMaxRangeModifier": return new ItemMaxRangeModifierEffect(parameters);
                case "IgnoreUnitAffiliations": return new IgnoreUnitAffiliationsEffect(parameters);
                //Unit Radius Stat Modifier Effects
                case "AllyRadiusCombatStatModifer": return new AllyRadiusCombatStatModiferEffect(parameters);
                case "AllyRadiusStatModifer": return new AllyRadiusStatModiferEffect(parameters);
                case "EnemyRadiusCombatStatModifer": return new EnemyRadiusCombatStatModiferEffect(parameters);
                case "EnemyRadiusStatModifer": return new EnemyRadiusStatModiferEffect(parameters);
            }

            throw new UnmatchedSkillEffectException(effectType);
        }
    }
}
