namespace RedditEmblemAPI.Models.Exceptions
{
    public class PositiveIntegerException : IntegerException
    {
        public PositiveIntegerException(string Value, string actualValue)
            : base(Value, actualValue, "a non-zero, positive number")
        { }
    }
}
