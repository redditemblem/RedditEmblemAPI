using RedditEmblemAPI.Helpers;

namespace UnitTests.Helpers
{
    public class DataParser_BooleanTests
    {
        #region OptionalBoolean_YesNo

        [Test]
        public void OptionalBoolean_YesNo_WithInput_MultiSet_Null()
        {
            IEnumerable<IEnumerable<string>> data = null;
            (int, int) indices = (0, 0);

            bool value = DataParser.OptionalBoolean_YesNo(data, indices, nameof(OptionalBoolean_YesNo_WithInput_MultiSet_Null));
            Assert.That(value, Is.False);
        }

        [Test]
        public void OptionalBoolean_YesNo_WithInput_MultiSet_IndexOutOfBounds()
        {
            IEnumerable<IEnumerable<string>> data = new string[][] { Array.Empty<string>() };
            (int, int) indices = (0, 0);

            bool value = DataParser.OptionalBoolean_YesNo(data, indices, nameof(OptionalBoolean_YesNo_WithInput_MultiSet_IndexOutOfBounds));
            Assert.That(value, Is.False);
        }

        [TestCase("Yes", "", 0, 0, true)]
        [TestCase("Yes", "", 1, 0, false)]
        [TestCase("", "Yes", 0, 0, false)]
        [TestCase("", "Yes", 1, 0, true)]
        public void OptionalBoolean_YesNo_MultiSet(string input1, string input2, int setIndex, int cellIndex, bool expected)
        {
            IEnumerable<IEnumerable<string>> data = new string[][] 
            {
                new string[]{ input1 },
                new string[]{ input2 }
            };
            (int, int) indices = (setIndex, cellIndex);

            bool value = DataParser.OptionalBoolean_YesNo(data, indices, nameof(OptionalBoolean_YesNo_MultiSet));
            Assert.That(value, Is.EqualTo(expected));
        }

        [Test]
        public void OptionalBoolean_YesNo_WithInput_Null()
        {
            IEnumerable<string> data = null;
            int index = 0;

            bool value = DataParser.OptionalBoolean_YesNo(data, index, nameof(OptionalBoolean_YesNo_WithInput_Null));
            Assert.That(value, Is.False);
        }

        [Test]
        public void OptionalBoolean_YesNo_WithInput_IndexOutOfBounds()
        {
            IEnumerable<string> data = new List<string>();
            int index = 0;

            bool value = DataParser.OptionalBoolean_YesNo(data, index, nameof(OptionalBoolean_YesNo_WithInput_IndexOutOfBounds));
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

            bool value = DataParser.OptionalBoolean_YesNo(data, index, nameof(OptionalBoolean_YesNo));
            Assert.That(value, Is.EqualTo(expected));
        }

        #endregion OptionalBoolean_YesNo
    }
}
