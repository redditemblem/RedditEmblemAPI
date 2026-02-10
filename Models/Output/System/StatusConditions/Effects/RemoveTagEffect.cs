using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.Units;
using RedditEmblemAPI.Services.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace RedditEmblemAPI.Models.Output.System.StatusConditions.Effects
{
    public class RemoveTagEffect : StatusConditionEffect
    {
        #region Attributes

        protected override string Name { get { return "RemoveTag"; } }
        protected override int ParameterCount { get { return 1; } }

        private List<string> Tags { get; set; }

        #endregion

        public RemoveTagEffect(List<string> parameters)
            : base(parameters)
        {
            this.Tags = DataParser.List_StringCSV(parameters, INDEX_PARAM_1);

            if (!this.Tags.Any())
                throw new RequiredValueNotProvidedException(NAME_PARAM_1);
        }

        /// <summary>
        /// Removes the tags in <c>Tags</c> from <paramref name="unit"/>.
        /// </summary>
        public override void Apply(Unit unit, UnitStatus status, IDictionary<string, ITag> tags)
        {
            foreach (string tag in this.Tags)
                unit.Tags.Remove(tag);
        }
    }
}
