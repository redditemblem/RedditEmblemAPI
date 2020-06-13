using System;

namespace RedditEmblemAPI.Models.Exceptions.Processing
{
    public class ConvoyItemProcessingException : Exception
    {
        public ConvoyItemProcessingException(string itemName, Exception innerException)
            : base(string.Format("An error occurred while processing convoy item \"{0}\".", itemName), innerException)
        { }
    }
}
