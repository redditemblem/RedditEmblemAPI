using Newtonsoft.Json;

namespace RedditEmblemAPI.Models.Output.System.Interfaces
{
    #region Interface

    /// <summary>
    /// Requires class to include a <c>Matched</c> flag and identifying name.
    /// </summary>
    public interface IMatchable
    {
        /// <inheritdoc cref="Matchable.Matched"/>
        bool Matched { get; }

        /// <inheritdoc cref="Matchable.Name"/>
        string Name { get; }

        /// <inheritdoc cref="Matchable.FlagAsMatched"/>
        void FlagAsMatched();
    }

    #endregion Interface

    /// <summary>
    /// Implements a <c>Matched</c> flag and identifying name.
    /// </summary>
    public abstract class Matchable : IMatchable
    {
        /// <summary>
        /// Flag indicating that the object is being actively by used by something in the current load. Used to minify the output JSON.
        /// </summary>
        [JsonIgnore]
        public bool Matched { get; protected set; }

        /// <summary>
        /// The object's name. Should be unique.
        /// </summary>
        public string Name { get; protected set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        public Matchable()
        {
            this.Matched = false;
        }

        /// <summary>
        /// Sets <c>this.Matched</c> equal to true. If the inheriting class has additional <c>IMatchable</c> child attributes, they must override the implementation.
        /// </summary>
        public virtual void FlagAsMatched()
        {
            this.Matched = true;
        }
    }
}
