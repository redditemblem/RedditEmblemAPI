using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Services.Helpers;

namespace UnitTests.Helpers
{
    [TestClass]
    public class DataParser_StringParsingTests
    {
        #region String

        [TestMethod]
        public void String_WithInput_Null()
        {
            List<string> data = new List<string>() { };
            int index = 0;

            Assert.ThrowsException<RequiredValueNotProvidedException>(() => DataParser.String(data, index, "String_WithInput_Null"));
        }

        [TestMethod]
        public void String_WithInput_EmptyString()
        {
            List<string> data = new List<string>() { string.Empty };
            int index = 0;

            Assert.ThrowsException<RequiredValueNotProvidedException>(() => DataParser.String(data, index, "String_WithInput_EmptyString"));
        }

        [TestMethod]
        public void String_WithInput_Whitespace()
        {
            List<string> data = new List<string>() { UnitTestConsts.WHITESPACE_STRING };
            int index = 0;

            Assert.ThrowsException<RequiredValueNotProvidedException>(() => DataParser.String(data, index, "String_WithInput_Whitespace"));
        }

        [TestMethod]
        public void String_WithInput_Numerical()
        {
            List<string> data = new List<string>() { "123" };
            int index = 0;

            string value = DataParser.String(data, index, "String_WithInput_Numerical");
            Assert.AreEqual<string>("123", value);
        }

        [TestMethod]
        public void String_WithInput_Alphabetical()
        {
            List<string> data = new List<string>() { "test" };
            int index = 0;

            string value = DataParser.String(data, index, "String_WithInput_Alphabetical");
            Assert.AreEqual<string>("test", value);
        }

        [TestMethod]
        public void String_WithInput_Alphabetical_And_Whitespace()
        {
            List<string> data = new List<string>() { " test " };
            int index = 0;

            string value = DataParser.String(data, index, "String_WithInput_Alphabetical_And_Whitespace");
            Assert.AreEqual<string>("test", value);
        }

        #endregion String

        #region OptionalString

        [TestMethod]
        public void OptionalString_WithInput_Null()
        {
            List<string> data = new List<string>() { };
            int index = 0;

            string value = DataParser.OptionalString(data, index, "OptionalString_WithInput_Null");
            Assert.AreEqual<string>(string.Empty, value);
        }

        [TestMethod]
        public void OptionalString_WithInput_EmptyString()
        {
            List<string> data = new List<string>() { string.Empty };
            int index = 0;

            string value = DataParser.OptionalString(data, index, "OptionalString_WithInput_EmptyString");
            Assert.AreEqual<string>(string.Empty, value);
        }

        [TestMethod]
        public void OptionalString_WithInput_Whitespace()
        {
            List<string> data = new List<string>() { UnitTestConsts.WHITESPACE_STRING };
            int index = 0;

            string value = DataParser.OptionalString(data, index, "OptionalString_WithInput_Whitespace");
            Assert.AreEqual<string>(string.Empty, value);
        }

        [TestMethod]
        public void OptionalString_WithInput_Alphabetical()
        {
            List<string> data = new List<string>() { "test" };
            int index = 0;

            string value = DataParser.OptionalString(data, index, "OptionalString_WithInput_Alphabetical");
            Assert.AreEqual<string>("test", value);
        }

        [TestMethod]
        public void OptionalString_WithInput_Alphabetical_And_Whitespace()
        {
            List<string> data = new List<string>() { " test " };
            int index = 0;

            string value = DataParser.OptionalString(data, index, "OptionalString_WithInput_Alphabetical_And_Whitespace");
            Assert.AreEqual<string>("test", value);
        }

        #endregion OptionalString

        #region String_URL

        [TestMethod]
        public void String_URL_WithInput_Null()
        {
            List<string> data = new List<string>() { };
            int index = 0;

            Assert.ThrowsException<RequiredValueNotProvidedException>(() => DataParser.String_URL(data, index, "String_URL_WithInput_Null"));
        }

