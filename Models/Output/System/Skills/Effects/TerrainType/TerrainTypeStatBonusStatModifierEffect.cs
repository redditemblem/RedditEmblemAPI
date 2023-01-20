using RedditEmblemAPI.Models.Exceptions.Unmatched;
using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.Map;
using RedditEmblemAPI.Models.Output.Map.Tiles;
using RedditEmblemAPI.Models.Output.Units;
using RedditEmblemAPI.Services.Helpers;
using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Output.System.Skills.Effects.TerrainType
{
    public class TerrainTypeStatBonusStatModifierEffect : SkillEffect
    {
        #region Attributes

        protected override string Name { get { return "TerrainTypeStatBonusStatModifier"; } }
        protected override int ParameterCount { get { return 3; } }

        /// <summary>
        /// Param1. The terrain type stat modifier to evaluate. 
        /// </summary>
        private string TerrainTypeStat { get; set; }

        /// <summary>
        /// Param2. The unit stats to be affected.
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
        public TerrainTypeStatBonusStatModifierEffect(List<string> parameters)
            : base(parameters)
        {
            this.TerrainTypeStat = DataParser.String(parameters, 0, "Param1");
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
        /// When <paramref name="unit"/> is standing on terrain that grants a positive modifier to <c>this.TerrainTypeStat</c>, then the values in <c>Values</c> are added as modifiers to the items in <c>Stats</c>.
        /// </summary>
        public override void Apply(Unit unit, Skill skill, MapObj map, List<Unit> units)
        {
            //If unit is not on the map, don't apply
            if (!unit.Location.IsOnMap())
                return;

            foreach(Tile tile in unit.Location.OriginTiles)
            {
                int modifier;
                if (!tile.TerrainTypeObj.StatModifiers.TryGetValue(this.TerrainTypeStat, out modifier))
                    continue;

                //Modifier must be positive
                if (modifier <= 0)
                    continue;

                ApplyUnitStatModifiers(unit, skill.Name, this.Stats, this.Values);
                break;
            }
            
        }
    }
}
