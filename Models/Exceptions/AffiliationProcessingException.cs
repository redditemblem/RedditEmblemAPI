using System;

namespace RedditEmblemAPI.Models.Exceptions
{
    public class AffiliationProcessingException : Exception
    {
        public AffiliationProcessingException(string affiliationName, Exception innerException)
            : base(string.Format("An error occurred while processing affiliation \"{0}\".", affiliationName), innerException)
        { }
    }
}
