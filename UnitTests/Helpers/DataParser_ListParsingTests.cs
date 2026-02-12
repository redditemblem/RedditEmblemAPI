using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Services.Helpers;

namespace UnitTests.Helpers
{
    public class DataParser_ListParsingTests
    {
        #region List_Strings

        [Test]
        public void List_Strings_WithInput_Null()
        {
            IEnumerable<string> data = new List<string>() { };
            List<int> indexes = new List<int>() { 0, 1, 2 };

            List<string> values = DataParser.List_Strings(data, indexes);

            Assert.That(values, Is.Empty);
        }

        [Test]
        public void List_Strings_WithInput_Null_KeepEmptyStrings()
        {
            IEnumerable<string> data = new List<string>();
            List<int> indexes = new List<int>() { 0, 1, 2 };

            List<string> values = DataParser.List_Strings(data, indexes, true);

            List<string> expected = new List<string>() { string.Empty, string.Empty, string.Empty };
            Assert.That(values, Is.EqualTo(expected));
        }

        [Test]
        public void List_Strings_WithInput_EmptyString()
        {
            IEnumerable<string> data = new List<string>() { string.Empty, string.Empty, string.Empty };
            List<int> indexes = new List<int>() { 0, 1, 2 };

            List<string> values = DataParser.List_Strings(data, indexes);

            Assert.That(values, Is.Empty);
        }

        [Test]
        public void List_Strings_WithInput_EmptyString_KeepEmptyStrings()
        {
            IEnumerable<string> data = new List<string>() { string.Empty, string.Empty, string.Empty };
            List<int> indexes = new List<int>() { 0, 1, 2 };

            List<string> values = DataParser.List_Strings(data, indexes, true);

            List<string> expected = new List<string>() { string.Empty, string.Empty, string.Empty };
            Assert.That(values, Is.EqualTo(expected));
        }

        [Test]
        public void List_Strings_WithInput_Value_And_EmptyString()
        {
            IEnumerable<string> data = new List<string>() { "1", "2", string.Empty };
            List<int> indexes = new List<int>() { 0, 1, 2 };

            List<string> values = DataParser.List_Strings(data, indexes);

            List<string> expected = new List<string>() { "1", "2" };
            Assert.That(values, Is.EqualTo(expected));
        }

        [Test]
        public void List_Strings_WithInput_Value_And_EmptyString_KeepEmptyStrings()
        {
            IEnumerable<string> data = new List<string>() { "1", "2", string.Empty };
            List<int> indexes = new List<int>() { 0, 1, 2 };

            List<string> values = DataParser.List_Strings(data, indexes, true);

            List<string> expected = new List<string>() { "1", "2", string.Empty };
            Assert.That(values, Is.EqualTo(expected));
        }

        [Test]
        public void List_Strings_WithInput_Whitespace()
        {
            IEnumerable<string> data = new List<string>() { "1", "2", " " };
            List<int> indexes = new List<int>() { 0, 1, 2 };

            List<string> values = DataParser.List_Strings(data, indexes);

            List<string> expected = new List<string>() { "1", "2" };
            Assert.That(values, Is.EqualTo(expected));
        }

        [Test]
        public void List_Strings_WithInput_Whitespace_KeepEmptyStrings()
        {
            IEnumerable<string> data = new List<string>() { "1", "2", " " };
            List<int> indexes = new List<int>() { 0, 1, 2 };

            List<string> values = DataParser.List_Strings(data, indexes, true);

            List<string> expected = new List<string>() { "1", "2", string.Empty };
            Assert.That(values, Is.EqualTo(expected));
        }

        #endregion List_Strings

        #region List_StringCSV

        [Test]
        public void List_StringCSV_WithInput_Null()
        {
            IEnumerable<string> data = new List<string>() { };
            int index = 0;

            List<string> values = DataParser.List_StringCSV(data, index);
            
            Assert.That(values, Is.Empty);
        }

        [Test]
        public void List_StringCSV_WithInput_Null_KeepEmptyStrings()
        {
            IEnumerable<string> data = new List<string>() { };
            int index = 0;

            List<string> values = DataParser.List_StringCSV(data, index, true);
            
            Assert.That(values, Is.Empty);
        }

        [Test]
        public void List_StringCSV_WithInput_EmptyString()
        {
            IEnumerable<string> data = new List<string>() { string.Empty };
            int index = 0;

            List<string> values = DataParser.List_StringCSV(data, index);

            Assert.That(values, Is.Empty);
        }

