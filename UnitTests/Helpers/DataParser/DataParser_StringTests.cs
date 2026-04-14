using RedditEmblemAPI.Helpers;
using RedditEmblemAPI.Models.Exceptions.Validation;

namespace UnitTests.Helpers
{
    public class DataParser_StringTests
    {
        #region String

        [Test]
        public void String_MultiSet_WithInput_Null()
        {
            IEnumerable<IEnumerable<string>> data = null;
            (int, int) indices = (0, 0);

            Assert.Throws<RequiredValueNotProvidedException>(() => DataParser.String(data, indices, nameof(String_MultiSet_WithInput_Null)));
        }

        [Test]
        public void String_MultiSet_WithInput_IndexOutOfBounds()
        {
            IEnumerable<IEnumerable<string>> data = new List<List<string>>();
            (int, int) indices = (0, 0);

            Assert.Throws<RequiredValueNotProvidedException>(() => DataParser.String(data, indices, nameof(String_MultiSet_WithInput_IndexOutOfBounds)));
        }

        [TestCase(0, 0, "This")]
        [TestCase(0, 1, "is")]
        [TestCase(1, 0, "my")]
        [TestCase(1, 1, "unit")]
        [TestCase(1, 2, "testing")]
        [TestCase(2, 0, "string")]
        [TestCase(2, 1, "data")]
        public void String_MultiSet_WithInput_ValidInputs(int setIndex, int cellIndex, string expected)
        {
            IEnumerable<IEnumerable<string>> data = new List<List<string>>()
            {
                new List<string>(){ "This", "is ", " " },
                new List<string>(){ " my ", "unit", "testing" },
                new List<string>(){ "string", "data", "" }
            };
            (int, int) indices = (setIndex, cellIndex);

            string actual = DataParser.String(data, indices, nameof(String_MultiSet_WithInput_ValidInputs));

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void String_WithInput_Null()
        {
            IEnumerable<string> data = null;
            int index = 0;

            Assert.Throws<RequiredValueNotProvidedException>(() => DataParser.String(data, index, nameof(String_WithInput_Null)));
        }

        [Test]
        public void String_WithInput_IndexOutOfBounds()
        {
            IEnumerable<string> data = new List<string>();
            int index = 0;

            Assert.Throws<RequiredValueNotProvidedException>(() => DataParser.String(data, index, nameof(String_WithInput_IndexOutOfBounds)));
        }

        [TestCase("")]
        [TestCase("   ")]
        public void String_WithInput_InvalidInputs(string input)
        {
            IEnumerable<string> data = new List<string>() { input };
            int index = 0;

            Assert.Throws<RequiredValueNotProvidedException>(() => DataParser.String(data, index, nameof(String_WithInput_InvalidInputs)));
        }

        [TestCase("123")]
        [TestCase("test")]
        public void String_WithInput_ValidInputs(string input)
        {
            IEnumerable<string> data = new List<string>() { input };
            int index = 0;

            string value = DataParser.String(data, index, nameof(String_WithInput_ValidInputs));

            Assert.That(value, Is.EqualTo(input));
        }


        [Test]
        public void String_WithInput_TrimWhitespace()
        {
            IEnumerable<string> data = new List<string>() { " test " };
            int index = 0;

            string value = DataParser.String(data, index, nameof(String_WithInput_TrimWhitespace));

            Assert.That(value, Is.EqualTo("test"));
        }

        #endregion String

        #region OptionalString

        [Test]
        public void OptionalString_MultiSet_WithInput_Null()
        {
            IEnumerable<IEnumerable<string>> data = null;
            (int, int) indices = (0, 0);

            string value = DataParser.OptionalString(data, indices, nameof(OptionalString_MultiSet_WithInput_Null));

            Assert.That(value, Is.Empty);
        }

        [Test]
        public void OptionalString_MultiSet_WithInput_IndexOutOfBounds()
        {
            IEnumerable<IEnumerable<string>> data = new List<List<string>>();
            (int, int) indices = (0, 0);

            string value = DataParser.OptionalString(data, indices, nameof(OptionalString_MultiSet_WithInput_IndexOutOfBounds));

            Assert.That(value, Is.Empty);
        }

        [TestCase(0, 0, "This")]
        [TestCase(0, 1, "is")]
        [TestCase(0, 2, "")]
        [TestCase(1, 0, "my")]
        [TestCase(1, 1, "unit")]
        [TestCase(1, 2, "testing")]
        [TestCase(2, 0, "string")]
        [TestCase(2, 1, "data")]
        [TestCase(2, 2, "")]
        public void OptionalString_MultiSet_WithInput_ValidInputs(int setIndex, int cellIndex, string expected)
        {
            IEnumerable<IEnumerable<string>> data = new List<List<string>>()
            {
                new List<string>(){ "This", "is ", " " },
                new List<string>(){ " my ", "unit", "testing" },
                new List<string>(){ "string", "data", "" }
            };
            (int, int) indices = (setIndex, cellIndex);

            string value = DataParser.OptionalString(data, indices, nameof(OptionalString_MultiSet_WithInput_ValidInputs));

            Assert.That(value, Is.EqualTo(expected));
        }

        [Test]
        public void OptionalString_WithInput_Null()
        {
            IEnumerable<string> data = null;
            int index = 0;

            string value = DataParser.OptionalString(data, index, nameof(OptionalString_WithInput_Null));

            Assert.That(value, Is.Empty);
        }

        [Test]
        public void OptionalString_WithInput_IndexOutOfBounds()
        {
            IEnumerable<string> data = new List<string>();
            int index = 0;

            string value = DataParser.OptionalString(data, index, nameof(OptionalString_WithInput_IndexOutOfBounds));

            Assert.That(value, Is.Empty);
        }

        [TestCase("")]
        [TestCase("   ")]
        public void OptionalString_EmptyStringInputs(string input)
        {
            IEnumerable<string> data = new List<string>() { input };
            int index = 0;

            string value = DataParser.OptionalString(data, index, nameof(OptionalString_EmptyStringInputs));

            Assert.That(value, Is.Empty);
        }

        [TestCase("test")]
        public void OptionalString_ValidInputs(string input)
        {
            IEnumerable<string> data = new List<string>() { input };
            int index = 0;

            string value = DataParser.OptionalString(data, index, nameof(OptionalString_ValidInputs));

            Assert.That(value, Is.EqualTo(input));
        }

        [Test]
        public void OptionalString_WithInput_TrimWhitespace()
        {
            IEnumerable<string> data = new List<string>() { " test " };
            int index = 0;

            string value = DataParser.OptionalString(data, index, nameof(OptionalString_WithInput_TrimWhitespace));

            Assert.That(value, Is.EqualTo("test"));
        }

        #endregion OptionalString

        #region String_URL

        [Test]
        public void String_URL_MultiSet_Null()
        {
            IEnumerable<IEnumerable<string>> data = null;
            (int, int) indices = (0, 0);

            Assert.Throws<RequiredValueNotProvidedException>(() => DataParser.String_URL(data, indices, nameof(String_URL_MultiSet_Null)));
        }

        [Test]
        public void String_URL_MultiSet_IndexOutOfBounds()
        {
            IEnumerable<IEnumerable<string>> data = new List<List<string>>();
            (int, int) indices = (0, 0);

            Assert.Throws<RequiredValueNotProvidedException>(() => DataParser.String_URL(data, indices, nameof(String_URL_MultiSet_IndexOutOfBounds)));
        }

        [TestCase(0, 1, "http://www.google.com")]
        [TestCase(1, 0, "https://www.google.com")]
        public void String_URL_MultiSet_ValidInputs(int setIndex, int cellIndex, string expected)
        {
            IEnumerable<IEnumerable<string>> data = new List<List<string>>()
            {
                new List<string>(){ "alert(\"Hacked!\")", "http://www.google.com", "" },
                new List<string>(){ "https://www.google.com", "google.com", " " }
            };
            (int, int) indices = (setIndex, cellIndex);

            string actual = DataParser.String_URL(data, indices, nameof(String_URL_MultiSet_ValidInputs));

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void String_URL_Null()
        {
            IEnumerable<string> data = null;
            int index = 0;

            Assert.Throws<RequiredValueNotProvidedException>(() => DataParser.String_URL(data, index, nameof(String_URL_Null)));
        }

        [Test]
        public void String_URL_IndexOutOfBounds()
        {
            IEnumerable<string> data = new List<string>();
            int index = 0;

            Assert.Throws<RequiredValueNotProvidedException>(() => DataParser.String_URL(data, index, nameof(String_URL_IndexOutOfBounds)));
        }

        [TestCase("")]
        [TestCase("   ")]
        public void String_URL_EmptyStringInputs(string input)
        {
            IEnumerable<string> data = new List<string>() { input };
            int index = 0;

            Assert.Throws<RequiredValueNotProvidedException>(() => DataParser.String_URL(data, index, nameof(String_URL_EmptyStringInputs)));
        }

        [TestCase("123")]
        [TestCase("test")]
        [TestCase("alert(\"Hacked!\")")] //Javascript
        [TestCase("google.com")] //partial URL
        public void String_URL_InvalidInputs(string input)
        {
            IEnumerable<string> data = new List<string>() { input };
            int index = 0;

            Assert.Throws<URLException>(() => DataParser.String_URL(data, index, nameof(String_URL_InvalidInputs)));
        }

        [TestCase("http://www.google.com")] //http
        [TestCase("https://www.google.com")] //https
        [TestCase("https://www.google.com/test_image.png")] //image URL
        public void String_URL_ValidInputs(string input)
        {
            IEnumerable<string> data = new List<string>() { input };
            int index = 0;

            string value = DataParser.String_URL(data, index, nameof(String_URL_ValidInputs));

            Assert.That(value, Is.EqualTo(input));
        }

        #endregion String_URL

        #region OptionalString_URL

        [Test]
        public void OptionalString_URL_MultiSet_Null()
        {
            IEnumerable<IEnumerable<string>> data = null;
            (int, int) indices = (0, 0);

            string actual = DataParser.OptionalString_URL(data, indices, nameof(OptionalString_URL_MultiSet_Null));

            Assert.That(actual, Is.Empty);
        }

        [Test]
        public void OptionalString_URL_MultiSet_IndexOutOfBounds()
        {
            IEnumerable<IEnumerable<string>> data = new List<List<string>>();
            (int, int) indices = (0, 0);

            string actual = DataParser.OptionalString_URL(data, indices, nameof(OptionalString_URL_MultiSet_IndexOutOfBounds));

            Assert.That(actual, Is.Empty);
        }

        [TestCase(0, 1, "http://www.google.com")]
        [TestCase(0, 2, "")]
        [TestCase(1, 0, "https://www.google.com")]
        [TestCase(1, 2, "")]
        public void OptionalString_URL_MultiSet_ValidInputs(int setIndex, int cellIndex, string expected)
        {
            IEnumerable<IEnumerable<string>> data = new List<List<string>>()
            {
                new List<string>(){ "alert(\"Hacked!\")", "http://www.google.com", "" },
                new List<string>(){ "https://www.google.com", "google.com", " " }
            };
            (int, int) indices = (setIndex, cellIndex);

            string actual = DataParser.OptionalString_URL(data, indices, nameof(OptionalString_URL_MultiSet_IndexOutOfBounds));

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void OptionalString_URL_Null()
        {
            IEnumerable<string> data = null;
            int index = 0;

            string value = DataParser.OptionalString_URL(data, index, nameof(OptionalString_URL_Null));

            Assert.That(value, Is.Empty);
        }

        [Test]
        public void OptionalString_URL_IndexOutOfBounds()
        {
            IEnumerable<string> data = new List<string>();
            int index = 0;

            string value = DataParser.OptionalString_URL(data, index, nameof(OptionalString_URL_IndexOutOfBounds));

            Assert.That(value, Is.Empty);
        }

        [TestCase("")]
        [TestCase("   ")]
        public void OptionalString_URL_EmptyStringInputs(string input)
        {
            IEnumerable<string> data = new List<string>() { input };
            int index = 0;

            string value = DataParser.OptionalString_URL(data, index, nameof(OptionalString_URL_EmptyStringInputs));

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

            Assert.Throws<URLException>(() => DataParser.OptionalString_URL(data, index, nameof(OptionalString_URL_InvalidInputs)));
        }

        [TestCase("http://www.google.com")] //http
        [TestCase("https://www.google.com")] //https
        [TestCase("https://www.google.com/test_image.png")] //image URL
        public void OptionalString_URL_ValidInputs(string input)
        {
            IEnumerable<string> data = new List<string>() { input };
            int index = 0;

            string value = DataParser.OptionalString_URL(data, index, nameof(OptionalString_URL_ValidInputs));

            Assert.That(value, Is.EqualTo(input));
        }

        #endregion OptionalString_URL

        #region String_HexCode

        [Test]
        public void String_HexCode_MultiSet_Null()
        {
            IEnumerable<IEnumerable<string>> data = null;
            (int, int) indices = (0, 0);

            Assert.Throws<RequiredValueNotProvidedException>(() => DataParser.String_HexCode(data, indices, nameof(String_HexCode_MultiSet_Null)));
        }

        [Test]
        public void String_HexCode_MultiSet_IndexOutOfBounds()
        {
            IEnumerable<IEnumerable<string>> data = new List<List<string>>();
            (int, int) indices = (0, 0);

            Assert.Throws<RequiredValueNotProvidedException>(() => DataParser.String_HexCode(data, indices, nameof(String_HexCode_MultiSet_IndexOutOfBounds)));
        }

        [TestCase(0, 2, "#1A2B3C")]
        [TestCase(1, 2, "#1A2B3C")]
        public void String_HexCode_MultiSet_ValidInputs(int setIndex, int cellIndex, string expected)
        {
            IEnumerable<IEnumerable<string>> data = new List<List<string>>()
            {
                new List<string>(){ "", "FFFFFFFF", "1A2B3C" },
                new List<string>(){ "   ", "FFFFFF", "#1A2B3C" }
            };
            (int, int) indices = (setIndex, cellIndex);

            string actual = DataParser.String_HexCode(data, indices, nameof(String_HexCode_MultiSet_ValidInputs));

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void String_HexCode_Null()
        {
            IEnumerable<string> data = null;
            int index = 0;

            Assert.Throws<RequiredValueNotProvidedException>(() => DataParser.String_HexCode(data, index, nameof(String_HexCode_Null)));
        }

        [Test]
        public void String_HexCode_IndexOutOfBounds()
        {
            IEnumerable<string> data = new List<string>();
            int index = 0;

            Assert.Throws<RequiredValueNotProvidedException>(() => DataParser.String_HexCode(data, index, nameof(String_HexCode_IndexOutOfBounds)));
        }

        [TestCase("")]
        [TestCase("   ")]
        public void String_HexCode_EmptyStringInputs(string input)
        {
            IEnumerable<string> data = new List<string>() { input };
            int index = 0;

            Assert.Throws<RequiredValueNotProvidedException>(() => DataParser.String_HexCode(data, index, nameof(String_HexCode_EmptyStringInputs)));
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

            Assert.Throws<HexException>(() => DataParser.String_HexCode(data, index, nameof(String_HexCode_InvalidInputs)));
        }

        [TestCase("1A2B3C", "#1A2B3C")]
        [TestCase("#1A2B3C", "#1A2B3C")]
        [TestCase(" #1A2B3C ", "#1A2B3C")]
        public void String_HexCode_ValidInputs(string input, string expected)
        {
            IEnumerable<string> data = new List<string>() { input };
            int index = 0;

            string value = DataParser.String_HexCode(data, index, nameof(String_HexCode_ValidInputs));

            Assert.That(value, Is.EqualTo(expected));
        }

        #endregion String_HexCode

        #region OptionalString_HexCode

        [Test]
        public void OptionalString_HexCode_MultiSet_Null()
        {
            IEnumerable<IEnumerable<string>> data = null;
            (int, int) indices = (0, 0);

            string value = DataParser.OptionalString_HexCode(data, indices, nameof(OptionalString_HexCode_MultiSet_Null));

            Assert.That(value, Is.Empty);
        }

        [Test]
        public void OptionalString_HexCode_MultiSet_IndexOutOfBounds()
        {
            IEnumerable<IEnumerable<string>> data = new List<List<string>>();
            (int, int) indices = (0, 0);

            string value = DataParser.OptionalString_HexCode(data, indices, nameof(OptionalString_HexCode_MultiSet_IndexOutOfBounds));

            Assert.That(value, Is.Empty);
        }

        [TestCase(0, 0, "")]
        [TestCase(0, 2, "#1A2B3C")]
        [TestCase(1, 0, "")]
        [TestCase(1, 2, "#1A2B3C")]
        public void OptionalString_HexCode_MultiSet_ValidInputs(int setIndex, int cellIndex, string expected)
        {
            IEnumerable<IEnumerable<string>> data = new List<List<string>>()
            {
                new List<string>(){ "", "FFFFFFFF", "1A2B3C" },
                new List<string>(){ "   ", "FFFFFF", "#1A2B3C" }
            };
            (int, int) indices = (setIndex, cellIndex);

            string value = DataParser.OptionalString_HexCode(data, indices, nameof(OptionalString_HexCode_MultiSet_ValidInputs));

            Assert.That(value, Is.EqualTo(expected));
        }

        [Test]
        public void OptionalString_HexCode_Null()
        {
            IEnumerable<string> data = null;
            int index = 0;

            string value = DataParser.OptionalString_HexCode(data, index, nameof(OptionalString_HexCode_Null));

            Assert.That(value, Is.Empty);
        }

        [Test]
        public void OptionalString_HexCode_IndexOutOfBounds()
        {
            IEnumerable<string> data = new List<string>();
            int index = 0;

            string value = DataParser.OptionalString_HexCode(data, index, nameof(OptionalString_HexCode_IndexOutOfBounds));

            Assert.That(value, Is.Empty);
        }

        [TestCase("")]
        [TestCase("   ")]
        public void OptionalString_HexCode_EmptyStringInputs(string input)
        {
            IEnumerable<string> data = new List<string>() { input };
            int index = 0;

            string value = DataParser.OptionalString_HexCode(data, index, nameof(OptionalString_HexCode_EmptyStringInputs));

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

            Assert.Throws<HexException>(() => DataParser.OptionalString_HexCode(data, index, nameof(OptionalString_HexCode_InvalidInputs)));
        }

        [TestCase("1A2B3C", "#1A2B3C")]
        [TestCase("#1A2B3C", "#1A2B3C")]
        [TestCase(" #1A2B3C ", "#1A2B3C")]
        public void OptionalString_HexCode_ValidInputs(string input, string expected)
        {
            IEnumerable<string> data = new List<string>() { input };
            int index = 0;

            string value = DataParser.OptionalString_HexCode(data, index, nameof(OptionalString_HexCode_ValidInputs));
            
            Assert.That(value, Is.EqualTo(expected));
        }

        #endregion OptionalString_HexCode

    }
}
