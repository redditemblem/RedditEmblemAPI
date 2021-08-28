using System;

namespace RedditEmblemAPI.Models.Exceptions.Validation
{
    public class UnrecognizedEquationVariableException : Exception
    {
        /// <summary>
        /// Thrown when a <c>CalculatedStatConfig</c> equation contains an unrecognized variable.
        /// </summary>
        /// <param name="equation"></param>
        public UnrecognizedEquationVariableException(string equation)
            : base($"The equation \"{equation}\" contains an unrecognized variable.")
        { }
    }
}
