using System;

namespace RedditEmblemAPI.Models.Exceptions.Processing
{
    public class UnitProcessingException : ProcessingException
    {
        /// <summary>
        /// Container exception thrown when an error occurs while processing a <c>Unit</c>.
        /// </summary>
        /// <param name="unitName"></param>
        /// <param name="innerException"></param>
        public UnitProcessingException(string unitName, Exception innerException)
            : base("unit", unitName, innerException)
        { }
    }
}
