using System;

namespace RedditEmblemAPI.Models.Exceptions.Processing
{
    public abstract class ProcessingException : Exception
    {
        public ProcessingException(string description, string erroringValue, Exception innerException)
            : base(string.Format("An error occurred while processing {0} \"{1}\".", description, erroringValue), innerException)
        { }
    }
}
