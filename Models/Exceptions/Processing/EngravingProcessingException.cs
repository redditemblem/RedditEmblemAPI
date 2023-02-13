using System;

namespace RedditEmblemAPI.Models.Exceptions.Processing
{
    public class EngravingProcessingException : ProcessingException
    {
        /// <summary>
        /// Container exception thrown when an error occurs while processing an <c>Engraving</c>.
        /// </summary>
        public EngravingProcessingException(string engravingName, Exception innerException)
            : base("engraving", engravingName, innerException)
        { }
    }
}
