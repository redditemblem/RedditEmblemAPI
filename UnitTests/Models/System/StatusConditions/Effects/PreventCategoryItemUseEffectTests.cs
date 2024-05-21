using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.System.StatusConditions.Effects;

namespace UnitTests.Models.System.StatusConditions.Effects
{
    [TestClass]
    public class PreventCategoryItemUseEffectTests
    {
        #region Constructor

        [TestMethod]
        public void PreventCategoryItemUseEffect_Constructor_Null()
        {
            List<string> parameters = new List<string>();

            Assert.ThrowsException<StatusConditionEffectMissingParameterException>(() => new PreventCategoryItemUseEffect(parameters));
        }

        [TestMethod]
        public void PreventCategoryItemUseEffect_Constructor_1EmptyString()
        {
            List<string> parameters = new List<string>() { string.Empty };

            Assert.ThrowsException<RequiredValueNotProvidedException>(() => new PreventCategoryItemUseEffect(parameters));
        }

        #endregion Constructor
    }
}
