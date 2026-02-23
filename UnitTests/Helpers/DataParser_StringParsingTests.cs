using RedditEmblemAPI.Helpers;
using RedditEmblemAPI.Models.Exceptions.Validation;

namespace UnitTests.Helpers
{
    public class DataParser_StringParsingTests
    {
        #region String

        [Test]
        public void String_WithInput_IndexOutOfBounds()
        {
            IEnumerable<string> data = new List<string>();
            int index = 0;

            Assert.Throws<RequiredValueNotProvidedException>(() => DataParser.String(data, index, "String_WithInput_IndexOutOfBounds"));
        }

        [TestCase("")]
        [TestCase("   ")]
        public void String_WithInput_InvalidInputs(string input)
        {
            IEnumerable<string> data = new List<string>() { input };
            int index = 0;

            Assert.Throws<RequiredValueNotProvidedException>(() => DataParser.String(data, index, "String_WithInput_InvalidInputs"));
        }

        [TestCase("123")]
        [TestCase("test")]
        public void String_WithInput_ValidInputs(string input)
        {
            IEnumerable<string> data = new List<string>() { input };
            int index = 0;

            string value = DataParser.String(data, index, "String_WithInput_ValidInputs");

            Assert.That(value, Is.EqualTo(input));
        }


        [Test]
        public void String_WithInput_TrimWhitespace()
        {
            IEnumerable<string> data = new List<string>() { " test " };
            int index = 0;

            string value = DataParser.String(data, index, "String_WithInput_TrimWhitespace");

            Assert.That(value, Is.EqualTo("test"));
        }

        #endregion String

        #region OptionalString

        [Test]
        public void OptionalString_WithInput_IndexOutOfBounds()
        {
            IEnumerable<string> data = new List<string>();
            int index = 0;

            string value = DataParser.OptionalString(data, index, "OptionalString_WithInput_IndexOutOfBounds");

            Assert.That(value, Is.Empty);
        }

        [TestCase("")]
        [TestCase("   ")]
        public void OptionalString_EmptyStringInputs(string input)
        {
            IEnumerable<string> data = new List<string>() { input };
            int index = 0;

            string value = DataParser.OptionalString(data, index, "OptionalString_EmptyStringInputs");

            Assert.That(value, Is.Empty);
        }

        [TestCase("test")]
        public void OptionalString_ValidInputs(string input)
        {
            IEnumerable<string> data = new List<string>() { input };
            int index = 0;

            string value = DataParser.OptionalString(data, index, "OptionalString_ValidInputs");

            Assert.That(value, Is.EqualTo(input));
        }

        [Test]
        public void OptionalString_WithInput_TrimWhitespace()
        {
            IEnumerable<string> data = new List<string>() { " test " };
            int index = 0;

            string value = DataParser.OptionalString(data, index, "OptionalString_WithInput_TrimWhitespace");

            Assert.That(value, Is.EqualTo("test"));
        }

        #endregion OptionalString

        #region String_URL

        [Test]
        public void String_URL_IndexOutOfBounds()
        {
            IEnumerable<string> data = new List<string>();
            int index = 0;

            Assert.Throws<RequiredValueNotProvidedException>(() => DataParser.String_URL(data, index, "String_URL_IndexOutOfBounds"));
        }

        [TestCase("")]
        [TestCase("   ")]
        public void String_URL_EmptyStringInputs(string input)
        {
            IEnumerable<string> data = new List<string>() { input };
            int index = 0;

            Assert.Throws<RequiredValueNotProvidedException>(() => DataParser.String_URL(data, index, "String_URL_EmptyStringInputs"));
        }

        [TestCase("123")]
        [TestCase("test")]
        [TestCase("alert(\"Hacked!\")")] //Javascript
        [TestCase("google.com")] //partial URL
        public void String_URL_InvalidInputs(string input)
        {
            IEnumerable<string> data = new List<string>() { input };
            int index = 0;

            Assert.Throws<URLException>(() => DataParser.String_URL(data, index, "String_URL_InvalidInputs"));
        }

        [TestCase("http://www.google.com")] //http
        [TestCase("https://www.google.com")] //https
        [TestCase("https://www.google.com/test_image.png")] //image URL
        public void String_URL_ValidInputs(string input)
        {
            IEnumerable<string> data = new List<string>() { input };
            int index = 0;

            string value = DataParser.String_URL(data, index, "String_URL_ValidInputs");

            Assert.That(value, Is.EqualTo(input));
        }

        #endregion String_URL

        #region OptionalString_URL

        [Test]
        public void OptionalString_URL_IndexOutOfBounds()
        {
            IEnumerable<string> data = new List<string>();
            int index = 0;

            string value = DataParser.OptionalString_URL(data, index, "OptionalString_URL_IndexOutOfBounds");

            Assert.That(value, Is.Empty);
        }

