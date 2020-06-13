using System;

namespace RedditEmblemAPI.Models.Exceptions.Processing
{
    public class ItemProcessingException : Exception
    {
        public ItemProcessingException(string itemName, Exception innerException)
            : base(string.Format("An error occurred while processing item \"{0}\".", itemName), innerException)
        { }
    }
}
