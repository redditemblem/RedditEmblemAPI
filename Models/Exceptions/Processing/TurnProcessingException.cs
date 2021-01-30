using System;

namespace RedditEmblemAPI.Models.Exceptions.Processing
{
    public class TurnProcessingException : Exception
    {
        /// <summary>
        /// Container exception thrown when an error occurs while processing a <c>Turn</c>.
        /// </summary>
        /// <param name="turnNumber"></param>
        /// <param name="innerException"></param>
        public TurnProcessingException(string turnNumber, Exception innerException)
            : base(string.Format("An error occurred while processing turn #{0}.", turnNumber), innerException)
        { }
    }
}
