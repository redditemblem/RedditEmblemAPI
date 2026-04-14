using RedditEmblemAPI.Helpers;
using RedditEmblemAPI.Models.Exceptions.Validation;

namespace UnitTests.Helpers
{
    public class DataParser_ListTests
    {
        #region List_Strings

        [Test]
        public void List_Strings_WithInput_Null()
        {
            IEnumerable<IEnumerable<string>> data = null;
            IEnumerable<(int, int)> indexes = new (int, int)[] { (0, 0), (0, 1), (0, 2) };

            List<string> values = DataParser.List_Strings(data, indexes);

            Assert.That(values, Is.Empty);
        }

        [Test]
        public void List_Strings_WithInput_IndexOutOfBounds()
        {
            IEnumerable<IEnumerable<string>> data = [];
            IEnumerable<(int, int)> indexes = new (int, int)[] { (0, 0), (0, 1), (0, 2) };

            List<string> values = DataParser.List_Strings(data, indexes);

            Assert.That(values, Is.Empty);
        }

        [Test]
        public void List_Strings_WithInput_IndexOutOfBounds_KeepEmptyStrings()
        {
            IEnumerable<IEnumerable<string>> data = [];
            IEnumerable<(int, int)> indexes = new (int, int)[] { (0, 0), (0, 1), (0, 2) };

            List<string> values = DataParser.List_Strings(data, indexes, true);

            List<string> expected = new List<string>() { string.Empty, string.Empty, string.Empty };
            Assert.That(values, Is.EqualTo(expected));
        }

        [Test]
        public void List_Strings_WithInput_EmptyString()
        {
            IEnumerable<IEnumerable<string>> data = new string[][]
            {
                new string[]{ string.Empty, string.Empty, string.Empty }
            };
            IEnumerable<(int, int)> indexes = new (int, int)[] { (0, 0), (0, 1), (0, 2) };

            List<string> values = DataParser.List_Strings(data, indexes);

            Assert.That(values, Is.Empty);
        }

        [Test]
        public void List_Strings_WithInput_EmptyString_KeepEmptyStrings()
        {
            IEnumerable<IEnumerable<string>> data = new string[][]
            {
                new string[]{ string.Empty, string.Empty, string.Empty }
            };
            IEnumerable<(int, int)> indexes = new (int, int)[] { (0, 0), (0, 1), (0, 2) };

            List<string> values = DataParser.List_Strings(data, indexes, true);

            List<string> expected = new List<string>() { string.Empty, string.Empty, string.Empty };
            Assert.That(values, Is.EqualTo(expected));
        }

        [Test]
        public void List_Strings_WithInput_Value_And_EmptyString()
        {
            IEnumerable<IEnumerable<string>> data = new string[][]
            {
                new string[]{ "1", "2", string.Empty }
            };
            IEnumerable<(int, int)> indexes = new (int, int)[] { (0, 0), (0, 1), (0, 2) };

            List<string> values = DataParser.List_Strings(data, indexes);

            List<string> expected = new List<string>() { "1", "2" };
            Assert.That(values, Is.EqualTo(expected));
        }

        [Test]
        public void List_Strings_WithInput_Value_And_EmptyString_KeepEmptyStrings()
        {
            IEnumerable<IEnumerable<string>> data = new string[][]
            {
                new string[]{ "1", "2", string.Empty }
            };
            IEnumerable<(int, int)> indexes = new (int, int)[] { (0, 0), (0, 1), (0, 2) };

            List<string> values = DataParser.List_Strings(data, indexes, true);

            List<string> expected = new List<string>() { "1", "2", string.Empty };
            Assert.That(values, Is.EqualTo(expected));
        }

        [Test]
        public void List_Strings_WithInput_Whitespace()
        {
            IEnumerable<IEnumerable<string>> data = new string[][] 
            {
                new string[]{ "1", "2", " " }
            };
            IEnumerable<(int, int)> indexes = new (int, int)[] { (0, 0), (0, 1), (0, 2) };

            List<string> values = DataParser.List_Strings(data, indexes);

            List<string> expected = new List<string>() { "1", "2" };
            Assert.That(values, Is.EqualTo(expected));
        }

        [Test]
        public void List_Strings_WithInput_Whitespace_KeepEmptyStrings()
        {
            IEnumerable<IEnumerable<string>> data = new string[][]
            {
                new string[]{ "1", "2", " " }
            };
            IEnumerable<(int, int)> indexes = new (int, int)[] { (0, 0), (0, 1), (0, 2) };

            List<string> values = DataParser.List_Strings(data, indexes, true);

            List<string> expected = new List<string>() { "1", "2", string.Empty };
            Assert.That(values, Is.EqualTo(expected));
        }

