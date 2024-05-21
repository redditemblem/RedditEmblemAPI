using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.System.Skills.Effects.ItemRange;

namespace UnitTests.Models.System.Skills.Effects.ItemRange
{
    [TestClass]
    public class ObstructItemRangesEffectTests
    {
        #region Constructor

        [TestMethod]
        public void ObstructItemRangesEffect_Constructor_Null()
        {
            List<string> parameters = new List<string>();

            Assert.ThrowsException<SkillEffectMissingParameterException>(() => new ObstructItemRangesEffect(parameters));
        }

        [TestMethod]
        public void ObstructItemRangesEffect_Constructor_1EmptyString()
        {
            List<string> parameters = new List<string>() { string.Empty };

            Assert.ThrowsException<PositiveIntegerException>(() => new ObstructItemRangesEffect(parameters));
        }

        [TestMethod]
        public void ObstructItemRangesEffect_Constructor_Radius_Neg1()
        {
            List<string> parameters = new List<string>() { "-1" };

            Assert.ThrowsException<PositiveIntegerException>(() => new ObstructItemRangesEffect(parameters));
        }

        #endregion Constructor
    }
}
