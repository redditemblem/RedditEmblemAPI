using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Services.Helpers;

namespace UnitTests.Helpers
{
    [TestClass]
    public class DataParser_ListParsingTests
    {
        #region List_Strings

        [TestMethod]
        public void List_Strings_WithInput_Null()
        {
            List<string> data = new List<string>() { };
            List<int> indexes = new List<int>() { 0, 1, 2 };

            List<string> values = DataParser.List_Strings(data, indexes);
            CollectionAssert.AreEqual(new List<string>() { }, values);
        }

        [TestMethod]
        public void List_Strings_WithInput_Null_KeepEmptyStrings()
        {
            List<string> data = new List<string>() { };
            List<int> indexes = new List<int>() { 0, 1, 2 };

            List<string> values = DataParser.List_Strings(data, indexes, true);
            CollectionAssert.AreEqual(new List<string>() { string.Empty, string.Empty, string.Empty }, values);
        }

        [TestMethod]
        public void List_Strings_WithInput_EmptyString()
        {
            List<string> data = new List<string>() { string.Empty, string.Empty, string.Empty };
            List<int> indexes = new List<int>() { 0, 1, 2 };

            List<string> values = DataParser.List_Strings(data, indexes);
            CollectionAssert.AreEqual(new List<string> { }, values);
        }

        [TestMethod]
        public void List_Strings_WithInput_EmptyString_KeepEmptyStrings()
        {
            List<string> data = new List<string>() { string.Empty, string.Empty, string.Empty };
            List<int> indexes = new List<int>() { 0, 1, 2 };

            List<string> values = DataParser.List_Strings(data, indexes, true);
            CollectionAssert.AreEqual(new List<string>() { string.Empty, string.Empty, string.Empty }, values);
        }

        [TestMethod]
        public void List_Strings_WithInput_Value_And_EmptyString()
        {
            List<string> data = new List<string>() { "1", "2", string.Empty };
            List<int> indexes = new List<int>() { 0, 1, 2 };

            List<string> values = DataParser.List_Strings(data, indexes);
            CollectionAssert.AreEqual(new List<string> { "1", "2" }, values);
        }

        [TestMethod]
        public void List_Strings_WithInput_Value_And_EmptyString_KeepEmptyStrings()
        {
            List<string> data = new List<string>() { "1", "2", string.Empty };
            List<int> indexes = new List<int>() { 0, 1, 2 };

            List<string> values = DataParser.List_Strings(data, indexes, true);
            CollectionAssert.AreEqual(new List<string>() { "1", "2", string.Empty }, values);
        }

        [TestMethod]
        public void List_Strings_WithInput_Whitespace()
        {
            List<string> data = new List<string>() { "1", "2", " " };
            List<int> indexes = new List<int>() { 0, 1, 2 };

            List<string> values = DataParser.List_Strings(data, indexes);
            CollectionAssert.AreEqual(new List<string> { "1", "2" }, values);
        }

        [TestMethod]
        public void List_Strings_WithInput_Whitespace_KeepEmptyStrings()
        {
            List<string> data = new List<string>() { "1", "2", " " };
            List<int> indexes = new List<int>() { 0, 1, 2 };

            List<string> values = DataParser.List_Strings(data, indexes, true);
            CollectionAssert.AreEqual(new List<string>() { "1", "2", string.Empty }, values);
        }

        #endregion List_Strings

        #region List_StringCSV

        [TestMethod]
        public void List_StringCSV_WithInput_Null()
        {
            List<string> data = new List<string>() { };
            int index = 0;

            List<string> values = DataParser.List_StringCSV(data, index);
            CollectionAssert.AreEqual(new List<string>() { }, values);
        }

        [TestMethod]
        public void List_StringCSV_WithInput_Null_KeepEmptyStrings()
        {
            List<string> data = new List<string>() { };
            int index = 0;

            List<string> values = DataParser.List_StringCSV(data, index, true);
            CollectionAssert.AreEqual(new List<string>() { }, values);
        }

        [TestMethod]
        public void List_StringCSV_WithInput_EmptyString()
        {
            List<string> data = new List<string>() { string.Empty };
            int index = 0;

            List<string> values = DataParser.List_StringCSV(data, index);
            CollectionAssert.AreEqual(new List<string>() { }, values);
        }

