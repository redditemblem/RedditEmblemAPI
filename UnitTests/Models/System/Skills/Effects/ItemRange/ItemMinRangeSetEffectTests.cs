using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.System.Skills.Effects.ItemRange;

namespace UnitTests.Models.System.Skills.Effects.ItemRange
{
    public class ItemMinRangeSetEffectTests
    {
        #region Constructor

        [Test]
        public void Constructor_Null()
        {
            List<string> parameters = new List<string>();

            Assert.Throws<SkillEffectMissingParameterException>(() => new ItemMinRangeSetEffect(parameters));
        }

        [Test]
        public void Constructor_1EmptyString()
        {
            List<string> parameters = new List<string>() { string.Empty };

            Assert.Throws<SkillEffectMissingParameterException>(() => new ItemMinRangeSetEffect(parameters));
        }

        [Test]
        public void Constructor_2EmptyStrings()
        {
            List<string> parameters = new List<string>() { string.Empty, string.Empty };

            Assert.Throws<NonZeroPositiveIntegerException>(() => new ItemMinRangeSetEffect(parameters));
        }

        [Test]
        public void Constructor_Value_0()
        {
            List<string> parameters = new List<string>() { string.Empty, "0" };

            Assert.Throws<NonZeroPositiveIntegerException>(() => new ItemMinRangeSetEffect(parameters));
        }

        [Test]
        public void Constructor_EmptyCategories()
        {
            List<string> parameters = new List<string>() { string.Empty, "1" };

            Assert.Throws<RequiredValueNotProvidedException>(() => new ItemMinRangeSetEffect(parameters));
        }

        #endregion Constructor
    }
}
