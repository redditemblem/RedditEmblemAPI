namespace RedditEmblemAPI.Models.Exceptions.Validation
{
    public class PositiveDecimalException : NumericalException
    {
        /// <summary>
        /// Thrown when a numerical value fails to parse and the expected result is a positive number or zero.
        /// </summary>
        public PositiveDecimalException(string fieldName, string actualValue)
            : base(fieldName, actualValue, "a positive decimal")
        { }
    }
}