        [TestMethod]
        public void List_StringCSV_WithInput_EmptyString_KeepEmptyStrings()
        {
            List<string> data = new List<string>() { string.Empty };
            int index = 0;

            List<string> values = DataParser.List_StringCSV(data, index, true);
            CollectionAssert.AreEqual(new List<string>() { }, values);
        }

        [TestMethod]
        public void List_StringCSV_WithInput_SingleValue()
        {
            List<string> data = new List<string>() { "1" };
            int index = 0;

            List<string> values = DataParser.List_StringCSV(data, index);
            CollectionAssert.AreEqual(new List<string>() { "1" }, values);
        }

        [TestMethod]
        public void List_StringCSV_WithInput_MultipleValues()
        {
            List<string> data = new List<string>() { "1,2,3" };
            int index = 0;

            List<string> values = DataParser.List_StringCSV(data, index);
            CollectionAssert.AreEqual(new List<string>() { "1", "2", "3" }, values);
        }

        [TestMethod]
        public void List_StringCSV_WithInput_MultipleValues_WithWhitespace()
        {
            List<string> data = new List<string>() { "1,2,3," };
            int index = 0;

            List<string> values = DataParser.List_StringCSV(data, index);
            CollectionAssert.AreEqual(new List<string>() { "1", "2", "3" }, values);
        }

        [TestMethod]
        public void List_StringCSV_WithInput_MultipleValues_WithWhitespace_KeepEmptyStrings()
        {
            List<string> data = new List<string>() { "1,2,3," };
            int index = 0;

            List<string> values = DataParser.List_StringCSV(data, index, true);
            CollectionAssert.AreEqual(new List<string>() { "1", "2", "3", string.Empty }, values);
        }

        [TestMethod]
        public void List_StringCSV_WithInput_MultipleIndexes_BothNull()
        {
            List<string> data = new List<string>() { };
            List<int> indexes = new List<int>() { 0, 1 };

            List<string> values = DataParser.List_StringCSV(data, indexes);
            CollectionAssert.AreEqual(new List<string>() { }, values);
        }

        [TestMethod]
        public void List_StringCSV_WithInput_MultipleIndexes_OneNull()
        {
            List<string> data = new List<string>() { "1,2" };
            List<int> indexes = new List<int>() { 0, 1 };

            List<string> values = DataParser.List_StringCSV(data, indexes);
            CollectionAssert.AreEqual(new List<string>() { "1", "2" }, values);
        }

        [TestMethod]
        public void List_StringCSV_WithInput_MultipleIndexes()
        {
            List<string> data = new List<string>() { "1,2", "2,3,4" };
            List<int> indexes = new List<int>() { 0, 1 };

            List<string> values = DataParser.List_StringCSV(data, indexes);
            CollectionAssert.AreEqual(new List<string>() { "1", "2", "2", "3", "4" }, values);
        }

        [TestMethod]
        public void List_StringCSV_WithInput_MultipleIndexes_WithWhitespace()
        {
            List<string> data = new List<string>() { "1,2", "," };
            List<int> indexes = new List<int>() { 0, 1 };

            List<string> values = DataParser.List_StringCSV(data, indexes);
            CollectionAssert.AreEqual(new List<string>() { "1", "2" }, values);
        }

        [TestMethod]
        public void List_StringCSV_WithInput_MultipleIndexes_WithWhitespace_KeepEmptyStrings()
        {
            List<string> data = new List<string>() { "1,2", "," };
            List<int> indexes = new List<int>() { 0, 1 };

            List<string> values = DataParser.List_StringCSV(data, indexes, true);
            CollectionAssert.AreEqual(new List<string>() { "1", "2", string.Empty, string.Empty }, values);
        }

        #endregion List_StringCSV

        #region List_IntCSV

        [TestMethod]
        public void List_IntCSV_WithInput_Null()
        {
            List<string> data = new List<string>() { };
            int index = 0;

            List<int> values = DataParser.List_IntCSV(data, index, "List_IntCSV_WithInput_Null", false);
            CollectionAssert.AreEqual(new List<int>() { }, values);
        }

        [TestMethod]
        public void List_IntCSV_WithInput_EmptyString()
        {
            List<string> data = new List<string>() { string.Empty };
            int index = 0;

            List<int> values = DataParser.List_IntCSV(data, index, "List_IntCSV_WithInput_EmptyString", false);
            CollectionAssert.AreEqual(new List<int>() { }, values);
        }

