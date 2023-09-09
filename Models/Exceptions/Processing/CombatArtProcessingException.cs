using System;

namespace RedditEmblemAPI.Models.Exceptions.Processing
{
    public class CombatArtProcessingException : ProcessingException
    {
        /// <summary>
        /// Container exception thrown when an error occurs while processing a <c>CombatArt</c>.
        /// </summary>
        public CombatArtProcessingException(string combatArtName, Exception innerException)
            : base("combat art", combatArtName, innerException)
        { }
    }
}
