using System;

namespace RedditEmblemAPI.Models.Exceptions.Processing
{
    public class UnitCombatStatFormulaProcessingException : ProcessingException
    {
        /// <summary>
        /// Container exception thrown when an error occurs while calculating a <c>Unit</c>'s combat stat formulas.
        /// </summary>
        /// <param name="unitName"></param>
        /// <param name="innerException"></param>
        public UnitCombatStatFormulaProcessingException(string unitName, Exception innerException)
            : base("combat stat formulas on unit", unitName, innerException)
        { }
    }
}
