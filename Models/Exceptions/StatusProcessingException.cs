using System;

namespace RedditEmblemAPI.Models.Exceptions
{
    public class StatusProcessingException : Exception
    {
        public StatusProcessingException(string statusName, Exception innerException)
            : base(string.Format("An error occurred while processing status condition \"{0}\".", statusName), innerException)
        { }
    }
}
