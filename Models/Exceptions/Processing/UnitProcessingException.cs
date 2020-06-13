using System;

namespace RedditEmblemAPI.Models.Exceptions.Processing
{
    public class UnitProcessingException : Exception
    {
        public UnitProcessingException(string unitName, Exception innerException)
            : base(string.Format("An error occurred while processing unit \"{0}\".", unitName), innerException)
        { }
    }
}
