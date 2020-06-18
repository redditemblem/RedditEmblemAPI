using RedditEmblemAPI.Models.Exceptions.Unmatched;
using RedditEmblemAPI.Models.Output.Units;
using RedditEmblemAPI.Services.Helpers;
using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Output.System.Skills.Effects
{
    public class AllyRadiusCombatStatModiferEffect : ISkillEffect
    {
        #region Attributes

        /// <summary>
        /// Param1. The range within this skill affects units.
        /// </summary>
        public int Radius { get; set; }

        /// <summary>
        /// Param2. The unit combat stat to be affected.
        /// </summary>
        public string Stat { get; set; }

        /// <summary>
        /// Param3. The value by which to modify the <c>Stat</c>.
        /// </summary>
        public int Value { get; set; }

        #endregion

        public AllyRadiusCombatStatModiferEffect(string param1, string param2, string param3)
        {
            this.Radius = ParseHelper.SafeIntParse(param1, "Param1", true);
            this.Stat = param2;
            this.Value = ParseHelper.SafeIntParse(param3, "Param3", false);
        }

        public void Apply(Unit unit, Skill skill, IList<Unit> units)
        {
            foreach(Unit other in units)
            {
                if (unit.Name == other.Name)
                    continue;

                //Units must be in the same grouping
                if (unit.AffiliationObj.Grouping != other.AffiliationObj.Grouping)
                    continue;

                //Units must be within range
                if (this.Radius < unit.Coordinate.DistanceFrom(other.Coordinate))
                    continue;

                ModifiedStatValue stat;
                if (!other.CombatStats.TryGetValue(this.Stat, out stat))
                    throw new UnmatchedStatException(this.Stat);
                stat.Modifiers.Add(unit.Name + "'s " + skill.Name, this.Value);
            }
        }
    }
}
