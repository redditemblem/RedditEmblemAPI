using RedditEmblemAPI.Helpers;

namespace UnitTests.Helpers
{
    public class DataParser_BooleanParsingTests
    {
        #region OptionalBoolean_YesNo

        [Test]
        public void OptionalBoolean_YesNo_WithInput_Null()
        {
            IEnumerable<string> data = new List<string>();
            int index = 0;

            bool value = DataParser.OptionalBoolean_YesNo(data, index, "OptionalBoolean_YesNo_WithInput_Null");
            Assert.That(value, Is.False);
        }

        [TestCase("", false)]
        [TestCase("   ", false)]
        [TestCase("test", false)]
        [TestCase("Y", false)]
        [TestCase("y", false)]
        [TestCase("N", false)]
        [TestCase("n", false)]
        [TestCase("YES", true)]
        [TestCase("yes", true)]
        [TestCase("YeS", true)]
        [TestCase("NO", false)]
        [TestCase("no", false)]
        [TestCase("No", false)]
        public void OptionalBoolean_YesNo(string input, bool expected)
        {
            IEnumerable<string> data = new List<string>() { input };
            int index = 0;

            bool value = DataParser.OptionalBoolean_YesNo(data, index, "OptionalBoolean_YesNo");
            Assert.That(value, Is.EqualTo(expected));
        }

        #endregion OptionalBoolean_YesNo
    }
}
