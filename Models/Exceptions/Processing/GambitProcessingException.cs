using System;

namespace RedditEmblemAPI.Models.Exceptions.Processing
{
    public class GambitProcessingException : ProcessingException
    {
        /// <summary>
        /// Container exception thrown when an error occurs while processing a <c>Gambit</c>.
        /// </summary>
        public GambitProcessingException(string gambitName, Exception innerException)
            : base("gambit", gambitName, innerException)
        { }
    }
}
