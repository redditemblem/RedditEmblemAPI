using System;

namespace RedditEmblemAPI.Models.Exceptions.Processing
{
    public class UnitInventoryItemRangeFormulaProcessingException : ProcessingException
    {
        /// <summary>
        /// Container exception thrown when an error occurs while calculating a <c>UnitInventoryItem</c>'s item range formulas.
        /// </summary>
        public UnitInventoryItemRangeFormulaProcessingException(string unitName, Exception innerException)
            : base($"item range formulas on unit", unitName, innerException)
        { }
    }
}
