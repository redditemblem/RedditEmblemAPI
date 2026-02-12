using RedditEmblemAPI.Models.Exceptions.Unmatched;
using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.System.Skills.Effects.ItemRange;

namespace UnitTests.Models.System.Skills.Effects.ItemRange
{
    public class ItemMaxRangeModifierEffectTests
    {
        #region Constructor

        [Test]
        public void Constructor_Null()
        {
            List<string> parameters = new List<string>();

            Assert.Throws<SkillEffectMissingParameterException>(() => new ItemMaxRangeModifierEffect(parameters));
        }

        [Test]
        public void Constructor_1EmptyString()
        {
            List<string> parameters = new List<string>() { string.Empty };

            Assert.Throws<SkillEffectMissingParameterException>(() => new ItemMaxRangeModifierEffect(parameters));
        }

        [Test]
        public void Constructor_2EmptyStrings()
        {
            List<string> parameters = new List<string>() { string.Empty, string.Empty };

            Assert.Throws<SkillEffectMissingParameterException>(() => new ItemMaxRangeModifierEffect(parameters));
        }

        [Test]
        public void Constructor_3EmptyStrings()
        {
            List<string> parameters = new List<string>() { string.Empty, string.Empty, string.Empty };

            Assert.Throws<NonZeroPositiveIntegerException>(() => new ItemMaxRangeModifierEffect(parameters));
        }

        [Test]
        public void Constructor_EmptyCategory()
        {
            List<string> parameters = new List<string>() { string.Empty, "1", string.Empty };

            Assert.Throws<RequiredValueNotProvidedException>(() => new ItemMaxRangeModifierEffect(parameters));
        }

        [Test]
        public void Constructor_EmptyCategory_WithDealsDamageFilterType()
        {
            List<string> parameters = new List<string>() { string.Empty, "1", "All" };

            Assert.Throws<RequiredValueNotProvidedException>(() => new ItemMaxRangeModifierEffect(parameters));
        }

        [Test]
        public void Constructor_DealsDamageFilterType_UnknownValue()
        {
            List<string> parameters = new List<string>() { "Category", "1", "Mismatch" };

            Assert.Throws<UnmatchedDealsDamageFilterTypeException>(() => new ItemMaxRangeModifierEffect(parameters));
        }

        #endregion Constructor
    }
}
