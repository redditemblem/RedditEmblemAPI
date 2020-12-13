using RedditEmblemAPI.Models.Exceptions.Unmatched;
using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.Units;
using RedditEmblemAPI.Services.Helpers;
using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Output.System.Skills.Effects
{
    public class TerrainTypeStatModifierEffect : ISkillEffect
    {
        #region Attributes

        /// <summary>
        /// Param1. The terrain type grouping to look for <c>Tile</c>s in.
        /// </summary>
        public int TerrainTypeGrouping { get; set; }

        /// <summary>
        /// Param2. The unit stats to be affected.
        /// </summary>
        public IList<string> Stats { get; set; }

        /// <summary>
        /// Param3. The values by which to modify the <c>Stats</c>.
        /// </summary>
        public IList<int> Values { get; set; }

        #endregion

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="parameters"></param>
        /// <exception cref="SkillEffectMissingParameterException"></exception>
        /// <exception cref="RequiredValueNotProvidedException"></exception>
        /// <exception cref="SkillEffectParameterLengthsMismatchedException"></exception>
        public TerrainTypeStatModifierEffect(IList<string> parameters)
        {
            if (parameters.Count < 3)
                throw new SkillEffectMissingParameterException("TerrainTypeStatModifier", 3, parameters.Count);

            this.TerrainTypeGrouping = ParseHelper.SafeIntParse(parameters, 0, "Param1", true);
            this.Stats = ParseHelper.StringCSVParse(parameters, 1); //Param2
            this.Values = ParseHelper.IntCSVParse(parameters, 2, "Param3", false);

            if (this.Stats.Count == 0)
                throw new RequiredValueNotProvidedException("Param2");
            if (this.Values.Count == 0)
                throw new RequiredValueNotProvidedException("Param3");

            if (this.Stats.Count != this.Values.Count)
                throw new SkillEffectParameterLengthsMismatchedException("Param2", "Param3");
        }

        /// <summary>
        /// If <paramref name="unit"/> originates on a tile with a terrain type in <c>TerrainTypeGrouping</c>, then the values in <c>Values</c> are added as modifiers to the items in <c>Stats</c>.
        /// </summary>
        /// <exception cref="UnmatchedStatException"></exception>
        public void Apply(Unit unit, Skill skill, IList<Unit> units)
        {
            //If unit is not on the map, don't apply
            if (unit.OriginTile == null)
                return;

            //The terrain type must be in the defined grouping
            if (!unit.OriginTile.TerrainTypeObj.Groupings.Contains(this.TerrainTypeGrouping))
                return;

            for (int i = 0; i < this.Stats.Count; i++)
            {
                string statName = this.Stats[i];
                int value = this.Values[i];

                ModifiedStatValue stat;
                if (!unit.Stats.TryGetValue(statName, out stat))
                    throw new UnmatchedStatException(statName);
                stat.Modifiers.Add(skill.Name, value);
            }
        }
    }
}
