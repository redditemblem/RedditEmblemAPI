using Newtonsoft.Json;
using RedditEmblemAPI.Models.Configuration.System.Skills;
using RedditEmblemAPI.Models.Exceptions.Processing;
using RedditEmblemAPI.Models.Exceptions.Unmatched;
using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.System.Interfaces;
using RedditEmblemAPI.Models.Output.System.Skills.Effects;
using RedditEmblemAPI.Models.Output.System.Skills.Effects.EquippedItem;
using RedditEmblemAPI.Models.Output.System.Skills.Effects.ItemRange;
using RedditEmblemAPI.Models.Output.System.Skills.Effects.MovementRange;
using RedditEmblemAPI.Models.Output.System.Skills.Effects.PairUp;
using RedditEmblemAPI.Models.Output.System.Skills.Effects.Radius;
using RedditEmblemAPI.Models.Output.System.Skills.Effects.TerrainType;
using RedditEmblemAPI.Models.Output.System.Skills.Effects.UnitStats;
using RedditEmblemAPI.Services.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RedditEmblemAPI.Models.Output.System.Skills
{
    /// <summary>
    /// Object representing a Skill definition in the team's system. 
    /// </summary>
    public class Skill : IMatchable
    {
        #region Attributes

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
        public List<string> TextFields { get; set; }

        /// <summary>
        /// The effect the skill applies, if any.
        /// </summary>
        [JsonIgnore]
        public List<SkillEffect> Effects { get; set; }

        #region JSON Serialization Only

        /// <summary>
        /// Flag indicating whether or not a skill effect is configured on this skill.
        /// </summary>
        [JsonProperty]
        private bool IsEffectConfigured { get { return this.Effects.Any(); } }

        #endregion JSON Serialization Only

        #endregion

        /// <summary>
        /// Constructor.
        /// </summary>
        public Skill(SkillsConfig config, IEnumerable<string> data)
        {
            this.Matched = false;
            this.Name = DataParser.String(data, config.Name, "Name");
            this.SpriteURL = DataParser.OptionalString_URL(data, config.SpriteURL, "Sprite URL");
            this.TextFields = DataParser.List_Strings(data, config.TextFields);

            this.Effects = new List<SkillEffect>();
            foreach (SkillEffectConfig effect in config.Effects)
            {
                string effectType = DataParser.OptionalString(data, effect.Type, "Skill Effect Type");
                List<string> effectParms = DataParser.List_Strings(data, effect.Parameters, true);

                if (!string.IsNullOrEmpty(effectType))
                    this.Effects.Add(BuildSkillEffect(effectType, effectParms));
            }
        }

        private SkillEffect BuildSkillEffect(string effectType, List<string> parameters)
        {
            switch (effectType)
            {
                //Unit Stat Effects
                case "CombatStatModifier": return new CombatStatModifierEffect(parameters);
                case "HPBelowCombatStatModifier": return new HPBelowCombatStatModifierEffect(parameters);
                case "HPAboveCombatStatModifier": return new HPAboveCombatStatModifierEffect(parameters);
                case "StatModifier": return new StatModifierEffect(parameters);
                case "HPBelowStatModifier": return new HPBelowStatModifierEffect(parameters);
                case "HPAboveStatModifier": return new HPAboveStatModifierEffect(parameters);
                case "HPDifferenceCombatStatModifier": return new HPDifferenceCombatStatModifierEffect(parameters);
                case "HPDifferenceStatModifier": return new HPDifferenceStatModifierEffect(parameters);
                case "ReplaceCombatStatFormulaVariable": return new ReplaceCombatStatFormulaVariableEffect(parameters);
                //Equipped Item Effects
                case "EquippedCategoryCombatStatModifier": return new EquippedCategoryCombatStatModifierEffect(parameters);
                case "EquippedCategoryStatModifier": return new EquippedCategoryStatModifierEffect(parameters);
                //Item Effects
                case "ItemMaxUsesMultiplier": return new ItemMaxUsesMultiplierEffect(parameters);
                //Terrain Type Effects
                case "TerrainTypeCombatStatModifier": return new TerrainTypeCombatStatModifierEffect(parameters);
                case "TerrainTypeStatModifier": return new TerrainTypeStatModifierEffect(parameters);
                case "TerrainTypeCombatStatBonusCombatStatModifier": return new TerrainTypeCombatStatBonusCombatStatModifierEffect(parameters);
                case "TerrainTypeStatBonusCombatStatModifier": return new TerrainTypeStatBonusCombatStatModifierEffect(parameters);
                case "TerrainTypeCombatStatBonusStatModifier": return new TerrainTypeCombatStatBonusStatModifierEffect(parameters);
                case "TerrainTypeStatBonusStatModifier": return new TerrainTypeStatBonusStatModifierEffect(parameters);
                //Unit Movement Range Effects
                //Movement Costs
                case "OverrideMovementType": return new OverrideMovementTypeEffect_Skill(parameters);
                case "TerrainTypeMovementCostModifier": return new TerrainTypeMovementCostModifierEffect(parameters);
                case "TerrainTypeMovementCostSet": return new TerrainTypeMovementCostSetEffect_Skill(parameters);
                case "WarpMovementCostModifier": return new WarpMovementCostModifierEffect(parameters);
                case "WarpMovementCostSet": return new WarpMovementCostSetEffect(parameters);
                case "RadiusAllyMovementCostSet": return new RadiusAllyMovementCostSetEffect(parameters);
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
                case "ObstructItemRanges": return new ObstructItemRangesEffect(parameters);
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
                //Pair Up Effects
                case "InPairUpFrontCombatStatModifier": return new InPairUpFrontCombatStatModifierEffect(parameters);
                case "InPairUpFrontStatModifier": return new InPairUpFrontStatModifierEffect(parameters);
                case "InPairUpBackAllyCombatStatModifier": return new InPairUpBackAllyCombatStatModifierEffect(parameters);
                case "InPairUpBackAllyStatModifier": return new InPairUpBackAllyStatModifierEffect(parameters);
            }

            throw new UnmatchedSkillEffectException(effectType);
        }

        #region Static Functions

        /// <summary>
        /// Iterates through the data in <paramref name="config"/>'s <c>Query</c> and builds a <c>Skill</c> from each valid row.
        /// </summary>
        /// <exception cref="SkillProcessingException"></exception>
        public static IDictionary<string, Skill> BuildDictionary(SkillsConfig config)
        {
            IDictionary<string, Skill> skills = new Dictionary<string, Skill>();
            if (config == null || config.Query == null)
                return skills;

            foreach (List<object> row in config.Query.Data)
            {
                string name = string.Empty;
                try
                {
                    IEnumerable<string> skill = row.Select(r => r.ToString());
                    name = DataParser.OptionalString(skill, config.Name, "Name");
                    if (string.IsNullOrEmpty(name)) continue;

                    if (!skills.TryAdd(name, new Skill(config, skill)))
                        throw new NonUniqueObjectNameException("skill");
                }
                catch (Exception ex)
                {
                    throw new SkillProcessingException(name, ex);
                }
            }

            return skills;
        }

        /// <summary>
        /// Matches each of the strings in <paramref name="names"/> to a <c>Skill</c> in <paramref name="skills"/> and returns the matches as a list.
        /// </summary>
        /// <param name="skipMatchedStatusSet">If true, will not set the <c>Matched</c> flag on the returned objects to true.</param>
        public static List<Skill> MatchNames(IDictionary<string, Skill> skills, IEnumerable<string> names, bool skipMatchedStatusSet = false)
        {
            return names.Select(n => MatchName(skills, n, skipMatchedStatusSet)).ToList();
        }

        /// <summary>
        /// Matches <paramref name="name"/> to a <c>Skill</c> in <paramref name="skills"/> and returns it.
        /// </summary>
        /// <param name="skipMatchedStatusSet">If true, will not set the <c>Matched</c> flag on the returned object to true.</param>
        /// <exception cref="UnmatchedSkillException"></exception>
        public static Skill MatchName(IDictionary<string, Skill> skills, string name, bool skipMatchedStatusSet = false)
        {
            Skill match;
            if (!skills.TryGetValue(name, out match))
                throw new UnmatchedSkillException(name);

            if (!skipMatchedStatusSet) match.Matched = true;

            return match;
        }

        #endregion
    }
}
