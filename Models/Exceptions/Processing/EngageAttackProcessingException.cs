using System;

namespace RedditEmblemAPI.Models.Exceptions.Processing
{
    public class EngageAttackProcessingException : ProcessingException
    {
        /// <summary>
        /// Container exception thrown when an error occurs while processing an <c>EngageAttack</c>.
        /// </summary>
        public EngageAttackProcessingException(string engageAttackName, Exception innerException)
            : base("engage attack", engageAttackName, innerException)
        { }
    }
}
