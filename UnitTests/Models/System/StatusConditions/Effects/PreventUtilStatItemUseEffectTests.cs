using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.System.StatusConditions.Effects;

namespace UnitTests.Models.System.StatusConditions.Effects
{
    public class PreventUtilStatItemUseEffectTests
    {
        #region Constructor

        [Test]
        public void Constructor_Null()
        {
            List<string> parameters = new List<string>();

            Assert.Throws<StatusConditionEffectMissingParameterException>(() => new PreventUtilStatItemUseEffect(parameters));
        }

        [Test]
        public void Constructor_1EmptyString()
        {
            List<string> parameters = new List<string>() { string.Empty };

            Assert.Throws<RequiredValueNotProvidedException>(() => new PreventUtilStatItemUseEffect(parameters));
        }

        #endregion Constructor
    }
}
