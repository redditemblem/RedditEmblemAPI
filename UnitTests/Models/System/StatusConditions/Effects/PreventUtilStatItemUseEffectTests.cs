using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.System.StatusConditions.Effects;

namespace UnitTests.Models.System.StatusConditions.Effects
{
    [TestClass]
    public class PreventUtilStatItemUseEffectTests
    {
        #region Constructor

        [TestMethod]
        public void PreventUtilStatItemUseEffect_Constructor_Null()
        {
            List<string> parameters = new List<string>();

            Assert.ThrowsException<StatusConditionEffectMissingParameterException>(() => new PreventUtilStatItemUseEffect(parameters));
        }

        [TestMethod]
        public void PreventUtilStatItemUseEffect_Constructor_1EmptyString()
        {
            List<string> parameters = new List<string>() { string.Empty };

            Assert.ThrowsException<RequiredValueNotProvidedException>(() => new PreventUtilStatItemUseEffect(parameters));
        }

        #endregion Constructor
    }
}
