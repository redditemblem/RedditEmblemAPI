using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.System.Skills.Effects.ItemRange;

namespace UnitTests.Models.System.Skills.Effects.ItemRange
{
    [TestClass]
    public class ItemMinRangeSetEffectTests
    {
        #region Constructor

        [TestMethod]
        public void ItemMinRangeSetEffect_Constructor_Null()
        {
            List<string> parameters = new List<string>();

            Assert.ThrowsException<SkillEffectMissingParameterException>(() => new ItemMinRangeSetEffect(parameters));
        }

        [TestMethod]
        public void ItemMinRangeSetEffect_Constructor_1EmptyString()
        {
            List<string> parameters = new List<string>() { string.Empty };

            Assert.ThrowsException<SkillEffectMissingParameterException>(() => new ItemMinRangeSetEffect(parameters));
        }

        [TestMethod]
        public void ItemMinRangeSetEffect_Constructor_2EmptyStrings()
        {
            List<string> parameters = new List<string>() { string.Empty, string.Empty };

            Assert.ThrowsException<NonZeroPositiveIntegerException>(() => new ItemMinRangeSetEffect(parameters));
        }

        [TestMethod]
        public void ItemMinRangeSetEffect_Constructor_Value_0()
        {
            List<string> parameters = new List<string>() { string.Empty, "0" };

            Assert.ThrowsException<NonZeroPositiveIntegerException>(() => new ItemMinRangeSetEffect(parameters));
        }

        [TestMethod]
        public void ItemMinRangeSetEffect_Constructor_EmptyCategories()
        {
            List<string> parameters = new List<string>() { string.Empty, "1" };

            Assert.ThrowsException<RequiredValueNotProvidedException>(() => new ItemMinRangeSetEffect(parameters));
        }

        #endregion Constructor
    }
}
