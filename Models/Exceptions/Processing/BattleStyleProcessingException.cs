using System;

namespace RedditEmblemAPI.Models.Exceptions.Processing
{
    public class BattleStyleProcessingException : ProcessingException
    {
        /// <summary>
        /// Container exception thrown when an error occurs while processing a <c>BattleStyle</c>.
        /// </summary>
        public BattleStyleProcessingException(string battleStyleName, Exception innerException)
            : base("battle style", battleStyleName, innerException)
        { }
    }
}