using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.System.Skills.Effects;
using RedditEmblemAPI.Models.Output.System.Skills.Effects.ItemRange;

namespace UnitTests.Models.System.Skills.Effects.ItemRange
{
    public class ObstructItemRangesEffectTests
    {
        #region Constructor

        [Test]
        public void Constructor_Null()
        {
            IEnumerable<string> parameters = new List<string>();

            Assert.Throws<SkillEffectMissingParameterException>(() => new ObstructItemRangesEffect(parameters));
        }

        [Test]
        public void Constructor_1EmptyString()
        {
            IEnumerable<string> parameters = new List<string>() { string.Empty };

            Assert.Throws<PositiveIntegerException>(() => new ObstructItemRangesEffect(parameters));
        }

        [Test]
        public void Constructor_Radius_Neg1()
        {
            IEnumerable<string> parameters = new List<string>() { "-1" };

            Assert.Throws<PositiveIntegerException>(() => new ObstructItemRangesEffect(parameters));
        }

        [Test]
        public void Constructor()
        {
            string radius = "2";

            IEnumerable<string> parameters = new List<string>()
            {
                radius
            };

            ObstructItemRangesEffect effect = new ObstructItemRangesEffect(parameters);

            Assert.That(effect.Radius, Is.EqualTo(2));
            Assert.That(effect.ExecutionOrder, Is.EqualTo(SkillEffectExecutionOrder.Standard));
        }

        #endregion Constructor
    }
}
