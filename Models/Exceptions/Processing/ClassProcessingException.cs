using System;

namespace RedditEmblemAPI.Models.Exceptions.Processing
{
    public class ClassProcessingException : ProcessingException
    {
        /// <summary>
        /// Container exception thrown when an error occurs while processing a <c>Class</c>.
        /// </summary>
        /// <param name="className"></param>
        /// <param name="innerException"></param>
        public ClassProcessingException(string className, Exception innerException)
            : base("class", className, innerException)
        { }
    }
}
