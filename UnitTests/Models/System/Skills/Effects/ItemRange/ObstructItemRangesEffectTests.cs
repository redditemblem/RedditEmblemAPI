using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.System.Skills.Effects.ItemRange;

namespace UnitTests.Models.System.Skills.Effects.ItemRange
{
    public class ObstructItemRangesEffectTests
    {
        #region Constructor

        [Test]
        public void Constructor_Null()
        {
            List<string> parameters = new List<string>();

            Assert.Throws<SkillEffectMissingParameterException>(() => new ObstructItemRangesEffect(parameters));
        }

        [Test]
        public void Constructor_1EmptyString()
        {
            List<string> parameters = new List<string>() { string.Empty };

            Assert.Throws<PositiveIntegerException>(() => new ObstructItemRangesEffect(parameters));
        }

        [Test]
        public void Constructor_Radius_Neg1()
        {
            List<string> parameters = new List<string>() { "-1" };

            Assert.Throws<PositiveIntegerException>(() => new ObstructItemRangesEffect(parameters));
        }

        #endregion Constructor
    }
}
