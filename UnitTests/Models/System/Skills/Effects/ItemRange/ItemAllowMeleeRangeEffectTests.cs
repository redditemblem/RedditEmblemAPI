using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.System.Skills.Effects.ItemRange;

namespace UnitTests.Models.System.Skills.Effects.ItemRange
{
    [TestClass]
    public class ItemAllowMeleeRangeEffectTests
    {
        #region Constructor

        [TestMethod]
        public void ItemAllowMeleeRangeEffect_Constructor_Null()
        {
            List<string> parameters = new List<string>();

            Assert.ThrowsException<SkillEffectMissingParameterException>(() => new ItemAllowMeleeRangeEffect(parameters));
        }

        [TestMethod]
        public void ItemAllowMeleeRangeEffect_Constructor_1EmptyString()
        {
            List<string> parameters = new List<string>() { string.Empty };

            Assert.ThrowsException<RequiredValueNotProvidedException>(() => new ItemAllowMeleeRangeEffect(parameters));
        }

        #endregion Constructor
    }
}
