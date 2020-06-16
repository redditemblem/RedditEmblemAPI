using System;

namespace RedditEmblemAPI.Models.Exceptions.Processing
{
    public class ConvoyItemProcessingException : ProcessingException
    {
        /// <summary>
        /// Container exception thrown when an error occurs while processing a <c>ConvoyItem</c>.
        /// </summary>
        /// <param name="convoyItemName"></param>
        /// <param name="innerException"></param>
        public ConvoyItemProcessingException(string convoyItemName, Exception innerException)
            : base("convoy item", convoyItemName, innerException)
        { }
    }
}
