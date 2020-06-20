using System;

namespace RedditEmblemAPI.Models.Exceptions.Validation
{
    public class RequiredValueNotProvidedException : Exception
    {
        //Thrown when a cell is blank instead of containing an expected value.
        public RequiredValueNotProvidedException(string fieldName)
            : base(string.Format("The field \"{0}\" contains no value, but is required to.", fieldName))
        { }
    }
}
