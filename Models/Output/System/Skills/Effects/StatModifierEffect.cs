﻿using RedditEmblemAPI.Models.Exceptions.Unmatched;
using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.Units;
using RedditEmblemAPI.Services.Helpers;
using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Output.System.Skills.Effects
{
    public class StatModifierEffect : ISkillEffect
    {
        #region Attributes

        /// <summary>
        /// Param1. The unit stats to be affected.
        /// </summary>
        public IList<string> Stats { get; private set; }

        /// <summary>
        /// Param2. The value by which to modify the <c>Stats</c>.
        /// </summary>
        public IList<int> Values { get; private set; }

        #endregion

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <exception cref="SkillEffectMissingParameterException"></exception>
        /// <exception cref="RequiredValueNotProvidedException"></exception>
        /// <exception cref="SkillEffectParameterLengthsMismatchedException"></exception>
        public StatModifierEffect(IList<string> parameters)
        {
            if (parameters.Count < 2)
                throw new SkillEffectMissingParameterException("StatModifier", 2, parameters.Count);

            this.Stats = ParseHelper.StringCSVParse(parameters, 0); //Param1
            this.Values = ParseHelper.IntCSVParse(parameters, 1, "Param2", false);

            if (this.Stats.Count == 0)
                throw new RequiredValueNotProvidedException("Param1");
            if (this.Values.Count == 0)
                throw new RequiredValueNotProvidedException("Param2");

            if (this.Stats.Count != this.Values.Count)
                throw new SkillEffectParameterLengthsMismatchedException("Param1", "Param2");
        }

        /// <summary>
        /// Adds the items in <c>Values</c> as modifiers to the stats in <c>Stats</c> for <paramref name="unit"/>.
        /// </summary>
        /// <exception cref="UnmatchedStatException"></exception>
        public void Apply(Unit unit, Skill skill, IList<Unit> units)
        {
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
