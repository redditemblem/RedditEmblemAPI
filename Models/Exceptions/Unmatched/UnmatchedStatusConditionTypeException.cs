using System;

namespace RedditEmblemAPI.Models.Exceptions.Unmatched
{
    public class UnmatchedStatusConditionTypeException : Exception
    {
        /// <summary>
        /// Thrown when text cannot be matched to the name of an <c>StatusConditionType</c> enum.
        /// </summary>
        public UnmatchedStatusConditionTypeException(string typeName)
            : base(string.Format("Status condition type \"{0}\" not recognized. Possible values are \"Positive\", \"Negative\", and \"Neutral\".", typeName))
        { }
    }
}