        [TestMethod]
        public void String_URL_WithInput_EmptyString()
        {
            List<string> data = new List<string>() { string.Empty };
            int index = 0;

            Assert.ThrowsException<RequiredValueNotProvidedException>(() => DataParser.String_URL(data, index, "String_URL_WithInput_EmptyString"));
        }

        [TestMethod]
        public void String_URL_WithInput_Whitespace()
        {
            List<string> data = new List<string>() { UnitTestConsts.WHITESPACE_STRING };
            int index = 0;

            Assert.ThrowsException<RequiredValueNotProvidedException>(() => DataParser.String_URL(data, index, "String_URL_WithInput_Whitespace"));
        }

        [TestMethod]
        public void String_URL_WithInput_Numerical()
        {
            List<string> data = new List<string>() { "123" };
            int index = 0;

            Assert.ThrowsException<URLException>(() => DataParser.String_URL(data, index, "String_URL_WithInput_Numerical"));
        }

        [TestMethod]
        public void String_URL_WithInput_Alphabetical()
        {
            List<string> data = new List<string>() { "test" };
            int index = 0;

            Assert.ThrowsException<URLException>(() => DataParser.String_URL(data, index, "String_URL_WithInput_Alphabetical"));
        }

        [TestMethod]
        public void String_URL_WithInput_Javascript()
        {
            List<string> data = new List<string>() { "alert(\"Hacked!\")" };
            int index = 0;

            Assert.ThrowsException<URLException>(() => DataParser.String_URL(data, index, "String_URL_WithInput_Javascript"));
        }

        [TestMethod]
        public void String_URL_WithInput_PartialURL()
        {
            List<string> data = new List<string>() { "google.com" };
            int index = 0;

            Assert.ThrowsException<URLException>(() => DataParser.String_URL(data, index, "String_URL_WithInput_PartialURL"));
        }

        [TestMethod]
        public void String_URL_WithInput_HTTP_URL()
        {
            List<string> data = new List<string>() { "http://www.google.com" };
            int index = 0;

            string value = DataParser.String_URL(data, index, "String_URL_WithInput_HTTP_URL");
            Assert.AreEqual<string>("http://www.google.com", value);
        }

        [TestMethod]
        public void String_URL_WithInput_HTTPS_URL()
        {
            List<string> data = new List<string>() { "https://www.google.com" };
            int index = 0;

            string value = DataParser.String_URL(data, index, "String_URL_WithInput_HTTPS_URL");
            Assert.AreEqual<string>("https://www.google.com", value);
        }

        [TestMethod]
        public void String_URL_WithInput_ImageURL()
        {
            List<string> data = new List<string>() { "https://www.google.com/test_image.png" };
            int index = 0;

            string value = DataParser.String_URL(data, index, "String_URL_WithInput_ImageURL");
            Assert.AreEqual<string>("https://www.google.com/test_image.png", value);
        }

        #endregion String_URL

        #region OptionalString_URL

        [TestMethod]
        public void OptionalString_URL_WithInput_Null()
        {
            List<string> data = new List<string>() { };
            int index = 0;

            string value = DataParser.OptionalString_URL(data, index, "OptionalString_URL_WithInput_Null");
            Assert.AreEqual<string>(string.Empty, value);
        }

        [TestMethod]
        public void OptionalString_URL_WithInput_EmptyString()
        {
            List<string> data = new List<string>() { string.Empty };
            int index = 0;

            string value = DataParser.OptionalString_URL(data, index, "OptionalString_URL_WithInput_EmptyString");
            Assert.AreEqual<string>(string.Empty, value);
        }

        [TestMethod]
        public void OptionalString_URL_WithInput_Whitespace()
        {
            List<string> data = new List<string>() { UnitTestConsts.WHITESPACE_STRING };
            int index = 0;

            string value = DataParser.OptionalString_URL(data, index, "OptionalString_URL_WithInput_Whitespace");
            Assert.AreEqual<string>(string.Empty, value);
        }

