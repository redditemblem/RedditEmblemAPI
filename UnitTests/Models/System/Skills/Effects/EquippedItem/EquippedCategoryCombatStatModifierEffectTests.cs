using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.System.Skills.Effects.EquippedItem;

namespace UnitTests.Models.System.Skills.Effects.EquippedItem
{
    [TestClass]
    public class EquippedCategoryCombatStatModifierEffectTests
    {
        #region Constructor

        [TestMethod]
        public void EquippedCategoryCombatStatModifierEffect_Constructor_Null()
        {
            List<string> parameters = new List<string>();

            Assert.ThrowsException<SkillEffectMissingParameterException>(() => new EquippedCategoryCombatStatModifierEffect(parameters));
        }

        [TestMethod]
        public void EquippedCategoryCombatStatModifierEffect_Constructor_1EmptyString()
        {
            List<string> parameters = new List<string>() { string.Empty };

            Assert.ThrowsException<SkillEffectMissingParameterException>(() => new EquippedCategoryCombatStatModifierEffect(parameters));
        }

        [TestMethod]
        public void EquippedCategoryCombatStatModifierEffect_Constructor_2EmptyString()
        {
            List<string> parameters = new List<string>() { string.Empty, string.Empty };

            Assert.ThrowsException<SkillEffectMissingParameterException>(() => new EquippedCategoryCombatStatModifierEffect(parameters));
        }

        [TestMethod]
        public void EquippedCategoryCombatStatModifierEffect_Constructor_3EmptyString()
        {
            List<string> parameters = new List<string>() { string.Empty, string.Empty, string.Empty };

            Assert.ThrowsException<RequiredValueNotProvidedException>(() => new EquippedCategoryCombatStatModifierEffect(parameters));
        }

        [TestMethod]
        public void EquippedCategoryCombatStatModifierEffect_Constructor_NoCategories()
        {
            List<string> parameters = new List<string>() { string.Empty, "Stat1", "0" };

            Assert.ThrowsException<RequiredValueNotProvidedException>(() => new EquippedCategoryCombatStatModifierEffect(parameters));
        }

        [TestMethod]
        public void EquippedCategoryCombatStatModifierEffect_Constructor_NoStats()
        {
            List<string> parameters = new List<string>() { "Category", string.Empty, "0" };

            Assert.ThrowsException<RequiredValueNotProvidedException>(() => new EquippedCategoryCombatStatModifierEffect(parameters));
        }

        [TestMethod]
        public void EquippedCategoryCombatStatModifierEffect_Constructor_NoValues()
        {
            List<string> parameters = new List<string>() { "Category", "Stat1", string.Empty };

            Assert.ThrowsException<RequiredValueNotProvidedException>(() => new EquippedCategoryCombatStatModifierEffect(parameters));
        }

        #endregion Constructor
    }
}
