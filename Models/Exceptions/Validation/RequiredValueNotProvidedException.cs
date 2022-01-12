using System;

namespace RedditEmblemAPI.Models.Exceptions.Validation
{
    public class RequiredValueNotProvidedException : Exception
    {
        /// <summary>
        /// Thrown when a cell is blank instead of containing an expected value.
        /// </summary>
        /// <param name="fieldName"></param>
        public RequiredValueNotProvidedException(string fieldName)
            : base($"The field \"{fieldName}\" contains no value, but is required to.")
        { }
    }
}