        [TestMethod]
        public void OptionalString_URL_WithInput_Alphabetical()
        {
            List<string> data = new List<string>() { "test" };
            int index = 0;

            Assert.ThrowsException<URLException>(() => DataParser.OptionalString_URL(data, index, "OptionalString_URL_WithInput_Alphabetical"));
        }

        [TestMethod]
        public void OptionalString_URL_WithInput_Alphabetical_And_Whitespace()
        {
            List<string> data = new List<string>() { " test " };
            int index = 0;

            Assert.ThrowsException<URLException>(() => DataParser.OptionalString_URL(data, index, "OptionalString_URL_WithInput_Alphabetical_And_Whitespace"));
        }

        [TestMethod]
        public void OptionalString_URL_WithInput_ImageURL()
        {
            List<string> data = new List<string>() { "https://www.google.com/test_image.png" };
            int index = 0;

            string value = DataParser.OptionalString_URL(data, index, "OptionalString_URL_WithInput_ImageURL");
            Assert.AreEqual<string>("https://www.google.com/test_image.png", value);
        }

        #endregion OptionalString_URL

        #region String_HexCode

        [TestMethod]
        public void String_HexCode_WithInput_Null()
        {
            List<string> data = new List<string>() { };
            int index = 0;

            Assert.ThrowsException<RequiredValueNotProvidedException>(() => DataParser.String_HexCode(data, index, "String_HexCode_WithInput_Null"));
        }

        [TestMethod]
        public void String_HexCode_WithInput_EmptyString()
        {
            List<string> data = new List<string>() { string.Empty };
            int index = 0;

            Assert.ThrowsException<RequiredValueNotProvidedException>(() => DataParser.String_HexCode(data, index, "String_HexCode_WithInput_EmptyString"));
        }

        [TestMethod]
        public void String_HexCode_WithInput_Whitespace()
        {
            List<string> data = new List<string>() { UnitTestConsts.WHITESPACE_STRING };
            int index = 0;

            Assert.ThrowsException<RequiredValueNotProvidedException>(() => DataParser.String_HexCode(data, index, "String_HexCode_WithInput_Whitespace"));
        }

        [TestMethod]
        public void String_HexCode_WithInput_Numerical()
        {
            List<string> data = new List<string>() { "123" };
            int index = 0;

            Assert.ThrowsException<HexException>(() => DataParser.String_HexCode(data, index, "String_HexCode_WithInput_Numerical"));
        }

        [TestMethod]
        public void String_HexCode_WithInput_Alphabetical()
        {
            List<string> data = new List<string>() { "test" };
            int index = 0;

            Assert.ThrowsException<HexException>(() => DataParser.String_HexCode(data, index, "String_HexCode_WithInput_Alphabetical"));
        }

        [TestMethod]
        public void String_HexCode_WithInput_TooManyChars()
        {
            List<string> data = new List<string>() { "FFFFFFFF" };
            int index = 0;

            Assert.ThrowsException<HexException>(() => DataParser.String_HexCode(data, index, "String_HexCode_WithInput_TooManyChars"));
        }

        [TestMethod]
        public void String_HexCode_WithInput_InvalidChar()
        {
            List<string> data = new List<string>() { "ABCDEG" };
            int index = 0;

            Assert.ThrowsException<HexException>(() => DataParser.String_HexCode(data, index, "String_HexCode_WithInput_InvalidChar"));
        }

        [TestMethod]
        public void String_HexCode_WithInput_InvalidLeadingChar()
        {
            List<string> data = new List<string>() { "?ABCDEF" };
            int index = 0;

            Assert.ThrowsException<HexException>(() => DataParser.String_HexCode(data, index, "String_HexCode_WithInput_InvalidLeadingChar"));
        }

