namespace RedditEmblemAPI.Models.Exceptions.Validation
{
    public class NegativeIntegerException : IntegerException
    {
        /// <summary>
        /// Thrown when a numerical value fails to parse and the expected result is a number below 0.
        /// </summary>
        public NegativeIntegerException(string fieldName, string actualValue)
            : base(fieldName, actualValue, "a negative numerical value")
        { }
    }
}
