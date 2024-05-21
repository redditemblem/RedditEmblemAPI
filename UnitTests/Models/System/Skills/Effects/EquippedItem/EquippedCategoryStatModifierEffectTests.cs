using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.System.Skills.Effects.EquippedItem;

namespace UnitTests.Models.System.Skills.Effects.EquippedItem
{
    [TestClass]
    public class EquippedCategoryStatModifierEffectTests
    {
        #region Constructor

        [TestMethod]
        public void EquippedCategoryStatModifierEffect_Constructor_Null()
        {
            List<string> parameters = new List<string>();

            Assert.ThrowsException<SkillEffectMissingParameterException>(() => new EquippedCategoryStatModifierEffect(parameters));
        }

        [TestMethod]
        public void EquippedCategoryStatModifierEffect_Constructor_1EmptyString()
        {
            List<string> parameters = new List<string>() { string.Empty };

            Assert.ThrowsException<SkillEffectMissingParameterException>(() => new EquippedCategoryStatModifierEffect(parameters));
        }

        [TestMethod]
        public void EquippedCategoryStatModifierEffect_Constructor_2EmptyString()
        {
            List<string> parameters = new List<string>() { string.Empty, string.Empty };

            Assert.ThrowsException<SkillEffectMissingParameterException>(() => new EquippedCategoryStatModifierEffect(parameters));
        }

        [TestMethod]
        public void EquippedCategoryStatModifierEffect_Constructor_3EmptyString()
        {
            List<string> parameters = new List<string>() { string.Empty, string.Empty, string.Empty };

            Assert.ThrowsException<RequiredValueNotProvidedException>(() => new EquippedCategoryStatModifierEffect(parameters));
        }

        [TestMethod]
        public void EquippedCategoryStatModifierEffect_Constructor_NoCategories()
        {
            List<string> parameters = new List<string>() { string.Empty, "Stat1", "0" };

            Assert.ThrowsException<RequiredValueNotProvidedException>(() => new EquippedCategoryStatModifierEffect(parameters));
        }

        [TestMethod]
        public void EquippedCategoryStatModifierEffect_Constructor_NoStats()
        {
            List<string> parameters = new List<string>() { "Category", string.Empty, "0" };

            Assert.ThrowsException<RequiredValueNotProvidedException>(() => new EquippedCategoryStatModifierEffect(parameters));
        }

        [TestMethod]
        public void EquippedCategoryStatModifierEffect_Constructor_NoValues()
        {
            List<string> parameters = new List<string>() { "Category", "Stat1", string.Empty };

            Assert.ThrowsException<RequiredValueNotProvidedException>(() => new EquippedCategoryStatModifierEffect(parameters));
        }

        #endregion Constructor
    }
}
