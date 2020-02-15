using System;

namespace RedditEmblemAPI.Models.Exceptions
{
    public class ClassProcessingException : Exception
    {
        public ClassProcessingException(string className, Exception innerException)
            : base(string.Format("An error occurred while processing class \"{0}\".", className), innerException)
        { }
    }
}
