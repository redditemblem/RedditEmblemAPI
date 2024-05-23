using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.System.StatusConditions.Effects;

namespace UnitTests.Models.System.StatusConditions.Effects
{
    [TestClass]
    public class AddTagEffectTests
    {
        #region Constructor

        [TestMethod]
        public void AddTagEffect_Constructor_Null()
        {
            List<string> parameters = new List<string>();

            Assert.ThrowsException<StatusConditionEffectMissingParameterException>(() => new AddTagEffect(parameters));
        }

        [TestMethod]
        public void AddTagEffect_Constructor_1EmptyString()
        {
            List<string> parameters = new List<string>() { string.Empty };

            Assert.ThrowsException<RequiredValueNotProvidedException>(() => new AddTagEffect(parameters));
        }

        #endregion Constructor
    }
}