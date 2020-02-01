namespace RedditEmblemAPI.Models.Exceptions
{
    public class AnyIntegerException : IntegerException
    {
        public AnyIntegerException(string Value, string actualValue)
            : base(Value, actualValue, "a numerical value")
        { }
    }
}