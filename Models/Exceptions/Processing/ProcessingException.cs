using System;

namespace RedditEmblemAPI.Models.Exceptions.Processing
{
    /// <summary>
    /// Abstract. An error thrown when a data type fails to parse.
    /// </summary>
    public abstract class ProcessingException : Exception
    {
        public ProcessingException(string description, string erroringValue, Exception innerException)
            : base($"An error occurred while processing {description} \"{erroringValue}\".", innerException)
        { }
    }
}
