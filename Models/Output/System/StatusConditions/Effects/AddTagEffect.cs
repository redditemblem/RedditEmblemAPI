using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.Units;
using RedditEmblemAPI.Services.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace RedditEmblemAPI.Models.Output.System.StatusConditions.Effects
{
    public class AddTagEffect : StatusConditionEffect
    {
        #region Attributes

        protected override string Name { get { return "AddTag"; } }
        protected override int ParameterCount { get { return 1; } }

        private List<string> Tags { get; set; }

        #endregion

        public AddTagEffect(List<string> parameters)
            : base(parameters)
        {
            this.Tags = DataParser.List_StringCSV(parameters, INDEX_PARAM_1);

            if (!this.Tags.Any())
                throw new RequiredValueNotProvidedException(NAME_PARAM_1);
        }

        /// <summary>
        /// Adds the tags in <c>Tags</c> to <paramref name="unit"/>, if they don't already exist.
        /// </summary>
        public override void Apply(Unit unit, UnitStatus status, IDictionary<string, ITag> tags)
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
