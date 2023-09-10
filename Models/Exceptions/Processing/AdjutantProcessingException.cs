using System;

namespace RedditEmblemAPI.Models.Exceptions.Processing
{
    public class AdjutantProcessingException : ProcessingException
    {
        /// <summary>
        /// Container exception thrown when an error occurs while processing an <c>Adjutant</c>.
        /// </summary>
        public AdjutantProcessingException(string adjutantName, Exception innerException)
            : base("adjutant", adjutantName, innerException)
        { }
    }
}
