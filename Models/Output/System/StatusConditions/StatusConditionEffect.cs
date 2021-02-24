using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.Units;
using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Output.System.StatusConditions
{
    public abstract class StatusConditionEffect
    {
        #region Attributes

        protected abstract string Name { get; }
        protected abstract int ParameterCount { get; }

        #endregion

        /// <summary>
        /// Base constructor.
        /// </summary>
        /// <exception cref="SkillEffectMissingParameterException"></exception>
        public StatusConditionEffect(IList<string> parameters)
        {
            //Make sure enough parameters were passed in
            if (parameters.Count < this.ParameterCount)
                throw new StatusConditionEffectMissingParameterException(this.Name, this.ParameterCount, parameters.Count);
        }

        public virtual void Apply(Unit unit)
        {
            //By default, the effect applies nothing
        }
    }
}