        [Test]
        public void List_StringCSV_WithInput_EmptyString_KeepEmptyStrings()
        {
            IEnumerable<string> data = new List<string>() { string.Empty };
            int index = 0;

            List<string> values = DataParser.List_StringCSV(data, index, true);

            Assert.That(values, Is.Empty);
        }

        [Test]
        public void List_StringCSV_WithInput_SingleValue()
        {
            IEnumerable<string> data = new List<string>() { "1" };
            int index = 0;

            List<string> values = DataParser.List_StringCSV(data, index);

            List<string> expected = new List<string>() { "1" };
            Assert.That(values, Is.EqualTo(expected));
        }

        [Test]
        public void List_StringCSV_WithInput_MultipleValues()
        {
            IEnumerable<string> data = new List<string>() { "1,2,3" };
            int index = 0;

            List<string> values = DataParser.List_StringCSV(data, index);

            List<string> expected = new List<string>() { "1", "2", "3" };
            Assert.That(values, Is.EqualTo(expected));
        }

        [Test]
        public void List_StringCSV_WithInput_MultipleValues_WithWhitespace()
        {
            IEnumerable<string> data = new List<string>() { "1,2,3," };
            int index = 0;

            List<string> values = DataParser.List_StringCSV(data, index);

            List<string> expected = new List<string>() { "1", "2", "3" };
            Assert.That(values, Is.EqualTo(expected));
        }

        [Test]
        public void List_StringCSV_WithInput_MultipleValues_WithWhitespace_KeepEmptyStrings()
        {
            IEnumerable<string> data = new List<string>() { "1,2,3," };
            int index = 0;

            List<string> values = DataParser.List_StringCSV(data, index, true);

            List<string> expected = new List<string>() { "1", "2", "3", string.Empty};
            Assert.That(values, Is.EqualTo(expected));
        }

        [Test]
        public void List_StringCSV_WithInput_MultipleIndexes_BothNull()
        {
            IEnumerable<string> data = new List<string>() { };
            List<int> indexes = new List<int>() { 0, 1 };

            List<string> values = DataParser.List_StringCSV(data, indexes);
            
            Assert.That(values, Is.Empty);
        }

        [Test]
        public void List_StringCSV_WithInput_MultipleIndexes_OneNull()
        {
            IEnumerable<string> data = new List<string>() { "1,2" };
            List<int> indexes = new List<int>() { 0, 1 };

            List<string> values = DataParser.List_StringCSV(data, indexes);

            List<string> expected = new List<string>() { "1", "2" };
            Assert.That(values, Is.EqualTo(expected));
        }

        [Test]
        public void List_StringCSV_WithInput_MultipleIndexes()
        {
            IEnumerable<string> data = new List<string>() { "1,2", "2,3,4" };
            List<int> indexes = new List<int>() { 0, 1 };

            List<string> values = DataParser.List_StringCSV(data, indexes);

            List<string> expected = new List<string>() { "1", "2", "2", "3", "4" };
            Assert.That(values, Is.EqualTo(expected));
        }

        [Test]
        public void List_StringCSV_WithInput_MultipleIndexes_WithWhitespace()
        {
            IEnumerable<string> data = new List<string>() { "1,2", "," };
            List<int> indexes = new List<int>() { 0, 1 };

            List<string> values = DataParser.List_StringCSV(data, indexes);

            List<string> expected = new List<string>() { "1", "2" };
            Assert.That(values, Is.EqualTo(expected));
        }

        [Test]
        public void List_StringCSV_WithInput_MultipleIndexes_WithWhitespace_KeepEmptyStrings()
        {
            IEnumerable<string> data = new List<string>() { "1,2", "," };
            List<int> indexes = new List<int>() { 0, 1 };

            List<string> values = DataParser.List_StringCSV(data, indexes, true);

            List<string> expected = new List<string>() { "1", "2", string.Empty, string.Empty };
            Assert.That(values, Is.EqualTo(expected));
        }

        #endregion List_StringCSV

        #region List_IntCSV

        [Test]
        public void List_IntCSV_WithInput_Null()
        {
            IEnumerable<string> data = new List<string>() { };
            int index = 0;

            List<int> values = DataParser.List_IntCSV(data, index, "List_IntCSV_WithInput_Null", false);
            
            Assert.That(values, Is.Empty);
        }

        [Test]
        public void List_IntCSV_WithInput_EmptyString()
        {
            IEnumerable<string> data = new List<string>() { string.Empty };
            int index = 0;

            List<int> values = DataParser.List_IntCSV(data, index, "List_IntCSV_WithInput_EmptyString", false);
            
            Assert.That(values, Is.Empty);
        }

