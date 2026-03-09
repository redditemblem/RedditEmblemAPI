using RedditEmblemAPI.Helpers;
using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.Units;
using System.Collections.Generic;
using System.Linq;

namespace RedditEmblemAPI.Models.Output.System.StatusConditions.Effects
{
    #region Interface

    /// <inheritdoc cref="RemoveTagEffect"/>
    public interface IRemoveTagEffect
    {
        /// <inheritdoc cref="RemoveTagEffect.Tags"/>
        IEnumerable<string> Tags { get; }
    }

    #endregion Interface

    public class RemoveTagEffect : StatusConditionEffect, IRemoveTagEffect
    {
        #region Attributes

        protected override string Name { get { return "RemoveTag"; } }
        protected override int ParameterCount { get { return 1; } }

        public IEnumerable<string> Tags { get; private set; }

        #endregion Attributes

        public RemoveTagEffect(IEnumerable<string> parameters)
            : base(parameters)
        {
            this.Tags = DataParser.List_StringCSV(parameters, INDEX_PARAM_1);

            if (!this.Tags.Any())
                throw new RequiredValueNotProvidedException(NAME_PARAM_1);
        }

        /// <summary>
        /// Removes the tags in <c>Tags</c> from <paramref name="unit"/>.
        /// </summary>
        public override void Apply(IUnit unit, IUnitStatus status, IDictionary<string, ITag> tags)
        {
            foreach (string tag in this.Tags)
                unit.Tags.Remove(tag);
        }
    }
}
