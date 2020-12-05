using System;

namespace RedditEmblemAPI.Models.Exceptions.Processing
{
    public class TagProcessingException : ProcessingException
    {
        /// <summary>
        /// Container exception thrown when an error occurs while processing a <c>Tag</c>.
        /// </summary>
        /// <param name="tagName"></param>
        /// <param name="innerException"></param>
        public TagProcessingException(string tagName, Exception innerException)
            : base("tag", tagName, innerException)
        { }
    }
}