        [TestMethod]
        public void String_HexCode_WithInput_NoHash()
        {
            List<string> data = new List<string>() { "1A2B3C" };
            int index = 0;

            string value = DataParser.String_HexCode(data, index, "String_HexCode_WithInput_NoHash");
            Assert.AreEqual<string>("#1A2B3C", value);
        }

        [TestMethod]
        public void String_HexCode_WithInput_Hash()
        {
            List<string> data = new List<string>() { "#1A2B3C" };
            int index = 0;

            string value = DataParser.String_HexCode(data, index, "String_HexCode_WithInput_Hash");
            Assert.AreEqual<string>("#1A2B3C", value);
        }

        [TestMethod]
        public void String_HexCode_WithInput_DoubleHash()
        {
            List<string> data = new List<string>() { "##1A2B3C" };
            int index = 0;

            Assert.ThrowsException<HexException>(() => DataParser.String_HexCode(data, index, "String_HexCode_WithInput_DoubleHash"));
        }

        [TestMethod]
        public void String_HexCode_WithInput_Hash_And_Whitespace()
        {
            List<string> data = new List<string>() { " #1A2B3C " };
            int index = 0;

            string value = DataParser.String_HexCode(data, index, "String_HexCode_WithInput_Hash_And_Whitespace");
            Assert.AreEqual<string>("#1A2B3C", value);
        }

        #endregion String_HexCode

        #region OptionalString_HexCode

        [TestMethod]
        public void OptionalString_HexCode_WithInput_Null()
        {
            List<string> data = new List<string>() { };
            int index = 0;

            string value = DataParser.OptionalString_HexCode(data, index, "OptionalString_HexCode_WithInput_Null");
            Assert.AreEqual<string>(string.Empty, value);
        }

        [TestMethod]
        public void OptionalString_HexCode_WithInput_EmptyString()
        {
            List<string> data = new List<string>() { string.Empty };
            int index = 0;

            string value = DataParser.OptionalString_HexCode(data, index, "OptionalString_HexCode_WithInput_EmptyString");
            Assert.AreEqual<string>(string.Empty, value);
        }

        [TestMethod]
        public void OptionalString_HexCode_WithInput_Whitespace()
        {
            List<string> data = new List<string>() { UnitTestConsts.WHITESPACE_STRING };
            int index = 0;

            string value = DataParser.OptionalString_HexCode(data, index, "OptionalString_HexCode_WithInput_Whitespace");
            Assert.AreEqual<string>(string.Empty, value);
        }

        [TestMethod]
        public void OptionalString_HexCode_WithInput_Numerical()
        {
            List<string> data = new List<string>() { "123" };
            int index = 0;

            Assert.ThrowsException<HexException>(() => DataParser.OptionalString_HexCode(data, index, "OptionalString_HexCode_WithInput_Numerical"));
        }

        [TestMethod]
        public void OptionalString_HexCode_WithInput_Alphabetical()
        {
            List<string> data = new List<string>() { "test" };
            int index = 0;

            Assert.ThrowsException<HexException>(() => DataParser.OptionalString_HexCode(data, index, "OptionalString_HexCode_WithInput_Alphabetical"));
        }

        [TestMethod]
        public void OptionalString_HexCode_WithInput_NoHash()
        {
            List<string> data = new List<string>() { "1A2B3C" };
            int index = 0;

            string value = DataParser.OptionalString_HexCode(data, index, "OptionalString_HexCode_WithInput_NoHash");
            Assert.AreEqual<string>("#1A2B3C", value);
        }

        [TestMethod]
        public void OptionalString_HexCode_WithInput_Hash()
        {
            List<string> data = new List<string>() { "#1A2B3C" };
            int index = 0;

            string value = DataParser.OptionalString_HexCode(data, index, "OptionalString_HexCode_WithInput_Hash");
            Assert.AreEqual<string>("#1A2B3C", value);
        }

        #endregion OptionalString_HexCode

    }
}
