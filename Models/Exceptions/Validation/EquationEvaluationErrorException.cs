using System;

namespace RedditEmblemAPI.Models.Exceptions.Validation
{
    public class EquationEvaluationErrorException : Exception
    {
        /// <summary>
        /// Thrown when a <c>CalculatedStatConfig</c> equation cannot be evaluated.
        /// </summary>
        /// <param name="equation"></param>
        public EquationEvaluationErrorException(string equation)
            : base($"Could not evaluate the following mathematical equation: \"{equation}\". Are there invalid characters?")
        { }
    }
}
