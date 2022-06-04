namespace RedditEmblemAPI.Models.Exceptions.Validation
{
    public class AnyIntegerException : NumericalException
    {
        /// <summary>
        /// Thrown when an integer fails to parse and the expected result can be positive or negative.
        /// </summary>
        public AnyIntegerException(string fieldName, string actualValue)
            : base(fieldName, actualValue, "an integer")
        { }
    }
}