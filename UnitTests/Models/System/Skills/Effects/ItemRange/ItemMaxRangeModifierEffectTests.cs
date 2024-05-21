using RedditEmblemAPI.Models.Exceptions.Unmatched;
using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.System.Skills.Effects.ItemRange;

namespace UnitTests.Models.System.Skills.Effects.ItemRange
{
    [TestClass]
    public class ItemMaxRangeModifierEffectTests
    {
        #region Constructor

        [TestMethod]
        public void ItemMaxRangeModifierEffect_Constructor_Null()
        {
            List<string> parameters = new List<string>();

            Assert.ThrowsException<SkillEffectMissingParameterException>(() => new ItemMaxRangeModifierEffect(parameters));
        }

        [TestMethod]
        public void ItemMaxRangeModifierEffect_Constructor_1EmptyString()
        {
            List<string> parameters = new List<string>() { string.Empty };

            Assert.ThrowsException<SkillEffectMissingParameterException>(() => new ItemMaxRangeModifierEffect(parameters));
        }

        [TestMethod]
        public void ItemMaxRangeModifierEffect_Constructor_2EmptyStrings()
        {
            List<string> parameters = new List<string>() { string.Empty, string.Empty };

            Assert.ThrowsException<SkillEffectMissingParameterException>(() => new ItemMaxRangeModifierEffect(parameters));
        }

        [TestMethod]
        public void ItemMaxRangeModifierEffect_Constructor_3EmptyStrings()
        {
            List<string> parameters = new List<string>() { string.Empty, string.Empty, string.Empty };

            Assert.ThrowsException<NonZeroPositiveIntegerException>(() => new ItemMaxRangeModifierEffect(parameters));
        }

        [TestMethod]
        public void ItemMaxRangeModifierEffect_Constructor_EmptyCategory()
        {
            List<string> parameters = new List<string>() { string.Empty, "1", string.Empty };

            Assert.ThrowsException<RequiredValueNotProvidedException>(() => new ItemMaxRangeModifierEffect(parameters));
        }

        [TestMethod]
        public void ItemMaxRangeModifierEffect_Constructor_EmptyCategory_WithDealsDamageFilterType()
        {
            List<string> parameters = new List<string>() { string.Empty, "1", "All" };

            Assert.ThrowsException<RequiredValueNotProvidedException>(() => new ItemMaxRangeModifierEffect(parameters));
        }

        [TestMethod]
        public void ItemMaxRangeModifierEffect_Constructor_DealsDamageFilterType_UnknownValue()
        {
            List<string> parameters = new List<string>() { "Category", "1", "Mismatch" };

            Assert.ThrowsException<UnmatchedDealsDamageFilterTypeException>(() => new ItemMaxRangeModifierEffect(parameters));
        }

        #endregion Constructor
    }
}
