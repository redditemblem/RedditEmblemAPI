namespace RedditEmblemAPI.Models.Exceptions.Validation
{
    public class AnyDecimalException : NumericalException
    {
        /// <summary>
        /// Thrown when a decimal fails to parse and the expected result can be positive or negative.
        /// </summary>
        public AnyDecimalException(string fieldName, string actualValue)
            : base(fieldName, actualValue, "a decimal")
        { }
    }
}