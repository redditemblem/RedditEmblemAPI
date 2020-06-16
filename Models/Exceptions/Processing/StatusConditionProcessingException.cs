using System;

namespace RedditEmblemAPI.Models.Exceptions.Processing
{
    public class StatusConditionProcessingException : ProcessingException
    {
        /// <summary>
        /// Container exception thrown when an error occurs while processing a <c>StatusCondition</c>.
        /// </summary>
        /// <param name="statusName"></param>
        /// <param name="innerException"></param>
        public StatusConditionProcessingException(string statusName, Exception innerException)
            : base("status condition", statusName, innerException)
        { }
    }
}
