using System;

namespace RedditEmblemAPI.Models.Exceptions.Validation
{
    public class NonUniqueObjectNameException : Exception
    {
        /// <summary>
        /// Thrown when a <paramref name="objectType"/> with the same name has already been added to a dictionary.
        /// </summary>
        /// <param name="objectType"></param>
        public NonUniqueObjectNameException(string objectType)
            : base($"Another {objectType} with this name already exists. Please provide a unique name.")
        { }

        /// <summary>
        /// Thrown when a <paramref name="objectType"/> with the <paramref name="name"/> has already been added to a dictionary.
        /// </summary>
        /// <param name="objectType"></param>
        public NonUniqueObjectNameException(string objectType, string name)
            : base($"Another {objectType} with the name \"{name}\" already exists. Please provide a unique name.")
        { }
    }
}
