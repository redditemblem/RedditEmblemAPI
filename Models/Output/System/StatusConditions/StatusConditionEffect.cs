﻿using RedditEmblemAPI.Models.Exceptions.Unmatched;
using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.Units;
using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Output.System.StatusConditions
{
    public abstract class StatusConditionEffect
    {
        #region Constants

        protected const int INDEX_PARAM_1 = 0;
        protected const int INDEX_PARAM_2 = 1;
        protected const int INDEX_PARAM_3 = 2;

        protected const string NAME_PARAM_1 = "Param1";
        protected const string NAME_PARAM_2 = "Param2";
        protected const string NAME_PARAM_3 = "Param3";

        #endregion Constants

        #region Attributes

        protected abstract string Name { get; }
        protected abstract int ParameterCount { get; }

        #endregion

        /// <summary>
        /// Base constructor.
        /// </summary>
        /// <exception cref="SkillEffectMissingParameterException"></exception>
        public StatusConditionEffect(List<string> parameters)
        {
            //Make sure enough parameters were passed in
            if (parameters.Count < this.ParameterCount)
                throw new StatusConditionEffectMissingParameterException(this.Name, this.ParameterCount, parameters.Count);
        }

        public virtual void Apply(Unit unit, StatusCondition status)
        {
            //By default, the effect applies nothing
        }
    }
}
