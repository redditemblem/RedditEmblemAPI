using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.System.StatusConditions.Effects;

namespace UnitTests.Models.System.StatusConditions.Effects
{
    [TestClass]
    public class RemoveTagEffectTests
    {
        #region Constructor

        [TestMethod]
        public void RemoveTagEffect_Constructor_Null()
        {
            List<string> parameters = new List<string>();

            Assert.ThrowsException<StatusConditionEffectMissingParameterException>(() => new RemoveTagEffect(parameters));
        }

        [TestMethod]
        public void RemoveTagEffect_Constructor_1EmptyString()
        {
            List<string> parameters = new List<string>() { string.Empty };

            Assert.ThrowsException<RequiredValueNotProvidedException>(() => new RemoveTagEffect(parameters));
        }

        #endregion Constructor
    }
}