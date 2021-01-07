using Newtonsoft.Json;
using RedditEmblemAPI.Models.Configuration.System.Skills;
using RedditEmblemAPI.Models.Exceptions.Unmatched;
using RedditEmblemAPI.Models.Output.System.Skills.Effects;
using RedditEmblemAPI.Models.Output.System.Skills.Effects.EquippedItem;
using RedditEmblemAPI.Models.Output.System.Skills.Effects.ItemRange;
using RedditEmblemAPI.Models.Output.System.Skills.Effects.MovementRange;
using RedditEmblemAPI.Models.Output.System.Skills.Effects.Radius;
using RedditEmblemAPI.Models.Output.System.Skills.Effects.TerrainType;
using RedditEmblemAPI.Models.Output.System.Skills.Effects.UnitStats;
using RedditEmblemAPI.Services.Helpers;
using System.Collections.Generic;

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
        public SkillEffect Effect { get; set; }

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

        private SkillEffect BuildSkillEffect(string effectType, IList<string> parameters)
        {
            if (string.IsNullOrEmpty(effectType))
                return null;

            switch (effectType)
            {
                //Unit Stat Effects
                case "CombatStatModifier": return new CombatStatModifierEffect(parameters);
                case "HPBelowCombatStatModifier": return new HPBelowCombatStatModifierEffect(parameters);
                case "HPAboveCombatStatModifier": return new HPAboveCombatStatModifierEffect(parameters);
                case "StatModifier": return new StatModifierEffect(parameters);
                case "HPBelowStatModifier": return new HPBelowStatModifierEffect(parameters);
                case "HPAboveStatModifier": return new HPAboveStatModifierEffect(parameters);
                //Equipped Item Effects
                case "EquippedCategoryCombatStatModifier": return new EquippedCategoryCombatStatModifierEffect(parameters);
                case "EquippedCategoryStatModifier": return new EquippedCategoryStatModifierEffect(parameters);
                //Terrain Type Effects
                case "TerrainTypeCombatStatModifier": return new TerrainTypeCombatStatModifierEffect(parameters);
                case "TerrainTypeStatModifier": return new TerrainTypeStatModifierEffect(parameters);
                //Unit Movement Range Effects
                    //Movement Costs
                case "TerrainTypeMovementCostModifier": return new TerrainTypeMovementCostModifierEffect(parameters);
                case "TerrainTypeMovementCostSet": return new TerrainTypeMovementCostSetEffect(parameters);
                case "WarpMovementCostModifier": return new WarpMovementCostModifierEffect(parameters);
                case "WarpMovementCostSet": return new WarpMovementCostSetEffect(parameters);
                    //Affiliations
                case "IgnoreUnitAffiliations": return new IgnoreUnitAffiliationsEffect(parameters);
                case "HPBelowIgnoreUnitAffiliations": return new HPBelowIgnoreUnitAffiliationsEffect(parameters);
                case "HPAboveIgnoreUnitAffiliations": return new HPAboveIgnoreUnitAffiliationsEffect(parameters);
                case "ObstructTileRadius": return new ObstructTileRadiusEffect(parameters);
                case "HPBelowObstructTileRadius": return new HPBelowObstructTileRadiusEffect(parameters);
                case "HPAboveObstructTileRadius": return new HPAboveObstructTileRadiusEffect(parameters);
                    //Teleportation
                case "AllyRadiusTeleport": return new AllyRadiusTeleportEffect(parameters);
                case "HPBelowAllyRadiusTeleport": return new HPBelowAllyRadiusTeleportEffect(parameters);
                case "HPAboveAllyRadiusTeleport": return new HPAboveAllyRadiusTeleportEffect(parameters);
                case "AllyHPBelowAllyRadiusTeleport": return new AllyHPBelowAllyRadiusTeleportEffect(parameters);
                case "AllyHPAboveAllyRadiusTeleport": return new AllyHPAboveAllyRadiusTeleportEffect(parameters);
                case "EnemyRadiusTeleport": return new EnemyRadiusTeleportEffect(parameters);
                case "HPBelowEnemyRadiusTeleport": return new HPBelowEnemyRadiusTeleportEffect(parameters);
                case "HPAboveEnemyRadiusTeleport": return new HPAboveEnemyRadiusTeleportEffect(parameters);
                //Unit Item Range Effects
                case "ItemAllowMeleeRange": return new ItemAllowMeleeRangeEffect(parameters);
                case "ItemMinRangeSet": return new ItemMinRangeSetEffect(parameters);
                case "ItemMinRangeModifier": return new ItemMinRangeModifierEffect(parameters);
                case "ItemMaxRangeSet": return new ItemMaxRangeSetEffect(parameters);
                case "ItemMaxRangeModifier": return new ItemMaxRangeModifierEffect(parameters);
                //Unit Radius Stat Effects
                    //Normal
                case "AllyRadiusCombatStatModifier": return new AllyRadiusCombatStatModifierEffect(parameters);
                case "AllyRadiusStatModifier": return new AllyRadiusStatModifierEffect(parameters);
                case "EnemyRadiusCombatStatModifier": return new EnemyRadiusCombatStatModifierEffect(parameters);
                case "EnemyRadiusStatModifier": return new EnemyRadiusStatModifierEffect(parameters);
                    //Inverted
                case "AllyRadiusSelfCombatStatModifier": return new AllyRadiusSelfCombatStatModifierEffect(parameters);
                case "AllyRadiusSelfStatModifier": return new AllyRadiusSelfStatModifierEffect(parameters);
                case "NoAllyRadiusSelfCombatStatModifier": return new NoAllyRadiusSelfCombatStatModifierEffect(parameters);
                case "NoAllyRadiusSelfStatModifier": return new NoAllyRadiusSelfStatModifierEffect(parameters);
            }

            throw new UnmatchedSkillEffectException(effectType);
        }
    }
}
