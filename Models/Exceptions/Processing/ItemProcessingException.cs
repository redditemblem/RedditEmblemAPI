using System;

namespace RedditEmblemAPI.Models.Exceptions.Processing
{
    public class ItemProcessingException : ProcessingException
    {
        /// <summary>
        /// Container exception thrown when an error occurs while processing an <c>Item</c>.
        /// </summary>
        /// <param name="itemName"></param>
        /// <param name="innerException"></param>
        public ItemProcessingException(string itemName, Exception innerException)
            : base("item", itemName, innerException)
        { }
    }
}
