﻿using RedditEmblemAPI.Models.Exceptions.Unmatched;
using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.Map;
using RedditEmblemAPI.Models.Output.Units;
using RedditEmblemAPI.Services.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace RedditEmblemAPI.Models.Output.System.Skills.Effects.Radius
{
    public class EnemyRadiusCombatStatModifierEffect : SkillEffect
    {
        #region Attributes

        protected override string Name { get { return "EnemyRadiusCombatStatModifier"; } }
        protected override int ParameterCount { get { return 3; } }

        /// <summary>
        /// Param1. The range within this skill affects units.
        /// </summary>
        private int Radius { get; set; }

        /// <summary>
        /// Param2. The unit combat stats to be affected.
        /// </summary>
        private List<string> Stats { get; set; }

        /// <summary>
        /// Param3. The values by which to modify the <c>Stats</c>.
        /// </summary>
        private List<int> Values { get; set; }

        #endregion

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <exception cref="RequiredValueNotProvidedException"></exception>
        /// <exception cref="SkillEffectParameterLengthsMismatchedException"></exception>
        public EnemyRadiusCombatStatModifierEffect(List<string> parameters)
            : base(parameters)
        {
            this.Radius = DataParser.Int_NonZeroPositive(parameters, 0, "Param1");
            this.Stats = DataParser.List_StringCSV(parameters, 1); //Param2
            this.Values = DataParser.List_IntCSV(parameters, 2, "Param3", false);

            if (this.Stats.Count == 0)
                throw new RequiredValueNotProvidedException("Param2");
            if (this.Values.Count == 0)
                throw new RequiredValueNotProvidedException("Param3");

            if (this.Stats.Count != this.Values.Count)
                throw new SkillEffectParameterLengthsMismatchedException("Param2", "Param3");
        }

        /// <summary>
        /// Searches the <paramref name="units"/> list for hostile units within <c>Radius</c> tiles. If it finds one, adds the values in <c>Values</c> as modifiers to the items in <c>Stats</c>.
        /// </summary>
        /// <exception cref="UnmatchedStatException"></exception>
        public override void Apply(Unit unit, Skill skill, MapObj map, List<Unit> units)
        {
            //If unit is not on the map, don't apply
            if (!unit.Location.IsOnMap())
                return;

            //Apply modifiers to enemies in range
            foreach (Unit enemy in units.Where(u => u.AffiliationObj.Grouping != unit.AffiliationObj.Grouping //different affiliation grouping
                                                 && u.Location.IsOnMap()
                                                 && u.Location.OriginTiles.Any(o1 => unit.Location.OriginTiles.Any(o2 => o2.Coordinate.DistanceFrom(o1.Coordinate) <= this.Radius))))
            {
                ApplyUnitCombatStatModifiers(enemy, $"{unit.Name}'s {skill.Name}", this.Stats, this.Values);
            }
        }
    }
}
