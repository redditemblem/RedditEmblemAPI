using System;

namespace RedditEmblemAPI.Models.Exceptions
{
    public class TerrainTypeProcessingException : Exception
    {
        public TerrainTypeProcessingException(string typeName, Exception innerException)
            : base(string.Format("An error occurred while processing terrain type \"{0}\".", typeName), innerException)
        { }
    }
}
