using System;

namespace RedditEmblemAPI.Models.Exceptions.Processing
{
    public class EmblemProcessingException : ProcessingException
    {
        /// <summary>
        /// Container exception thrown when an error occurs while processing an <c>Emblem</c>.
        /// </summary>
        public EmblemProcessingException(string emblemName, Exception innerException)
            : base("emblem", emblemName, innerException)
        { }
    }
}
