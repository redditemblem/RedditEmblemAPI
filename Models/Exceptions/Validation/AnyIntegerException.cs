namespace RedditEmblemAPI.Models.Exceptions.Validation
{
    public class AnyIntegerException : IntegerException
    {
        /// <summary>
        /// Thrown when a numerical value fails to parse and the expected result can be positive or negative.
        /// </summary>
        public AnyIntegerException(string fieldName, string actualValue)
            : base(fieldName, actualValue, "a numerical value")
        { }
    }
}