        #endregion List_Strings

        #region List_StringCSV

        [Test]
        public void List_StringCSV_WithInput_Null()
        {
            IEnumerable<IEnumerable<string>> data = null;
            IEnumerable<(int, int)> indexes = new (int, int)[] { (0, 0) };

            List<string> values = DataParser.List_StringCSV(data, indexes);

            Assert.That(values, Is.Empty);
        }

        [Test]
        public void List_StringCSV_WithInput_IndexOutOfBounds()
        {
            IEnumerable<IEnumerable<string>> data = [];
            IEnumerable<(int, int)> indexes = new (int, int)[] { (0, 0) };

            List<string> values = DataParser.List_StringCSV(data, indexes);
            
            Assert.That(values, Is.Empty);
        }

        [Test]
        public void List_StringCSV_WithInput_Null_KeepEmptyStrings()
        {
            IEnumerable<IEnumerable<string>> data = [];
            IEnumerable<(int, int)> indexes = new (int, int)[] { (0, 0) };

            List<string> values = DataParser.List_StringCSV(data, indexes, true);
            
            Assert.That(values, Is.Empty);
        }

        [Test]
        public void List_StringCSV_WithInput_EmptyString()
        {
            IEnumerable<IEnumerable<string>> data = new string[][]
            {
                new string[]{ string.Empty }
            };
            IEnumerable<(int, int)> indexes = new (int, int)[] { (0, 0) };

            List<string> values = DataParser.List_StringCSV(data, indexes);

            Assert.That(values, Is.Empty);
        }

        [Test]
        public void List_StringCSV_WithInput_EmptyString_KeepEmptyStrings()
        {
            IEnumerable<IEnumerable<string>> data = new string[][]
            {
                new string[]{ string.Empty }
            };
            IEnumerable<(int, int)> indexes = new (int, int)[] { (0, 0) };

            List<string> values = DataParser.List_StringCSV(data, indexes, true);

            Assert.That(values, Is.Empty);
        }

        [Test]
        public void List_StringCSV_WithInput_SingleValue()
        {
            IEnumerable<IEnumerable<string>> data = new string[][]
            {
                new string[]{ "1" }
            };
            IEnumerable<(int, int)> indexes = new (int, int)[] { (0, 0) };

            List<string> values = DataParser.List_StringCSV(data, indexes);

            List<string> expected = new List<string>() { "1" };
            Assert.That(values, Is.EqualTo(expected));
        }

        [Test]
        public void List_StringCSV_WithInput_MultipleValues()
        {
            IEnumerable<IEnumerable<string>> data = new string[][]
            {
                new string[]{ "1,2,3" }
            };
            IEnumerable<(int, int)> indexes = new (int, int)[] { (0, 0) };

            List<string> values = DataParser.List_StringCSV(data, indexes);

            List<string> expected = new List<string>() { "1", "2", "3" };
            Assert.That(values, Is.EqualTo(expected));
        }

        [Test]
        public void List_StringCSV_WithInput_MultipleValues_WithWhitespace()
        {
            IEnumerable<IEnumerable<string>> data = new string[][]
            {
                new string[]{ "1,2,3," }
            };
            IEnumerable<(int, int)> indexes = new (int, int)[] { (0, 0) };

            List<string> values = DataParser.List_StringCSV(data, indexes);

            List<string> expected = new List<string>() { "1", "2", "3" };
            Assert.That(values, Is.EqualTo(expected));
        }

        [Test]
        public void List_StringCSV_WithInput_MultipleValues_WithWhitespace_KeepEmptyStrings()
        {
            IEnumerable<IEnumerable<string>> data = new string[][]
            {
                new string[]{ "1,2,3," }
            };
            IEnumerable<(int, int)> indexes = new (int, int)[] { (0, 0) };

            List<string> values = DataParser.List_StringCSV(data, indexes, true);

            List<string> expected = new List<string>() { "1", "2", "3", string.Empty};
            Assert.That(values, Is.EqualTo(expected));
        }

        [Test]
        public void List_StringCSV_WithInput_MultipleIndexes_BothNull()
        {
            IEnumerable<IEnumerable<string>> data = [];
            IEnumerable<(int, int)> indexes = new (int, int)[] { (0, 0), (0, 1) };

            List<string> values = DataParser.List_StringCSV(data, indexes);
            
            Assert.That(values, Is.Empty);
        }

