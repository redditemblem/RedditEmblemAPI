using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.Units;
using RedditEmblemAPI.Services.Helpers;
using System.Collections.Generic;

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
            this.Tags = DataParser.List_StringCSV(parameters, 0); //Param1

            if (this.Tags.Count == 0)
                throw new RequiredValueNotProvidedException("Param1");
        }

        /// <summary>
        /// Removes the tags in <c>Tags</c> from <paramref name="unit"/>.
        /// </summary>
        public override void Apply(Unit unit, StatusCondition status)
        {
            foreach (string tag in this.Tags)
                unit.Tags.Remove(tag);
        }
    }
}
