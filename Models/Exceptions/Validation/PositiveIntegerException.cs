namespace RedditEmblemAPI.Models.Exceptions.Validation
{
    public class PositiveIntegerException : IntegerException
    {
        /// <summary>
        /// Thrown when a numerical value fails to parse and the expected result is a positive number or zero.
        /// </summary>
        public PositiveIntegerException(string fieldName, string actualValue)
            : base(fieldName, actualValue, "a positive integer")
        { }
    }
}