        [Test]
        public void List_StringCSV_WithInput_MultipleIndexes_OneNull()
        {
            IEnumerable<IEnumerable<string>> data = new string[][]
            {
                new string[]{ "1,2" }
            };
            IEnumerable<(int, int)> indexes = new (int, int)[] { (0, 0), (0, 1) };

            List<string> values = DataParser.List_StringCSV(data, indexes);

            List<string> expected = new List<string>() { "1", "2" };
            Assert.That(values, Is.EqualTo(expected));
        }

        [Test]
        public void List_StringCSV_WithInput_MultipleIndexes()
        {
            IEnumerable<IEnumerable<string>> data = new string[][]
            {
                new string[]{ "1,2", "2,3,4" }
            };
            IEnumerable<(int, int)> indexes = new (int, int)[] { (0, 0), (0, 1) };

            List<string> values = DataParser.List_StringCSV(data, indexes);

            List<string> expected = new List<string>() { "1", "2", "2", "3", "4" };
            Assert.That(values, Is.EqualTo(expected));
        }

        [Test]
        public void List_StringCSV_WithInput_MultipleIndexes_WithWhitespace()
        {
            IEnumerable<IEnumerable<string>> data = new string[][]
            {
                new string[]{ "1,2", "," }
            };
            IEnumerable<(int, int)> indexes = new (int, int)[] { (0, 0), (0, 1) };

            List<string> values = DataParser.List_StringCSV(data, indexes);

            List<string> expected = new List<string>() { "1", "2" };
            Assert.That(values, Is.EqualTo(expected));
        }

        [Test]
        public void List_StringCSV_WithInput_MultipleIndexes_WithWhitespace_KeepEmptyStrings()
        {
            IEnumerable<IEnumerable<string>> data = new string[][]
            {
                new string[]{ "1,2", "," }
            };
            IEnumerable<(int, int)> indexes = new (int, int)[] { (0, 0), (0, 1) };

            List<string> values = DataParser.List_StringCSV(data, indexes, true);

            List<string> expected = new List<string>() { "1", "2", string.Empty, string.Empty };
            Assert.That(values, Is.EqualTo(expected));
        }

        #endregion List_StringCSV

        #region List_IntCSV

        [Test]
        public void List_IntCSV_WithInput_Null()
        {
            IEnumerable<IEnumerable<string>> data = null;
            (int, int) indices = (0, 0);

            List<int> values = DataParser.List_IntCSV(data, indices, nameof(List_IntCSV_WithInput_Null), false);

            Assert.That(values, Is.Empty);
        }

        [Test]
        public void List_IntCSV_WithInput_IndexOutOfBounds()
        {
            IEnumerable<IEnumerable<string>> data = [];
            (int, int) indices = (0, 0);

            List<int> values = DataParser.List_IntCSV(data, indices, nameof(List_IntCSV_WithInput_IndexOutOfBounds), false);
            
            Assert.That(values, Is.Empty);
        }

        [Test]
        public void List_IntCSV_WithInput_EmptyString()
        {
            IEnumerable<IEnumerable<string>> data = new string[][]
            {
                new string[]{ string.Empty }
            };
            (int, int) indices = (0, 0);

            List<int> values = DataParser.List_IntCSV(data, indices, nameof(List_IntCSV_WithInput_EmptyString), false);
            
            Assert.That(values, Is.Empty);
        }

        [Test]
        public void List_IntCSV_WithInput_Whitespace()
        {
            IEnumerable<IEnumerable<string>> data = new string[][]
            {
                new string[]{ UnitTestConsts.WHITESPACE_STRING }
            };
            (int, int) indices = (0, 0);

            List<int> values = DataParser.List_IntCSV(data, indices, nameof(List_IntCSV_WithInput_Whitespace), false);
            
            Assert.That(values, Is.Empty);
        }

        [Test]
        public void List_IntCSV_WithInput_DelimitedWhitespace()
        {
            IEnumerable<IEnumerable<string>> data = new string[][]
            {
                new string[]{ "," }
            };
            (int, int) indices = (0, 0);

            Assert.Throws<AnyIntegerException>(() => DataParser.List_IntCSV(data, indices, nameof(List_IntCSV_WithInput_DelimitedWhitespace), false));
        }

