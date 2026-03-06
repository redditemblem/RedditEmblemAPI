using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.System.Skills.Effects;
using RedditEmblemAPI.Models.Output.System.Skills.Effects.ItemRange;

namespace UnitTests.Models.System.Skills.Effects.Items
{
    public class ItemMaxUsesMultiplierEffectTests
    {
        #region Constructor

        [Test]
        public void Constructor_Null()
        {
            IEnumerable<string> parameters = new List<string>();

            Assert.Throws<SkillEffectMissingParameterException>(() => new ItemMaxUsesMultiplierEffect(parameters));
        }

        [Test]
        public void Constructor_1EmptyString()
        {
            IEnumerable<string> parameters = new List<string>() { string.Empty };

            Assert.Throws<SkillEffectMissingParameterException>(() => new ItemMaxUsesMultiplierEffect(parameters));
        }

        [Test]
        public void Constructor_2EmptyStrings()
        {
            IEnumerable<string> parameters = new List<string>() { string.Empty, string.Empty };

            Assert.Throws<OneOrGreaterDecimalException>(() => new ItemMaxUsesMultiplierEffect(parameters));
        }

        [Test]
        public void Constructor_Multiplier_0_5()
        {
            IEnumerable<string> parameters = new List<string>() { string.Empty, "0.5" };

            Assert.Throws<OneOrGreaterDecimalException>(() => new ItemMaxUsesMultiplierEffect(parameters));
        }

        [Test]
        public void Constructor_EmptyCategory()
        {
            IEnumerable<string> parameters = new List<string>() { string.Empty, "1" };

            Assert.Throws<RequiredValueNotProvidedException>(() => new ItemMaxUsesMultiplierEffect(parameters));
        }

        [Test]
        public void Constructor()
        {
            string category = "Category";
            string multiplier = "1.5";

            IEnumerable<string> parameters = new List<string>()
            {
                category,
                multiplier
            };

            ItemMaxUsesMultiplierEffect effect = new ItemMaxUsesMultiplierEffect(parameters);

            Assert.That(effect.Categories, Is.EqualTo(new List<string>() { category }));
            Assert.That(effect.Multiplier, Is.EqualTo(1.5m));
            Assert.That(effect.ExecutionOrder, Is.EqualTo(SkillEffectExecutionOrder.Standard));
        }

        #endregion Constructor
    }
}
