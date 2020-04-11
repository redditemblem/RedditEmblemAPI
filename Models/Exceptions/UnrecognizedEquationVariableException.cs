using System;

namespace RedditEmblemAPI.Models.Exceptions
{
    public class UnrecognizedEquationVariableException : Exception
    {
        public UnrecognizedEquationVariableException(string equation)
            : base(string.Format("The equation \"{0}\" contains an unrecognized variable", equation))
        { }
    }
}
