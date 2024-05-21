using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.System.Skills.Effects.ItemRange;

namespace UnitTests.Models.System.Skills.Effects.ItemRange
{
    [TestClass]
    public class ItemMaxRangeSetEffectTests
    {
        #region Constructor

        [TestMethod]
        public void ItemMaxRangeSetEffect_Constructor_Null()
        {
            List<string> parameters = new List<string>();

            Assert.ThrowsException<SkillEffectMissingParameterException>(() => new ItemMaxRangeSetEffect(parameters));
        }

        [TestMethod]
        public void ItemMaxRangeSetEffect_Constructor_1EmptyString()
        {
            List<string> parameters = new List<string>() { string.Empty };

            Assert.ThrowsException<SkillEffectMissingParameterException>(() => new ItemMaxRangeSetEffect(parameters));
        }

        [TestMethod]
        public void ItemMaxRangeSetEffect_Constructor_2EmptyStrings()
        {
            List<string> parameters = new List<string>() { string.Empty, string.Empty };

            Assert.ThrowsException<NonZeroPositiveIntegerException>(() => new ItemMaxRangeSetEffect(parameters));
        }

        [TestMethod]
        public void ItemMaxRangeSetEffect_Constructor_EmptyCategories()
        {
            List<string> parameters = new List<string>() { string.Empty, "1" };

            Assert.ThrowsException<RequiredValueNotProvidedException>(() => new ItemMaxRangeSetEffect(parameters));
        }

        #endregion Constructor
    }
}
