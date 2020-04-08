namespace RedditEmblemAPI.Models.Exceptions
{
    public class PositiveIntegerException : IntegerException
    {
        public PositiveIntegerException(string fieldName, string actualValue)
            : base(fieldName, actualValue, "a non-zero, positive numerical value")
        { }
    }
}