        [TestMethod]
        public void List_IntCSV_WithInput_Whitespace()
        {
            List<string> data = new List<string>() { "   " };
            int index = 0;

            List<int> values = DataParser.List_IntCSV(data, index, "List_IntCSV_WithInput_Whitespace", false);
            CollectionAssert.AreEqual(new List<int>() { }, values);
        }

        [TestMethod]
        public void List_IntCSV_WithInput_DelimitedWhitespace()
        {
            List<string> data = new List<string>() { "," };
            int index = 0;

            Assert.ThrowsException<AnyIntegerException>(() => DataParser.List_IntCSV(data, index, "List_IntCSV_WithInput_DelimitedWhitespace", false));
        }

        [TestMethod]
        public void List_IntCSV_WithInput_DelimitedWhitespace_Positive()
        {
            List<string> data = new List<string>() { "," };
            int index = 0;

            Assert.ThrowsException<PositiveIntegerException>(() => DataParser.List_IntCSV(data, index, "List_IntCSV_WithInput_DelimitedWhitespace_Positive", true));
        }

        [TestMethod]
        public void List_IntCSV_WithInput_SingleValue_Alphabetical()
        {
            List<string> data = new List<string>() { "a" };
            int index = 0;

            Assert.ThrowsException<AnyIntegerException>(() => DataParser.List_IntCSV(data, index, "List_IntCSV_WithInput_SingleValue_Alphabetical", false));
        }

        [TestMethod]
        public void List_IntCSV_WithInput_SingleValue_Alphabetical_Positive()
        {
            List<string> data = new List<string>() { "a" };
            int index = 0;

            Assert.ThrowsException<PositiveIntegerException>(() => DataParser.List_IntCSV(data, index, "List_IntCSV_WithInput_SingleValue_Alphabetical_Positive", true));
        }

        [TestMethod]
        public void List_IntCSV_WithInput_SingleValue()
        {
            List<string> data = new List<string>() { "-1" };
            int index = 0;

            List<int> values = DataParser.List_IntCSV(data, index, "List_IntCSV_WithInput_SingleValue", false);
            CollectionAssert.AreEqual(new List<int>() { -1 }, values);
        }

        [TestMethod]
        public void List_IntCSV_WithInput_SingleValue_Positive()
        {
            List<string> data = new List<string>() { "-1" };
            int index = 0;

            Assert.ThrowsException<PositiveIntegerException>(() => DataParser.List_IntCSV(data, index, "List_IntCSV_WithInput_SingleValue_Positive", true));
        }

        [TestMethod]
        public void List_IntCSV_WithInput_MultipleValues()
        {
            List<string> data = new List<string>() { "1,2" };
            int index = 0;

            List<int> values = DataParser.List_IntCSV(data, index, "List_IntCSV_WithInput_MultipleValues", false);
            CollectionAssert.AreEqual(new List<int>() { 1, 2 }, values);
        }

        [TestMethod]
        public void List_IntCSV_WithInput_MultipleValues_MixedSign()
        {
            List<string> data = new List<string>() { "1,-1" };
            int index = 0;

            List<int> values = DataParser.List_IntCSV(data, index, "List_IntCSV_WithInput_MultipleValues_MixedSign", false);
            CollectionAssert.AreEqual(new List<int>() { 1, -1 }, values);
        }

        [TestMethod]
        public void List_IntCSV_WithInput_MultipleValues_Positive()
        {
            List<string> data = new List<string>() { "1,-1" };
            int index = 0;

            Assert.ThrowsException<PositiveIntegerException>(() => DataParser.List_IntCSV(data, index, "List_IntCSV_WithInput_MultipleValues_Positive", true));
        }

        [TestMethod]
        public void List_IntCSV_WithInput_MultipleValues_WithAlphabetical()
        {
            List<string> data = new List<string>() { "1,-1,a" };
            int index = 0;

            Assert.ThrowsException<AnyIntegerException>(() => DataParser.List_IntCSV(data, index, "List_IntCSV_WithInput_MultipleValues_WithAlphabetical", false));
        }

        [TestMethod]
        public void List_IntCSV_WithInput_MultipleValues_WithAlphabetical_Positive()
        {
            List<string> data = new List<string>() { "1,2,a" };
            int index = 0;

            Assert.ThrowsException<PositiveIntegerException>(() => DataParser.List_IntCSV(data, index, "List_IntCSV_WithInput_MultipleValues_WithAlphabetical_Positive", true));
        }

        #endregion List_IntCSV
    }
}
