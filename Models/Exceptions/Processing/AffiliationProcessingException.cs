using System;

namespace RedditEmblemAPI.Models.Exceptions.Processing
{
    public class AffiliationProcessingException : ProcessingException
    {
        /// <summary>
        /// Container exception thrown when an error occurs while processing an <c>Affiliation</c>.
        /// </summary>
        /// <param name="affiliationName"></param>
        /// <param name="innerException"></param>
        public AffiliationProcessingException(string affiliationName, Exception innerException)
            : base("affiliation", affiliationName, innerException)
        { }
    }
}
