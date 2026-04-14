using System;

namespace RedditEmblemAPI.Models.Exceptions.Validation
{
    public class InvalidIntTupleConverterInputException : Exception
    {
        public InvalidIntTupleConverterInputException()
            : base($"Value cannot be converted into a (int, int) tuple by the IntTupleConverter")
        { }
    }
}
