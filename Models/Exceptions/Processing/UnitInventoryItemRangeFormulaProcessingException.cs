using System;

namespace RedditEmblemAPI.Models.Exceptions.Processing
{
    public class UnitInventoryItemRangeFormulaProcessingException : ProcessingException
    {
        /// <summary>
        /// Container exception thrown when an error occurs while calculating a <c>UnitInventoryItem</c>'s item range formulas.
        /// </summary>
        public UnitInventoryItemRangeFormulaProcessingException(string unitName, string itemName, Exception innerException)
            : base($"item range formulas on unit {unitName}'s inventory item", itemName, innerException)
        { }
    }
}
