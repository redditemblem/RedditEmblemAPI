using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.System.Skills.Effects.ItemRange;

namespace UnitTests.Models.System.Skills.Effects.Items
{
    public class ItemMaxUsesMultiplierEffectTests
    {
        #region Constructor

        [Test]
        public void Constructor_Null()
        {
            List<string> parameters = new List<string>();

            Assert.Throws<SkillEffectMissingParameterException>(() => new ItemMaxUsesMultiplierEffect(parameters));
        }

        [Test]
        public void Constructor_1EmptyString()
        {
            List<string> parameters = new List<string>() { string.Empty };

            Assert.Throws<SkillEffectMissingParameterException>(() => new ItemMaxUsesMultiplierEffect(parameters));
        }

        [Test]
        public void Constructor_2EmptyStrings()
        {
            List<string> parameters = new List<string>() { string.Empty, string.Empty };

            Assert.Throws<OneOrGreaterDecimalException>(() => new ItemMaxUsesMultiplierEffect(parameters));
        }

        [Test]
        public void Constructor_Multiplier_0_5()
        {
            List<string> parameters = new List<string>() { string.Empty, "0.5" };

            Assert.Throws<OneOrGreaterDecimalException>(() => new ItemMaxUsesMultiplierEffect(parameters));
        }

        [Test]
        public void Constructor_EmptyCategory()
        {
            List<string> parameters = new List<string>() { string.Empty, "1" };

            Assert.Throws<RequiredValueNotProvidedException>(() => new ItemMaxUsesMultiplierEffect(parameters));
        }

        #endregion Constructor
    }
}
