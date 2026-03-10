using RedditEmblemAPI.Models.Output.System.StatusConditions.Effects;

namespace UnitTests.Models.System.StatusConditions.Effects
{
    public class DoesNotBlockEnemyAffiliationsEffectTests
    {
        #region Constructor

        [Test]
        public void Constructor()
        {
            IEnumerable<string> parameters = new List<string>();

            //Effect does not have any parameters
            Assert.DoesNotThrow(() => new DoesNotBlockEnemyAffiliationsEffect(parameters));
        }

        #endregion Constructor
    }
}