        [Test]
        public void List_IntCSV_WithInput_Whitespace()
        {
            IEnumerable<string> data = new List<string>() { UnitTestConsts.WHITESPACE_STRING };
            int index = 0;

            List<int> values = DataParser.List_IntCSV(data, index, "List_IntCSV_WithInput_Whitespace", false);
            
            Assert.That(values, Is.Empty);
        }

        [Test]
        public void List_IntCSV_WithInput_DelimitedWhitespace()
        {
            IEnumerable<string> data = new List<string>() { "," };
            int index = 0;

            Assert.Throws<AnyIntegerException>(() => DataParser.List_IntCSV(data, index, "List_IntCSV_WithInput_DelimitedWhitespace", false));
        }

        [Test]
        public void List_IntCSV_WithInput_DelimitedWhitespace_Positive()
        {
            IEnumerable<string> data = new List<string>() { "," };
            int index = 0;

            Assert.Throws<PositiveIntegerException>(() => DataParser.List_IntCSV(data, index, "List_IntCSV_WithInput_DelimitedWhitespace_Positive", true));
        }

        [Test]
        public void List_IntCSV_WithInput_SingleValue_Alphabetical()
        {
            IEnumerable<string> data = new List<string>() { "a" };
            int index = 0;

            Assert.Throws<AnyIntegerException>(() => DataParser.List_IntCSV(data, index, "List_IntCSV_WithInput_SingleValue_Alphabetical", false));
        }

        [Test]
        public void List_IntCSV_WithInput_SingleValue_Alphabetical_Positive()
        {
            IEnumerable<string> data = new List<string>() { "a" };
            int index = 0;

            Assert.Throws<PositiveIntegerException>(() => DataParser.List_IntCSV(data, index, "List_IntCSV_WithInput_SingleValue_Alphabetical_Positive", true));
        }

        [Test]
        public void List_IntCSV_WithInput_SingleValue()
        {
            IEnumerable<string> data = new List<string>() { "-1" };
            int index = 0;

            List<int> values = DataParser.List_IntCSV(data, index, "List_IntCSV_WithInput_SingleValue", false);

            List<int> expected = new List<int>() { -1 };
            Assert.That(values, Is.EqualTo(expected));
        }

        [Test]
        public void List_IntCSV_WithInput_SingleValue_Positive()
        {
            IEnumerable<string> data = new List<string>() { "-1" };
            int index = 0;

            Assert.Throws<PositiveIntegerException>(() => DataParser.List_IntCSV(data, index, "List_IntCSV_WithInput_SingleValue_Positive", true));
        }

        [Test]
        public void List_IntCSV_WithInput_MultipleValues()
        {
            IEnumerable<string> data = new List<string>() { "1,2" };
            int index = 0;

            List<int> values = DataParser.List_IntCSV(data, index, "List_IntCSV_WithInput_MultipleValues", false);

            List<int> expected = new List<int>() { 1, 2 };
            Assert.That(values, Is.EqualTo(expected));
        }

        [Test]
        public void List_IntCSV_WithInput_MultipleValues_MixedSign()
        {
            IEnumerable<string> data = new List<string>() { "1,-1" };
            int index = 0;

            List<int> values = DataParser.List_IntCSV(data, index, "List_IntCSV_WithInput_MultipleValues_MixedSign", false);

            List<int> expected = new List<int>() { 1, -1 };
            Assert.That(values, Is.EqualTo(expected));
        }

        [Test]
        public void List_IntCSV_WithInput_MultipleValues_Positive()
        {
            IEnumerable<string> data = new List<string>() { "1,-1" };
            int index = 0;

            Assert.Throws<PositiveIntegerException>(() => DataParser.List_IntCSV(data, index, "List_IntCSV_WithInput_MultipleValues_Positive", true));
        }

        [Test]
        public void List_IntCSV_WithInput_MultipleValues_WithAlphabetical()
        {
            IEnumerable<string> data = new List<string>() { "1,-1,a" };
            int index = 0;

            Assert.Throws<AnyIntegerException>(() => DataParser.List_IntCSV(data, index, "List_IntCSV_WithInput_MultipleValues_WithAlphabetical", false));
        }

        [Test]
        public void List_IntCSV_WithInput_MultipleValues_WithAlphabetical_Positive()
        {
            IEnumerable<string> data = new List<string>() { "1,2,a" };
            int index = 0;

            Assert.Throws<PositiveIntegerException>(() => DataParser.List_IntCSV(data, index, "List_IntCSV_WithInput_MultipleValues_WithAlphabetical_Positive", true));
        }

        #endregion List_IntCSV
    }
}
