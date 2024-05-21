using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.System.Skills.Effects.ItemRange;

namespace UnitTests.Models.System.Skills.Effects.Items
{
    [TestClass]
    public class ItemMaxUsesMultiplierEffectTests
    {
        #region Constructor

        [TestMethod]
        public void ItemMaxUsesMultiplierEffect_Constructor_Null()
        {
            List<string> parameters = new List<string>();

            Assert.ThrowsException<SkillEffectMissingParameterException>(() => new ItemMaxUsesMultiplierEffect(parameters));
        }

        [TestMethod]
        public void ItemMaxUsesMultiplierEffect_Constructor_1EmptyString()
        {
            List<string> parameters = new List<string>() { string.Empty };

            Assert.ThrowsException<SkillEffectMissingParameterException>(() => new ItemMaxUsesMultiplierEffect(parameters));
        }

        [TestMethod]
        public void ItemMaxUsesMultiplierEffect_Constructor_2EmptyStrings()
        {
            List<string> parameters = new List<string>() { string.Empty, string.Empty };

            Assert.ThrowsException<OneOrGreaterDecimalException>(() => new ItemMaxUsesMultiplierEffect(parameters));
        }

        [TestMethod]
        public void ItemMaxUsesMultiplierEffect_Constructor_Multiplier_0_5()
        {
            List<string> parameters = new List<string>() { string.Empty, "0.5" };

            Assert.ThrowsException<OneOrGreaterDecimalException>(() => new ItemMaxUsesMultiplierEffect(parameters));
        }

        [TestMethod]
        public void ItemMaxUsesMultiplierEffect_Constructor_EmptyCategory()
        {
            List<string> parameters = new List<string>() { string.Empty, "1" };

            Assert.ThrowsException<RequiredValueNotProvidedException>(() => new ItemMaxUsesMultiplierEffect(parameters));
        }

        #endregion Constructor
    }
}
