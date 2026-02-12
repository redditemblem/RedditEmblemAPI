using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.System.Skills.Effects.EquippedItem;

namespace UnitTests.Models.System.Skills.Effects.EquippedItem
{
    public class EquippedCategoryStatModifierEffectTests
    {
        #region Constructor

        [Test]
        public void Constructor_Null()
        {
            List<string> parameters = new List<string>();

            Assert.Throws<SkillEffectMissingParameterException>(() => new EquippedCategoryStatModifierEffect(parameters));
        }

        [Test]
        public void Constructor_1EmptyString()
        {
            List<string> parameters = new List<string>() { string.Empty };

            Assert.Throws<SkillEffectMissingParameterException>(() => new EquippedCategoryStatModifierEffect(parameters));
        }

        [Test]
        public void Constructor_2EmptyString()
        {
            List<string> parameters = new List<string>() { string.Empty, string.Empty };

            Assert.Throws<SkillEffectMissingParameterException>(() => new EquippedCategoryStatModifierEffect(parameters));
        }

        [Test]
        public void Constructor_3EmptyString()
        {
            List<string> parameters = new List<string>() { string.Empty, string.Empty, string.Empty };

            Assert.Throws<RequiredValueNotProvidedException>(() => new EquippedCategoryStatModifierEffect(parameters));
        }

        [Test]
        public void Constructor_NoCategories()
        {
            List<string> parameters = new List<string>() { string.Empty, "Stat1", "0" };

            Assert.Throws<RequiredValueNotProvidedException>(() => new EquippedCategoryStatModifierEffect(parameters));
        }

        [Test]
        public void Constructor_NoStats()
        {
            List<string> parameters = new List<string>() { "Category", string.Empty, "0" };

            Assert.Throws<RequiredValueNotProvidedException>(() => new EquippedCategoryStatModifierEffect(parameters));
        }

        [Test]
        public void Constructor_NoValues()
        {
            List<string> parameters = new List<string>() { "Category", "Stat1", string.Empty };

            Assert.Throws<RequiredValueNotProvidedException>(() => new EquippedCategoryStatModifierEffect(parameters));
        }

        #endregion Constructor
    }
}
