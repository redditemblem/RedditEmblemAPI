using RedditEmblemAPI.Services.Helpers;

namespace UnitTests.Helpers
{
    [TestClass]
    public class DataParser_BooleanParsingTests
    {
        #region OptionalBoolean_YesNo

        [TestMethod]
        public void OptionalBoolean_YesNo_WithInput_Null()
        {
            List<string> data = new List<string>() { };
            int index = 0;

            bool value = DataParser.OptionalBoolean_YesNo(data, index, "OptionalBoolean_YesNo_WithInput_Null");
            Assert.IsFalse(value);
        }

        [TestMethod]
        public void OptionalBoolean_YesNo_WithInput_EmptyString()
        {
            List<string> data = new List<string>() { string.Empty };
            int index = 0;

            bool value = DataParser.OptionalBoolean_YesNo(data, index, "OptionalBoolean_YesNo_WithInput_EmptyString");
            Assert.IsFalse(value);
        }

        [TestMethod]
        public void OptionalBoolean_YesNo_WithInput_Whitespace()
        {
            List<string> data = new List<string>() { "   " };
            int index = 0;

            bool value = DataParser.OptionalBoolean_YesNo(data, index, "OptionalBoolean_YesNo_WithInput_Whitespace");
            Assert.IsFalse(value);
        }

        [TestMethod]
        public void OptionalBoolean_YesNo_WithInput_Alphabetical()
        {
            List<string> data = new List<string>() { "test" };
            int index = 0;

            bool value = DataParser.OptionalBoolean_YesNo(data, index, "OptionalBoolean_YesNo_WithInput_Alphabetical");
            Assert.IsFalse(value);
        }

        [TestMethod]
        public void OptionalBoolean_YesNo_WithInput_Y()
        {
            List<string> data = new List<string>() { "Y" };
            int index = 0;

            bool value = DataParser.OptionalBoolean_YesNo(data, index, "OptionalBoolean_YesNo_WithInput_Y");
            Assert.IsFalse(value);
        }

        [TestMethod]
        public void OptionalBoolean_YesNo_WithInput_y()
        {
            List<string> data = new List<string>() { "y" };
            int index = 0;

            bool value = DataParser.OptionalBoolean_YesNo(data, index, "OptionalBoolean_YesNo_WithInput_y");
            Assert.IsFalse(value);
        }

        [TestMethod]
        public void OptionalBoolean_YesNo_WithInput_N()
        {
            List<string> data = new List<string>() { "N" };
            int index = 0;

            bool value = DataParser.OptionalBoolean_YesNo(data, index, "OptionalBoolean_YesNo_WithInput_N");
            Assert.IsFalse(value);
        }

        [TestMethod]
        public void OptionalBoolean_YesNo_WithInput_n()
        {
            List<string> data = new List<string>() { "n" };
            int index = 0;

            bool value = DataParser.OptionalBoolean_YesNo(data, index, "OptionalBoolean_YesNo_WithInput_n");
            Assert.IsFalse(value);
        }

        [TestMethod]
        public void OptionalBoolean_YesNo_WithInput_YES()
        {
            List<string> data = new List<string>() { "YES" };
            int index = 0;

            bool value = DataParser.OptionalBoolean_YesNo(data, index, "OptionalBoolean_YesNo_WithInput_YES");
            Assert.IsTrue(value);
        }

        [TestMethod]
        public void OptionalBoolean_YesNo_WithInput_yes()
        {
            List<string> data = new List<string>() { "yes" };
            int index = 0;

            bool value = DataParser.OptionalBoolean_YesNo(data, index, "OptionalBoolean_YesNo_WithInput_yes");
            Assert.IsTrue(value);
        }

        [TestMethod]
        public void OptionalBoolean_YesNo_WithInput_YeS()
        {
            List<string> data = new List<string>() { "YeS" };
            int index = 0;

            bool value = DataParser.OptionalBoolean_YesNo(data, index, "OptionalBoolean_YesNo_WithInput_YeS");
            Assert.IsTrue(value);
        }

        [TestMethod]
        public void OptionalBoolean_YesNo_WithInput_NO()
        {
            List<string> data = new List<string>() { "NO" };
            int index = 0;

            bool value = DataParser.OptionalBoolean_YesNo(data, index, "OptionalBoolean_YesNo_WithInput_NO");
            Assert.IsFalse(value);
        }

        [TestMethod]
        public void OptionalBoolean_YesNo_WithInput_no()
        {
            List<string> data = new List<string>() { "no" };
            int index = 0;

            bool value = DataParser.OptionalBoolean_YesNo(data, index, "OptionalBoolean_YesNo_WithInput_no");
            Assert.IsFalse(value);
        }

        [TestMethod]
        public void OptionalBoolean_YesNo_WithInput_No()
        {
            List<string> data = new List<string>() { "No" };
            int index = 0;

            bool value = DataParser.OptionalBoolean_YesNo(data, index, "OptionalBoolean_YesNo_WithInput_No");
            Assert.IsFalse(value);
        }

        #endregion OptionalBoolean_YesNo
    }
}