        [TestCase("")]
        [TestCase("   ")]
        public void OptionalString_URL_EmptyStringInputs(string input)
        {
            IEnumerable<string> data = new List<string>() { input };
            int index = 0;

            string value = DataParser.OptionalString_URL(data, index, "OptionalString_URL_EmptyStringInputs");

            Assert.That(value, Is.Empty);
        }

        [TestCase("123")]
        [TestCase("test")]
        [TestCase("alert(\"Hacked!\")")] //Javascript
        [TestCase("google.com")] //partial URL
        public void OptionalString_URL_InvalidInputs(string input)
        {
            IEnumerable<string> data = new List<string>() { input };
            int index = 0;

            Assert.Throws<URLException>(() => DataParser.OptionalString_URL(data, index, "OptionalString_URL_InvalidInputs"));
        }

        [TestCase("http://www.google.com")] //http
        [TestCase("https://www.google.com")] //https
        [TestCase("https://www.google.com/test_image.png")] //image URL
        public void OptionalString_URL_ValidInputs(string input)
        {
            IEnumerable<string> data = new List<string>() { input };
            int index = 0;

            string value = DataParser.OptionalString_URL(data, index, "OptionalString_URL_ValidInputs");

            Assert.That(value, Is.EqualTo(input));
        }

        #endregion OptionalString_URL

        #region String_HexCode

        [Test]
        public void String_HexCode_IndexOutOfBounds()
        {
            IEnumerable<string> data = new List<string>();
            int index = 0;

            Assert.Throws<RequiredValueNotProvidedException>(() => DataParser.String_HexCode(data, index, "String_HexCode_IndexOutOfBounds"));
        }

        [TestCase("")]
        [TestCase("   ")]
        public void String_HexCode_EmptyStringInputs(string input)
        {
            IEnumerable<string> data = new List<string>() { input };
            int index = 0;

            Assert.Throws<RequiredValueNotProvidedException>(() => DataParser.String_HexCode(data, index, "String_HexCode_EmptyStringInputs"));
        }

        [TestCase("123")]
        [TestCase("test")]
        [TestCase("FFFFFFFF")] //too many characters
        [TestCase("ABCDEG")] //illegal character
        [TestCase("?ABCDEF")] //illegal leading character
        [TestCase("##1A2B3C")] //double hash
        public void String_HexCode_InvalidInputs(string input)
        {
            IEnumerable<string> data = new List<string>() { input };
            int index = 0;

            Assert.Throws<HexException>(() => DataParser.String_HexCode(data, index, "String_HexCode_InvalidInputs"));
        }

        [TestCase("1A2B3C", "#1A2B3C")]
        [TestCase("#1A2B3C", "#1A2B3C")]
        [TestCase(" #1A2B3C ", "#1A2B3C")]
        public void String_HexCode_ValidInputs(string input, string expected)
        {
            IEnumerable<string> data = new List<string>() { input };
            int index = 0;

            string value = DataParser.String_HexCode(data, index, "String_HexCode_ValidInputs");

            Assert.That(value, Is.EqualTo(expected));
        }

        #endregion String_HexCode

        #region OptionalString_HexCode

        [Test]
        public void OptionalString_HexCode_IndexOutOfBounds()
        {
            IEnumerable<string> data = new List<string>();
            int index = 0;

            string value = DataParser.OptionalString_HexCode(data, index, "OptionalString_HexCode_IndexOutOfBounds");

            Assert.That(value, Is.Empty);
        }

        [TestCase("")]
        [TestCase("   ")]
        public void OptionalString_HexCode_EmptyStringInputs(string input)
        {
            IEnumerable<string> data = new List<string>() { input };
            int index = 0;

            string value = DataParser.OptionalString_HexCode(data, index, "OptionalString_HexCode_EmptyStringInputs");

            Assert.That(value, Is.Empty);
        }

        [TestCase("123")]
        [TestCase("test")]
        [TestCase("FFFFFFFF")] //too many characters
        [TestCase("ABCDEG")] //illegal character
        [TestCase("?ABCDEF")] //illegal leading character
        [TestCase("##1A2B3C")] //double hash
        public void OptionalString_HexCode_InvalidInputs(string input)
        {
            IEnumerable<string> data = new List<string>() { input };
            int index = 0;

            Assert.Throws<HexException>(() => DataParser.OptionalString_HexCode(data, index, "OptionalString_HexCode_InvalidInputs"));
        }

        [TestCase("1A2B3C", "#1A2B3C")]
        [TestCase("#1A2B3C", "#1A2B3C")]
        [TestCase(" #1A2B3C ", "#1A2B3C")]
        public void OptionalString_HexCode_ValidInputs(string input, string expected)
        {
            IEnumerable<string> data = new List<string>() { input };
            int index = 0;

            string value = DataParser.OptionalString_HexCode(data, index, "OptionalString_HexCode_ValidInputs");
            
            Assert.That(value, Is.EqualTo(expected));
        }

        #endregion OptionalString_HexCode

    }
}
