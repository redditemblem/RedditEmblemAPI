namespace RedditEmblemAPI.Models.Exceptions
{
    public class PositiveIntegerException : IntegerException
    {
        public PositiveIntegerException(string cell, string actualValue)
            : base(cell, actualValue, "a non-zero, positive number")
        { }
    }
}
