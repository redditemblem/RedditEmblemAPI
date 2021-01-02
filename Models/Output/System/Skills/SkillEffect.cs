using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.Map;
using RedditEmblemAPI.Models.Output.Units;
using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Output.System.Skills.Effects
{
    public abstract class SkillEffect
    {
        #region Attributes

        protected abstract string SkillEffectName { get; }
        protected abstract int ParameterCount { get; }

        #endregion

        /// <summary>
        /// Base constructor.
        /// </summary>
        /// <exception cref="SkillEffectMissingParameterException"></exception>
        public SkillEffect(IList<string> parameters)
        {
            //Make sure enough parameters were passed in
            if (parameters.Count < this.ParameterCount)
                throw new SkillEffectMissingParameterException(this.SkillEffectName, this.ParameterCount, parameters.Count);
        }

        public virtual void Apply(Unit unit, Skill skill, MapObj Map, IList<Unit> units)
        {
            //By default, the effect applies nothing
        }
    }
}
