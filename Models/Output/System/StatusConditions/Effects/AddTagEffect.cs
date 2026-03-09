using RedditEmblemAPI.Helpers;
using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.Units;
using System.Collections.Generic;
using System.Linq;

namespace RedditEmblemAPI.Models.Output.System.StatusConditions.Effects
{
    #region Interface

    /// <inheritdoc cref="AddTagEffect"/>
    public interface IAddTagEffect
    {
        /// <inheritdoc cref="AddTagEffect.Tags"/>
        IEnumerable<string> Tags { get; }
    }

    #endregion Interface

    public class AddTagEffect : StatusConditionEffect, IAddTagEffect
    {
        #region Attributes

        protected override string Name { get { return "AddTag"; } }
        protected override int ParameterCount { get { return 1; } }

        public IEnumerable<string> Tags { get; private set; }

        #endregion Attributes

        public AddTagEffect(IEnumerable<string> parameters)
            : base(parameters)
        {
            this.Tags = DataParser.List_StringCSV(parameters, INDEX_PARAM_1);

            if (!this.Tags.Any())
                throw new RequiredValueNotProvidedException(NAME_PARAM_1);
        }

        /// <summary>
        /// Adds the tags in <c>Tags</c> to <paramref name="unit"/>, if they don't already exist.
        /// </summary>
        public override void Apply(IUnit unit, IUnitStatus status, IDictionary<string, ITag> tags)
        {
            foreach (string tag in this.Tags)
            {
                if (!unit.Tags.Contains(tag))
                {
                    unit.Tags.Add(tag);

                    //If system uses tags, match it
                    if (tags.Any())
                        Tag.MatchName(tags, tag);
                }
            }

        }
    }
}
