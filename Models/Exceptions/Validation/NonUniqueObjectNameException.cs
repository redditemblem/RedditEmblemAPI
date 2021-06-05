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
            : base(string.Format("Another {0} with this name already exists. Please provide a unique name.", objectType))
        { }

        /// <summary>
        /// Thrown when a <paramref name="objectType"/> with the <paramref name="name"/> has already been added to a dictionary.
        /// </summary>
        /// <param name="objectType"></param>
        public NonUniqueObjectNameException(string objectType, string name)
            : base(string.Format("Another {0} with the name \"{1}\" already exists. Please provide a unique name.", objectType, name))
        { }
    }
}