        [Test]
        public void List_IntCSV_WithInput_DelimitedWhitespace_Positive()
        {
            IEnumerable<IEnumerable<string>> data = new string[][]
            {
                new string[]{ "," }
            };
            (int, int) indices = (0, 0);

            Assert.Throws<PositiveIntegerException>(() => DataParser.List_IntCSV(data, indices, nameof(List_IntCSV_WithInput_DelimitedWhitespace_Positive), true));
        }

        [Test]
        public void List_IntCSV_WithInput_SingleValue_Alphabetical()
        {
            IEnumerable<IEnumerable<string>> data = new string[][]
            {
                new string[]{ "a" }
            };
            (int, int) indices = (0, 0);

            Assert.Throws<AnyIntegerException>(() => DataParser.List_IntCSV(data, indices, nameof(List_IntCSV_WithInput_SingleValue_Alphabetical), false));
        }

        [Test]
        public void List_IntCSV_WithInput_SingleValue_Alphabetical_Positive()
        {
            IEnumerable<IEnumerable<string>> data = new string[][]
            {
                new string[]{ "a" }
            };
            (int, int) indices = (0, 0);

            Assert.Throws<PositiveIntegerException>(() => DataParser.List_IntCSV(data, indices, nameof(List_IntCSV_WithInput_SingleValue_Alphabetical_Positive), true));
        }

        [Test]
        public void List_IntCSV_WithInput_SingleValue()
        {
            IEnumerable<IEnumerable<string>> data = new string[][]
            {
                new string[]{ "-1" }
            };
            (int, int) indices = (0, 0);

            List<int> values = DataParser.List_IntCSV(data, indices, nameof(List_IntCSV_WithInput_SingleValue), false);

            List<int> expected = new List<int>() { -1 };
            Assert.That(values, Is.EqualTo(expected));
        }

        [Test]
        public void List_IntCSV_WithInput_SingleValue_Positive()
        {
            IEnumerable<IEnumerable<string>> data = new string[][]
            {
                new string[]{ "-1" }
            };
            (int, int) indices = (0, 0);

            Assert.Throws<PositiveIntegerException>(() => DataParser.List_IntCSV(data, indices, nameof(List_IntCSV_WithInput_SingleValue_Positive), true));
        }

        [Test]
        public void List_IntCSV_WithInput_MultipleValues()
        {
            IEnumerable<IEnumerable<string>> data = new string[][]
            {
                new string[]{ "1,2" }
            };
            (int, int) indices = (0, 0);

            List<int> values = DataParser.List_IntCSV(data, indices, nameof(List_IntCSV_WithInput_MultipleValues), false);

            List<int> expected = new List<int>() { 1, 2 };
            Assert.That(values, Is.EqualTo(expected));
        }

        [Test]
        public void List_IntCSV_WithInput_MultipleValues_MixedSign()
        {
            IEnumerable<IEnumerable<string>> data = new string[][]
            {
                new string[]{ "1,-1" }
            };
            (int, int) indices = (0, 0);

            List<int> values = DataParser.List_IntCSV(data, indices, nameof(List_IntCSV_WithInput_MultipleValues_MixedSign), false);

            List<int> expected = new List<int>() { 1, -1 };
            Assert.That(values, Is.EqualTo(expected));
        }

        [Test]
        public void List_IntCSV_WithInput_MultipleValues_Positive()
        {
            IEnumerable<IEnumerable<string>> data = new string[][]
            {
                new string[]{ "1,-1" }
            };
            (int, int) indices = (0, 0);

            Assert.Throws<PositiveIntegerException>(() => DataParser.List_IntCSV(data, indices, nameof(List_IntCSV_WithInput_MultipleValues_Positive), true));
        }

        [Test]
        public void List_IntCSV_WithInput_MultipleValues_WithAlphabetical()
        {
            IEnumerable<IEnumerable<string>> data = new string[][]
            {
                new string[]{ "1,-1,a" }
            };
            (int, int) indices = (0, 0);

            Assert.Throws<AnyIntegerException>(() => DataParser.List_IntCSV(data, indices, nameof(List_IntCSV_WithInput_MultipleValues_WithAlphabetical), false));
        }

        [Test]
        public void List_IntCSV_WithInput_MultipleValues_WithAlphabetical_Positive()
        {
            IEnumerable<IEnumerable<string>> data = new string[][]
            {
                new string[]{ "1,2,a" }
            };
            (int, int) indices = (0, 0);

            Assert.Throws<PositiveIntegerException>(() => DataParser.List_IntCSV(data, indices, nameof(List_IntCSV_WithInput_MultipleValues_WithAlphabetical_Positive), true));
        }

        #endregion List_IntCSV
    }
}
