using System;

namespace RedditEmblemAPI.Models.Exceptions.Processing
{
    public class BattalionProcessingException : ProcessingException
    {
        /// <summary>
        /// Container exception thrown when an error occurs while processing a <c>Battalion</c>.
        /// </summary>
        public BattalionProcessingException(string battalionName, Exception innerException)
            : base("battalion", battalionName, innerException)
        { }
    }
}
