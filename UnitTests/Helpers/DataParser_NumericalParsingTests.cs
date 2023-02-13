using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Services.Helpers;

namespace UnitTests.Helpers
{
    [TestClass]
    public class DataParser_NumericalParsingTests
    {
        #region Int_Any

        [TestMethod]
        public void Int_Any_WithInput_Null()
        {
            List<string> data = new List<string>() { };
            int index = 0;

            Assert.ThrowsException<AnyIntegerException>(() => DataParser.Int_Any(data, index, "Int_Any_WithInput_Null"));
        }

        [TestMethod]
        public void Int_Any_WithInput_EmptyString()
        {
            List<string> data = new List<string>() { string.Empty };
            int index = 0;

            Assert.ThrowsException<AnyIntegerException>(() => DataParser.Int_Any(data, index, "Int_Any_WithInput_EmptyString"));
        }

        [TestMethod]
        public void Int_Any_WithInput_Whitespace()
        {
            List<string> data = new List<string>() { "   " };
            int index = 0;

            Assert.ThrowsException<AnyIntegerException>(() => DataParser.Int_Any(data, index, "Int_Any_WithInput_Whitespace"));
        }

        [TestMethod]
        public void Int_Any_WithInput_Alphabetical()
        {
            List<string> data = new List<string>() { "test" };
            int index = 0;

            Assert.ThrowsException<AnyIntegerException>(() => DataParser.Int_Any(data, index, "Int_Any_WithInput_Alphabetical"));
        }

        [TestMethod]
        public void Int_Any_WithInput_Decimal()
        {
            List<string> data = new List<string>() { "1.5" };
            int index = 0;

            Assert.ThrowsException<AnyIntegerException>(() => DataParser.Int_Any(data, index, "Int_Any_WithInput_Decimal"));
        }

        [TestMethod]
        public void Int_Any_WithInput_0()
        {
            List<string> data = new List<string>() { "0" };
            int index = 0;

            int value = DataParser.Int_Any(data, index, "Int_Any_WithInput_0");
            Assert.AreEqual<int>(0, value);
        }

        [TestMethod]
        public void Int_Any_WithInput_1()
        {
            List<string> data = new List<string>() { "1" };
            int index = 0;

            int value = DataParser.Int_Any(data, index, "Int_Any_WithInput_1");
            Assert.AreEqual<int>(1, value);
        }

        [TestMethod]
        public void Int_Any_WithInput_Neg1()
        {
            List<string> data = new List<string>() { "-1" };
            int index = 0;

            int value = DataParser.Int_Any(data, index, "Int_Any_WithInput_Neg1");
            Assert.AreEqual<int>(-1, value);
        }

        #endregion Int_Any

        #region OptionalInt_Any

        [TestMethod]
        public void OptionalInt_Any_WithInput_Null_NoDefault()
        {
            List<string> data = new List<string>() { };
            int index = 0;

            int value = DataParser.OptionalInt_Any(data, index, "OptionalInt_Any_WithInput_Null_NoDefault");
            Assert.AreEqual<int>(0, value);
        }

        [TestMethod]
        public void OptionalInt_Any_WithInput_Null_WithDefault()
        {
            List<string> data = new List<string>() { };
            int index = 0;

            int value = DataParser.OptionalInt_Any(data, index, "OptionalInt_Any_WithInput_Null_WithDefault", 1);
            Assert.AreEqual<int>(1, value);
        }

        [TestMethod]
        public void OptionalInt_Any_WithInput_EmptyString_NoDefault()
        {
            List<string> data = new List<string>() { string.Empty };
            int index = 0;

            int value = DataParser.OptionalInt_Any(data, index, "OptionalInt_Any_WithInput_EmptyString_NoDefault");
            Assert.AreEqual<int>(0, value);
        }

        [TestMethod]
        public void OptionalInt_Any_WithInput_EmptyString_WithDefault()
        {
            List<string> data = new List<string>() { string.Empty };
            int index = 0;

            int value = DataParser.OptionalInt_Any(data, index, "OptionalInt_Any_WithInput_EmptyString_WithDefault", 1);
            Assert.AreEqual<int>(1, value);
        }

        [TestMethod]
        public void OptionalInt_Any_WithInput_Whitespace_NoDefault()
        {
            List<string> data = new List<string>() { "   " };
            int index = 0;

            int value = DataParser.OptionalInt_Any(data, index, "OptionalInt_Any_WithInput_Whitespace_NoDefault");
            Assert.AreEqual<int>(0, value);
        }

        [TestMethod]
        public void OptionalInt_Any_WithInput_Whitespace_WithDefault()
        {
            List<string> data = new List<string>() { "   " };
            int index = 0;

            int value = DataParser.OptionalInt_Any(data, index, "OptionalInt_Any_WithInput_Whitespace_WithDefault", 1);
            Assert.AreEqual<int>(1, value);
        }

        [TestMethod]
        public void OptionalInt_Any_WithInput_0()
        {
            List<string> data = new List<string>() { "0" };
            int index = 0;

            int value = DataParser.OptionalInt_Any(data, index, "OptionalInt_Any_WithInput_0");
            Assert.AreEqual<int>(0, value);
        }

        [TestMethod]
        public void OptionalInt_Any_WithInput_1()
        {
            List<string> data = new List<string>() { "1" };
            int index = 0;

            int value = DataParser.OptionalInt_Any(data, index, "OptionalInt_Any_WithInput_1");
            Assert.AreEqual<int>(1, value);
        }

        [TestMethod]
        public void OptionalInt_Any_WithInput_Neg1()
        {
            List<string> data = new List<string>() { "-1" };
            int index = 0;

            int value = DataParser.OptionalInt_Any(data, index, "OptionalInt_Any_WithInput_Neg1");
            Assert.AreEqual<int>(-1, value);
        }

        #endregion OptionalInt_Any

        #region Int_Positive

        [TestMethod]
        public void Int_Positive_WithInput_Null()
        {
            List<string> data = new List<string>() { };
            int index = 0;

            Assert.ThrowsException<PositiveIntegerException>(() => DataParser.Int_Positive(data, index, "Int_Positive_WithInput_Null"));
        }

        [TestMethod]
        public void Int_Positive_WithInput_EmptyString()
        {
            List<string> data = new List<string>() { string.Empty };
            int index = 0;

            Assert.ThrowsException<PositiveIntegerException>(() => DataParser.Int_Positive(data, index, "Int_Positive_WithInput_EmptyString"));
        }

        [TestMethod]
        public void Int_Positive_WithInput_Whitespace()
        {
            List<string> data = new List<string>() { "   " };
            int index = 0;

            Assert.ThrowsException<PositiveIntegerException>(() => DataParser.Int_Positive(data, index, "Int_Positive_WithInput_Whitespace"));
        }

        [TestMethod]
        public void Int_Positive_WithInput_Alphabetical()
        {
            List<string> data = new List<string>() { "test" };
            int index = 0;

            Assert.ThrowsException<PositiveIntegerException>(() => DataParser.Int_Positive(data, index, "Int_Positive_WithInput_Alphabetical"));
        }

        [TestMethod]
        public void Int_Positive_WithInput_Decimal()
        {
            List<string> data = new List<string>() { "1.5" };
            int index = 0;

            Assert.ThrowsException<PositiveIntegerException>(() => DataParser.Int_Positive(data, index, "Int_Positive_WithInput_Decimal"));
        }

        [TestMethod]
        public void Int_Positive_WithInput_0()
        {
            List<string> data = new List<string>() { "0" };
            int index = 0;

            int value = DataParser.Int_Positive(data, index, "Int_Positive_WithInput_0");
            Assert.AreEqual<int>(0, value);
        }

        [TestMethod]
        public void Int_Positive_WithInput_1()
        {
            List<string> data = new List<string>() { "1" };
            int index = 0;

            int value = DataParser.Int_Positive(data, index, "Int_Positive_WithInput_1");
            Assert.AreEqual<int>(1, value);
        }

        [TestMethod]
        public void Int_Positive_WithInput_Neg1()
        {
            List<string> data = new List<string>() { "-1" };
            int index = 0;

            Assert.ThrowsException<PositiveIntegerException>(() => DataParser.Int_Positive(data, index, "Int_Positive_WithInput_Neg1"));
        }

        #endregion Int_Positive

        #region OptionalInt_Positive

        [TestMethod]
        public void OptionalInt_Positive_WithInput_Null_NoDefault()
        {
            List<string> data = new List<string>() { };
            int index = 0;

            int value = DataParser.OptionalInt_Positive(data, index, "OptionalInt_Positive_WithInput_Null_NoDefault");
            Assert.AreEqual<int>(0, value);
        }

        [TestMethod]
        public void OptionalInt_Positive_WithInput_Null_WithDefault()
        {
            List<string> data = new List<string>() { };
            int index = 0;

            int value = DataParser.OptionalInt_Positive(data, index, "OptionalInt_Positive_WithInput_Null_WithDefault", 1);
            Assert.AreEqual<int>(1, value);
        }

        [TestMethod]
        public void OptionalInt_Positive_WithInput_EmptyString_NoDefault()
        {
            List<string> data = new List<string>() { string.Empty };
            int index = 0;

            int value = DataParser.OptionalInt_Positive(data, index, "OptionalInt_Positive_WithInput_EmptyString_NoDefault");
            Assert.AreEqual<int>(0, value);
        }

        [TestMethod]
        public void OptionalInt_Positive_WithInput_EmptyString_WithDefault()
        {
            List<string> data = new List<string>() { };
            int index = 0;

            int value = DataParser.OptionalInt_Positive(data, index, "OptionalInt_Positive_WithInput_EmptyString_WithDefault", 1);
            Assert.AreEqual<int>(1, value);
        }

        [TestMethod]
        public void OptionalInt_Positive_WithInput_Whitespace_NoDefault()
        {
            List<string> data = new List<string>() { "   " };
            int index = 0;

            int value = DataParser.OptionalInt_Positive(data, index, "OptionalInt_Positive_WithInput_Whitespace_NoDefault");
            Assert.AreEqual<int>(0, value);
        }

        [TestMethod]
        public void OptionalInt_Positive_WithInput_Whitespace_WithDefault()
        {
            List<string> data = new List<string>() { "   " };
            int index = 0;

            int value = DataParser.OptionalInt_Positive(data, index, "OptionalInt_Positive_WithInput_Whitespace_WithDefault", 1);
            Assert.AreEqual<int>(1, value);
        }

        [TestMethod]
        public void OptionalInt_Positive_WithInput_0()
        {
            List<string> data = new List<string>() { "0" };
            int index = 0;

            int value = DataParser.OptionalInt_Positive(data, index, "OptionalInt_Positive_WithInput_0");
            Assert.AreEqual<int>(0, value);
        }

        [TestMethod]
        public void OptionalInt_Positive_WithInput_1()
        {
            List<string> data = new List<string>() { "1" };
            int index = 0;

            int value = DataParser.OptionalInt_Positive(data, index, "OptionalInt_Positive_WithInput_1");
            Assert.AreEqual<int>(1, value);
        }

        [TestMethod]
        public void OptionalInt_Positive_WithInput_Neg1()
        {
            List<string> data = new List<string>() { "-1" };
            int index = 0;

            Assert.ThrowsException<PositiveIntegerException>(() => DataParser.OptionalInt_Positive(data, index, "OptionalInt_Positive_WithInput_Neg1"));
        }

        #endregion OptionalInt_Positive

        #region Int_NonZeroPositive

        [TestMethod]
        public void Int_NonZeroPositive_WithInput_Null()
        {
            List<string> data = new List<string>() { };
            int index = 0;

            Assert.ThrowsException<NonZeroPositiveIntegerException>(() => DataParser.Int_NonZeroPositive(data, index, "Int_NonZeroPositive_WithInput_Null"));
        }

        [TestMethod]
        public void Int_NonZeroPositive_WithInput_EmptyString()
        {
            List<string> data = new List<string>() { string.Empty };
            int index = 0;

            Assert.ThrowsException<NonZeroPositiveIntegerException>(() => DataParser.Int_NonZeroPositive(data, index, "Int_NonZeroPositive_WithInput_EmptyString"));
        }

        [TestMethod]
        public void Int_NonZeroPositive_WithInput_Whitespace()
        {
            List<string> data = new List<string>() { "   " };
            int index = 0;

            Assert.ThrowsException<NonZeroPositiveIntegerException>(() => DataParser.Int_NonZeroPositive(data, index, "Int_NonZeroPositive_WithInput_Whitespace"));
        }

        [TestMethod]
        public void Int_NonZeroPositive_WithInput_Alphabetical()
        {
            List<string> data = new List<string>() { "test" };
            int index = 0;

            Assert.ThrowsException<NonZeroPositiveIntegerException>(() => DataParser.Int_NonZeroPositive(data, index, "Int_NonZeroPositive_WithInput_Alphabetical"));
        }

        [TestMethod]
        public void Int_NonZeroPositive_WithInput_Decimal()
        {
            List<string> data = new List<string>() { "1.5" };
            int index = 0;

            Assert.ThrowsException<NonZeroPositiveIntegerException>(() => DataParser.Int_NonZeroPositive(data, index, "Int_NonZeroPositive_WithInput_Decimal"));
        }

        [TestMethod]
        public void Int_NonZeroPositive_WithInput_0()
        {
            List<string> data = new List<string>() { "0" };
            int index = 0;

            Assert.ThrowsException<NonZeroPositiveIntegerException>(() => DataParser.Int_NonZeroPositive(data, index, "Int_NonZeroPositive_WithInput_0"));
        }

        [TestMethod]
        public void Int_NonZeroPositive_WithInput_1()
        {
            List<string> data = new List<string>() { "1" };
            int index = 0;

            int value = DataParser.Int_NonZeroPositive(data, index, "Int_NonZeroPositive_WithInput_1");
            Assert.AreEqual<int>(1, value);
        }

        [TestMethod]
        public void Int_NonZeroPositive_WithInput_Neg1()
        {
            List<string> data = new List<string>() { "-1" };
            int index = 0;

            Assert.ThrowsException<NonZeroPositiveIntegerException>(() => DataParser.Int_NonZeroPositive(data, index, "Int_NonZeroPositive_WithInput_Neg1"));
        }

        #endregion Int_NonZeroPositive

        #region OptionalInt_NonZeroPositive

        [TestMethod]
        public void OptionalInt_NonZeroPositive_WithInput_Null_NoDefault()
        {
            List<string> data = new List<string>() { };
            int index = 0;

            int value = DataParser.OptionalInt_NonZeroPositive(data, index, "OptionalInt_NonZeroPositive_WithInput_Null_NoDefault");
            Assert.AreEqual<int>(1, value);
        }

        [TestMethod]
        public void OptionalInt_NonZeroPositive_WithInput_Null_WithDefault()
        {
            List<string> data = new List<string>() { };
            int index = 0;

            int value = DataParser.OptionalInt_NonZeroPositive(data, index, "OptionalInt_NonZeroPositive_WithInput_Null_WithDefault", 2);
            Assert.AreEqual<int>(2, value);
        }

        [TestMethod]
        public void OptionalInt_NonZeroPositive_WithInput_EmptyString_NoDefault()
        {
            List<string> data = new List<string>() { string.Empty };
            int index = 0;

            int value = DataParser.OptionalInt_NonZeroPositive(data, index, "OptionalInt_NonZeroPositive_WithInput_EmptyString_NoDefault");
            Assert.AreEqual<int>(1, value);
        }

        [TestMethod]
        public void OptionalInt_NonZeroPositive_WithInput_EmptyString_WithDefault()
        {
            List<string> data = new List<string>() { string.Empty };
            int index = 0;

            int value = DataParser.OptionalInt_NonZeroPositive(data, index, "OptionalInt_NonZeroPositive_WithInput_EmptyString_WithDefault", 2);
            Assert.AreEqual<int>(2, value);
        }

        [TestMethod]
        public void OptionalInt_NonZeroPositive_WithInput_Whitespace_NoDefault()
        {
            List<string> data = new List<string>() { "   " };
            int index = 0;

            int value = DataParser.OptionalInt_NonZeroPositive(data, index, "OptionalInt_NonZeroPositive_WithInput_Whitespace_NoDefault");
            Assert.AreEqual<int>(1, value);
        }

        [TestMethod]
        public void OptionalInt_NonZeroPositive_WithInput_Whitespace_WithDefault()
        {
            List<string> data = new List<string>() { "   " };
            int index = 0;

            int value = DataParser.OptionalInt_NonZeroPositive(data, index, "OptionalInt_NonZeroPositive_WithInput_Whitespace_WithDefault", 2);
            Assert.AreEqual<int>(2, value);
        }

        [TestMethod]
        public void OptionalInt_NonZeroPositive_WithInput_0()
        {
            List<string> data = new List<string>() { "0" };
            int index = 0;

            Assert.ThrowsException<NonZeroPositiveIntegerException>(() => DataParser.OptionalInt_NonZeroPositive(data, index, "OptionalInt_NonZeroPositive_WithInput_0"));
        }

        [TestMethod]
        public void OptionalInt_NonZeroPositive_WithInput_1()
        {
            List<string> data = new List<string>() { "1" };
            int index = 0;

            int value = DataParser.OptionalInt_NonZeroPositive(data, index, "OptionalInt_NonZeroPositive_WithInput_1");
            Assert.AreEqual<int>(1, value);
        }

        [TestMethod]
        public void OptionalInt_NonZeroPositive_WithInput_Neg1()
        {
            List<string> data = new List<string>() { "-1" };
            int index = 0;

            Assert.ThrowsException<NonZeroPositiveIntegerException>(() => DataParser.OptionalInt_NonZeroPositive(data, index, "OptionalInt_NonZeroPositive_WithInput_Neg1"));
        }

        #endregion OptionalInt_NonZeroPositive

        #region Int_Negative

        [TestMethod]
        public void Int_Negative_WithInput_Null()
        {
            List<string> data = new List<string>() { };
            int index = 0;

            Assert.ThrowsException<NegativeIntegerException>(() => DataParser.Int_Negative(data, index, "Int_Negative_WithInput_Null"));
        }

        [TestMethod]
        public void Int_Negative_WithInput_EmptyString()
        {
            List<string> data = new List<string>() { string.Empty };
            int index = 0;

            Assert.ThrowsException<NegativeIntegerException>(() => DataParser.Int_Negative(data, index, "Int_Negative_WithInput_EmptyString"));
        }

        [TestMethod]
        public void Int_Negative_WithInput_Whitespace()
        {
            List<string> data = new List<string>() { "   " };
            int index = 0;

            Assert.ThrowsException<NegativeIntegerException>(() => DataParser.Int_Negative(data, index, "Int_Negative_WithInput_Whitespace"));
        }

        [TestMethod]
        public void Int_Negative_WithInput_Alphabetical()
        {
            List<string> data = new List<string>() { "test" };
            int index = 0;

            Assert.ThrowsException<NegativeIntegerException>(() => DataParser.Int_Negative(data, index, "Int_Negative_WithInput_Alphabetical"));
        }

        [TestMethod]
        public void Int_Negative_WithInput_Decimal()
        {
            List<string> data = new List<string>() { "1.5" };
            int index = 0;

            Assert.ThrowsException<NegativeIntegerException>(() => DataParser.Int_Negative(data, index, "Int_Negative_WithInput_Decimal"));
        }

        [TestMethod]
        public void Int_Negative_WithInput_0()
        {
            List<string> data = new List<string>() { "0" };
            int index = 0;

            Assert.ThrowsException<NegativeIntegerException>(() => DataParser.Int_Negative(data, index, "Int_Negative_WithInput_0"));
        }

        [TestMethod]
        public void Int_Negative_WithInput_1()
        {
            List<string> data = new List<string>() { "1" };
            int index = 0;

            Assert.ThrowsException<NegativeIntegerException>(() => DataParser.Int_Negative(data, index, "Int_Negative_WithInput_1"));
        }

        [TestMethod]
        public void Int_Negative_WithInput_Neg1()
        {
            List<string> data = new List<string>() { "-1" };
            int index = 0;

            int value = DataParser.Int_Negative(data, index, "Int_Negative_WithInput_Neg1");
            Assert.AreEqual<int>(-1, value);
        }

        #endregion Int_Negative

        #region OptionalInt_Negative

        [TestMethod]
        public void OptionalInt_Negative_WithInput_Null_NoDefault()
        {
            List<string> data = new List<string>() { };
            int index = 0;

            int value = DataParser.OptionalInt_Negative(data, index, "OptionalInt_Negative_WithInput_Null_NoDefault");
            Assert.AreEqual<int>(-1, value);
        }

        [TestMethod]
        public void OptionalInt_Negative_WithInput_Null_WithDefault()
        {
            List<string> data = new List<string>() { };
            int index = 0;

            int value = DataParser.OptionalInt_Negative(data, index, "OptionalInt_Negative_WithInput_Null_WithDefault", -2);
            Assert.AreEqual<int>(-2, value);
        }

        [TestMethod]
        public void OptionalInt_Negative_WithInput_EmptyString_NoDefault()
        {
            List<string> data = new List<string>() { string.Empty };
            int index = 0;

            int value = DataParser.OptionalInt_Negative(data, index, "OptionalInt_Negative_WithInput_EmptyString_NoDefault");
            Assert.AreEqual<int>(-1, value);
        }

        [TestMethod]
        public void OptionalInt_Negative_WithInput_EmptyString_WithDefault()
        {
            List<string> data = new List<string>() { string.Empty };
            int index = 0;

            int value = DataParser.OptionalInt_Negative(data, index, "OptionalInt_Negative_WithInput_EmptyString_WithDefault", -2);
            Assert.AreEqual<int>(-2, value);
        }

        [TestMethod]
        public void OptionalInt_Negative_WithInput_Whitespace_NoDefault()
        {
            List<string> data = new List<string>() { "   " };
            int index = 0;

            int value = DataParser.OptionalInt_Negative(data, index, "OptionalInt_Negative_WithInput_Whitespace_NoDefault");
            Assert.AreEqual<int>(-1, value);
        }

        [TestMethod]
        public void OptionalInt_Negative_WithInput_Whitespace_WithDefault()
        {
            List<string> data = new List<string>() { "   " };
            int index = 0;

            int value = DataParser.OptionalInt_Negative(data, index, "OptionalInt_Negative_WithInput_Whitespace_WithDefault", -2);
            Assert.AreEqual<int>(-2, value);
        }

        [TestMethod]
        public void OptionalInt_Negative_WithInput_0()
        {
            List<string> data = new List<string>() { "0" };
            int index = 0;

            Assert.ThrowsException<NegativeIntegerException>(() => DataParser.OptionalInt_Negative(data, index, "OptionalInt_Negative_WithInput_0"));
        }

        [TestMethod]
        public void OptionalInt_Negative_WithInput_1()
        {
            List<string> data = new List<string>() { "1" };
            int index = 0;

            Assert.ThrowsException<NegativeIntegerException>(() => DataParser.OptionalInt_Negative(data, index, "OptionalInt_Negative_WithInput_1"));
        }

        [TestMethod]
        public void OptionalInt_Negative_WithInput_Neg1()
        {
            List<string> data = new List<string>() { "-1" };
            int index = 0;

            int value = DataParser.OptionalInt_Negative(data, index, "OptionalInt_Negative_WithInput_Neg1");
            Assert.AreEqual<int>(-1, value);
        }

        #endregion OptionalInt_Negative

        #region Decimal_Any

        [TestMethod]
        public void Decimal_Any_WithInput_Null()
        {
            List<string> data = new List<string>() { };
            int index = 0;

            Assert.ThrowsException<AnyDecimalException>(() => DataParser.Decimal_Any(data, index, "Decimal_Any_WithInput_Null"));
        }

        [TestMethod]
        public void Decimal_Any_WithInput_EmptyString()
        {
            List<string> data = new List<string>() { string.Empty };
            int index = 0;

            Assert.ThrowsException<AnyDecimalException>(() => DataParser.Decimal_Any(data, index, "Decimal_Any_WithInput_EmptyString"));
        }

        [TestMethod]
        public void Decimal_Any_WithInput_Whitespace()
        {
            List<string> data = new List<string>() { "   " };
            int index = 0;

            Assert.ThrowsException<AnyDecimalException>(() => DataParser.Decimal_Any(data, index, "Decimal_Any_WithInput_Whitespace"));
        }

        [TestMethod]
        public void Decimal_Any_WithInput_Alphabetical()
        {
            List<string> data = new List<string>() { "test" };
            int index = 0;

            Assert.ThrowsException<AnyDecimalException>(() => DataParser.Decimal_Any(data, index, "Decimal_Any_WithInput_Alphabetical"));
        }

        [TestMethod]
        public void Decimal_Any_WithInput_Decimal()
        {
            List<string> data = new List<string>() { "1.5" };
            int index = 0;

            decimal value = DataParser.Decimal_Any(data, index, "Decimal_Any_WithInput_Decimal");
            Assert.AreEqual<decimal>(1.5m, value);
        }

        [TestMethod]
        public void Decimal_Any_WithInput_0()
        {
            List<string> data = new List<string>() { "0" };
            int index = 0;

            decimal value = DataParser.Decimal_Any(data, index, "Decimal_Any_WithInput_0");
            Assert.AreEqual<decimal>(0.0m, value);
        }

        [TestMethod]
        public void Decimal_Any_WithInput_1()
        {
            List<string> data = new List<string>() { "1" };
            int index = 0;

            decimal value = DataParser.Decimal_Any(data, index, "Decimal_Any_WithInput_1");
            Assert.AreEqual<decimal>(1.0m, value);
        }

        [TestMethod]
        public void Decimal_Any_WithInput_Neg1()
        {
            List<string> data = new List<string>() { "-1" };
            int index = 0;

            decimal value = DataParser.Decimal_Any(data, index, "Decimal_Any_WithInput_Neg1");
            Assert.AreEqual<decimal>(-1.0m, value);
        }

        #endregion Int_Any

        #region OptionalDecimal_Any

        [TestMethod]
        public void OptionalDecimal_Any_WithInput_Null_NoDefault()
        {
            List<string> data = new List<string>() { };
            int index = 0;

            decimal value = DataParser.OptionalDecimal_Any(data, index, "OptionalDecimal_Any_WithInput_Null_NoDefault");
            Assert.AreEqual<decimal>(0.0m, value);
        }

        [TestMethod]
        public void OptionalDecimal_Any_WithInput_Null_WithDefault()
        {
            List<string> data = new List<string>() { };
            int index = 0;

            decimal value = DataParser.OptionalDecimal_Any(data, index, "OptionalDecimal_Any_WithInput_Null_WithDefault", 1.0m);
            Assert.AreEqual<decimal>(1.0m, value);
        }

        [TestMethod]
        public void OptionalDecimal_Any_WithInput_EmptyString_NoDefault()
        {
            List<string> data = new List<string>() { string.Empty };
            int index = 0;

            decimal value = DataParser.OptionalDecimal_Any(data, index, "OptionalDecimal_Any_WithInput_EmptyString_NoDefault");
            Assert.AreEqual<decimal>(0.0m, value);
        }

        [TestMethod]
        public void OptionalDecimal_Any_WithInput_EmptyString_WithDefault()
        {
            List<string> data = new List<string>() { string.Empty };
            int index = 0;

            decimal value = DataParser.OptionalDecimal_Any(data, index, "OptionalDecimal_Any_WithInput_EmptyString_WithDefault", 1.0m);
            Assert.AreEqual<decimal>(1.0m, value);
        }

        [TestMethod]
        public void OptionalDecimal_Any_WithInput_Whitespace_NoDefault()
        {
            List<string> data = new List<string>() { "   " };
            int index = 0;

            decimal value = DataParser.OptionalDecimal_Any(data, index, "OptionalDecimal_Any_WithInput_Whitespace_NoDefault");
            Assert.AreEqual<decimal>(0.0m, value);
        }

        [TestMethod]
        public void OptionalDecimal_Any_WithInput_Whitespace_WithDefault()
        {
            List<string> data = new List<string>() { "   " };
            int index = 0;

            decimal value = DataParser.OptionalDecimal_Any(data, index, "OptionalDecimal_Any_WithInput_Whitespace_WithDefault", 1.0m);
            Assert.AreEqual<decimal>(1.0m, value);
        }

        [TestMethod]
        public void OptionalDecimal_Any_WithInput_0()
        {
            List<string> data = new List<string>() { "0" };
            int index = 0;

            decimal value = DataParser.OptionalDecimal_Any(data, index, "OptionalDecimal_Any_WithInput_0");
            Assert.AreEqual<decimal>(0.0m, value);
        }

        [TestMethod]
        public void OptionalDecimal_Any_WithInput_1()
        {
            List<string> data = new List<string>() { "1" };
            int index = 0;

            decimal value = DataParser.OptionalDecimal_Any(data, index, "OptionalDecimal_Any_WithInput_1");
            Assert.AreEqual<decimal>(1.0m, value);
        }

        [TestMethod]
        public void OptionalDecimal_Any_WithInput_Neg1()
        {
            List<string> data = new List<string>() { "-1" };
            int index = 0;

            decimal value = DataParser.OptionalDecimal_Any(data, index, "OptionalDecimal_Any_WithInput_Neg1");
            Assert.AreEqual<decimal>(-1.0m, value);
        }

        #endregion OptionalDecimal_Any

        #region Decimal_Positive

        [TestMethod]
        public void Decimal_Positive_WithInput_Null()
        {
            List<string> data = new List<string>() { };
            int index = 0;

            Assert.ThrowsException<PositiveDecimalException>(() => DataParser.Decimal_Positive(data, index, "Decimal_Positive_WithInput_Null"));
        }

        [TestMethod]
        public void Decimal_Positive_WithInput_EmptyString()
        {
            List<string> data = new List<string>() { string.Empty };
            int index = 0;

            Assert.ThrowsException<PositiveDecimalException>(() => DataParser.Decimal_Positive(data, index, "Decimal_Positive_WithInput_EmptyString"));
        }

        [TestMethod]
        public void Decimal_Positive_WithInput_Whitespace()
        {
            List<string> data = new List<string>() { "   " };
            int index = 0;

            Assert.ThrowsException<PositiveDecimalException>(() => DataParser.Decimal_Positive(data, index, "Decimal_Positive_WithInput_Whitespace"));
        }

        [TestMethod]
        public void Decimal_Positive_WithInput_Alphabetical()
        {
            List<string> data = new List<string>() { "test" };
            int index = 0;

            Assert.ThrowsException<PositiveDecimalException>(() => DataParser.Decimal_Positive(data, index, "Decimal_Positive_WithInput_Alphabetical"));
        }

        [TestMethod]
        public void Decimal_Positive_WithInput_Decimal()
        {
            List<string> data = new List<string>() { "1.5" };
            int index = 0;

            decimal value = DataParser.Decimal_Positive(data, index, "Decimal_Positive_WithInput_Decimal");
            Assert.AreEqual<decimal>(1.5m, value);
        }

        [TestMethod]
        public void Decimal_Positive_WithInput_0()
        {
            List<string> data = new List<string>() { "0" };
            int index = 0;

            decimal value = DataParser.Decimal_Positive(data, index, "Decimal_Positive_WithInput_0");
            Assert.AreEqual<decimal>(0.0m, value);
        }

        [TestMethod]
        public void Decimal_Positive_WithInput_1()
        {
            List<string> data = new List<string>() { "1" };
            int index = 0;

            decimal value = DataParser.Decimal_Positive(data, index, "Decimal_Positive_WithInput_1");
            Assert.AreEqual<decimal>(1.0m, value);
        }

        [TestMethod]
        public void Decimal_Positive_WithInput_Neg1()
        {
            List<string> data = new List<string>() { "-1" };
            int index = 0;

            Assert.ThrowsException<PositiveDecimalException>(() => DataParser.Decimal_Positive(data, index, "Decimal_Positive_WithInput_Neg1"));
        }

        #endregion Decimal_Positive

        #region OptionalDecimal_Positive

        [TestMethod]
        public void OptionalDecimal_Positive_WithInput_Null_NoDefault()
        {
            List<string> data = new List<string>() { };
            int index = 0;

            decimal value = DataParser.OptionalDecimal_Positive(data, index, "OptionalDecimal_Positive_WithInput_Null_NoDefault");
            Assert.AreEqual<decimal>(0.0m, value);
        }

        [TestMethod]
        public void OptionalDecimal_Positive_WithInput_Null_WithDefault()
        {
            List<string> data = new List<string>() { };
            int index = 0;

            decimal value = DataParser.OptionalDecimal_Positive(data, index, "OptionalDecimal_Positive_WithInput_Null_WithDefault", 1.0m);
            Assert.AreEqual<decimal>(1.0m, value);
        }

        [TestMethod]
        public void OptionalDecimal_Positive_WithInput_EmptyString_NoDefault()
        {
            List<string> data = new List<string>() { string.Empty };
            int index = 0;

            decimal value = DataParser.OptionalDecimal_Positive(data, index, "OptionalDecimal_Positive_WithInput_EmptyString_NoDefault");
            Assert.AreEqual<decimal>(0.0m, value);
        }

        [TestMethod]
        public void OptionalDecimal_Positive_WithInput_EmptyString_WithDefault()
        {
            List<string> data = new List<string>() { string.Empty };
            int index = 0;

            decimal value = DataParser.OptionalDecimal_Positive(data, index, "OptionalDecimal_Positive_WithInput_EmptyString_WithDefault", 1.0m);
            Assert.AreEqual<decimal>(1.0m, value);
        }

        [TestMethod]
        public void OptionalDecimal_Positive_WithInput_Whitespace_NoDefault()
        {
            List<string> data = new List<string>() { "   " };
            int index = 0;

            decimal value = DataParser.OptionalDecimal_Positive(data, index, "OptionalDecimal_Positive_WithInput_Whitespace_NoDefault");
            Assert.AreEqual<decimal>(0.0m, value);
        }

        [TestMethod]
        public void OptionalDecimal_Positive_WithInput_Whitespace_WithDefault()
        {
            List<string> data = new List<string>() { "   " };
            int index = 0;

            decimal value = DataParser.OptionalDecimal_Positive(data, index, "OptionalDecimal_Positive_WithInput_Whitespace_WithDefault", 1.0m);
            Assert.AreEqual<decimal>(1.0m, value);
        }

        [TestMethod]
        public void OptionalDecimal_Positive_WithInput_0()
        {
            List<string> data = new List<string>() { "0" };
            int index = 0;

            decimal value = DataParser.OptionalDecimal_Positive(data, index, "OptionalDecimal_Positive_WithInput_0");
            Assert.AreEqual<decimal>(0.0m, value);
        }

        [TestMethod]
        public void OptionalDecimal_Positive_WithInput_1()
        {
            List<string> data = new List<string>() { "1" };
            int index = 0;

            decimal value = DataParser.OptionalDecimal_Positive(data, index, "OptionalDecimal_Positive_WithInput_1");
            Assert.AreEqual<decimal>(1.0m, value);
        }

        [TestMethod]
        public void OptionalDecimal_Positive_WithInput_Neg1()
        {
            List<string> data = new List<string>() { "-1" };
            int index = 0;

            Assert.ThrowsException<PositiveDecimalException>(() => DataParser.OptionalDecimal_Positive(data, index, "OptionalDecimal_Positive_WithInput_Neg1"));
        }

        #endregion OptionalDecimal_Positive

        #region Decimal_NonZeroPositive

        [TestMethod]
        public void Decimal_NonZeroPositive_WithInput_Null()
        {
            List<string> data = new List<string>() { };
            int index = 0;

            Assert.ThrowsException<NonZeroPositiveDecimalException>(() => DataParser.Decimal_NonZeroPositive(data, index, "Decimal_NonZeroPositive_WithInput_Null"));
        }

        [TestMethod]
        public void Decimal_NonZeroPositive_WithInput_EmptyString()
        {
            List<string> data = new List<string>() { string.Empty };
            int index = 0;

            Assert.ThrowsException<NonZeroPositiveDecimalException>(() => DataParser.Decimal_NonZeroPositive(data, index, "Decimal_NonZeroPositive_WithInput_EmptyString"));
        }

        [TestMethod]
        public void Decimal_NonZeroPositive_WithInput_Whitespace()
        {
            List<string> data = new List<string>() { "   " };
            int index = 0;

            Assert.ThrowsException<NonZeroPositiveDecimalException>(() => DataParser.Decimal_NonZeroPositive(data, index, "Decimal_NonZeroPositive_WithInput_Whitespace"));
        }

        [TestMethod]
        public void Decimal_NonZeroPositive_WithInput_Alphabetical()
        {
            List<string> data = new List<string>() { "test" };
            int index = 0;

            Assert.ThrowsException<NonZeroPositiveDecimalException>(() => DataParser.Decimal_NonZeroPositive(data, index, "Decimal_NonZeroPositive_WithInput_Alphabetical"));
        }

        [TestMethod]
        public void Decimal_NonZeroPositive_WithInput_Decimal()
        {
            List<string> data = new List<string>() { "0.5" };
            int index = 0;

            decimal value = DataParser.Decimal_NonZeroPositive(data, index, "Decimal_NonZeroPositive_WithInput_Decimal");
            Assert.AreEqual<decimal>(0.5m, value);
        }

        [TestMethod]
        public void Decimal_NonZeroPositive_WithInput_0()
        {
            List<string> data = new List<string>() { "0" };
            int index = 0;

            Assert.ThrowsException<NonZeroPositiveDecimalException>(() => DataParser.Decimal_NonZeroPositive(data, index, "Decimal_NonZeroPositive_WithInput_0"));
        }

        [TestMethod]
        public void Decimal_NonZeroPositive_WithInput_1()
        {
            List<string> data = new List<string>() { "1" };
            int index = 0;

            decimal value = DataParser.Decimal_NonZeroPositive(data, index, "Decimal_NonZeroPositive_WithInput_1");
            Assert.AreEqual<decimal>(1.0m, value);
        }

        [TestMethod]
        public void Decimal_NonZeroPositive_WithInput_Neg1()
        {
            List<string> data = new List<string>() { "-1" };
            int index = 0;

            Assert.ThrowsException<NonZeroPositiveDecimalException>(() => DataParser.Decimal_NonZeroPositive(data, index, "Decimal_NonZeroPositive_WithInput_Neg1"));
        }

        #endregion Decimal_NonZeroPositive

        #region OptionalDecimal_NonZeroPositive

        [TestMethod]
        public void OptionalDecimal_NonZeroPositive_WithInput_Null_NoDefault()
        {
            List<string> data = new List<string>() { };
            int index = 0;

            decimal value = DataParser.OptionalDecimal_NonZeroPositive(data, index, "OptionalDecimal_NonZeroPositive_WithInput_Null_NoDefault");
            Assert.AreEqual<decimal>(1.0m, value);
        }

        [TestMethod]
        public void OptionalDecimal_NonZeroPositive_WithInput_Null_WithDefault()
        {
            List<string> data = new List<string>() { };
            int index = 0;

            decimal value = DataParser.OptionalDecimal_NonZeroPositive(data, index, "OptionalDecimal_NonZeroPositive_WithInput_Null_WithDefault", 0.5m);
            Assert.AreEqual<decimal>(0.5m, value);
        }

        [TestMethod]
        public void OptionalDecimal_NonZeroPositive_WithInput_EmptyString_NoDefault()
        {
            List<string> data = new List<string>() { string.Empty };
            int index = 0;

            decimal value = DataParser.OptionalDecimal_NonZeroPositive(data, index, "OptionalDecimal_NonZeroPositive_WithInput_EmptyString_NoDefault");
            Assert.AreEqual<decimal>(1.0m, value);
        }

        [TestMethod]
        public void OptionalDecimal_NonZeroPositive_WithInput_EmptyString_WithDefault()
        {
            List<string> data = new List<string>() { string.Empty };
            int index = 0;

            decimal value = DataParser.OptionalDecimal_NonZeroPositive(data, index, "OptionalDecimal_NonZeroPositive_WithInput_EmptyString_WithDefault", 0.5m);
            Assert.AreEqual<decimal>(0.5m, value);
        }

        [TestMethod]
        public void OptionalDecimal_NonZeroPositive_WithInput_Whitespace_NoDefault()
        {
            List<string> data = new List<string>() { "   " };
            int index = 0;

            decimal value = DataParser.OptionalDecimal_NonZeroPositive(data, index, "OptionalDecimal_NonZeroPositive_WithInput_Whitespace_NoDefault");
            Assert.AreEqual<decimal>(1.0m, value);
        }

        [TestMethod]
        public void OptionalDecimal_NonZeroPositive_WithInput_Whitespace_WithDefault()
        {
            List<string> data = new List<string>() { "   " };
            int index = 0;

            decimal value = DataParser.OptionalDecimal_NonZeroPositive(data, index, "OptionalDecimal_NonZeroPositive_WithInput_Whitespace_WithDefault", 0.5m);
            Assert.AreEqual<decimal>(0.5m, value);
        }

        [TestMethod]
        public void OptionalDecimal_NonZeroPositive_WithInput_0()
        {
            List<string> data = new List<string>() { "0" };
            int index = 0;

            Assert.ThrowsException<NonZeroPositiveDecimalException>(() => DataParser.Decimal_NonZeroPositive(data, index, "OptionalDecimal_NonZeroPositive_WithInput_0"));
        }

        [TestMethod]
        public void OptionalDecimal_NonZeroPositive_WithInput_1()
        {
            List<string> data = new List<string>() { "1" };
            int index = 0;

            decimal value = DataParser.OptionalDecimal_NonZeroPositive(data, index, "OptionalDecimal_NonZeroPositive_WithInput_1");
            Assert.AreEqual<decimal>(1.0m, value);
        }

        [TestMethod]
        public void OptionalDecimal_NonZeroPositive_WithInput_Neg1()
        {
            List<string> data = new List<string>() { "-1" };
            int index = 0;

            Assert.ThrowsException<NonZeroPositiveDecimalException>(() => DataParser.Decimal_NonZeroPositive(data, index, "OptionalDecimal_NonZeroPositive_WithInput_Neg1"));
        }

        #endregion OptionalDecimal_NonZeroPositive

        #region Decimal_OneOrGreater

        [TestMethod]
        public void Decimal_OneOrGreater_WithInput_Null()
        {
            List<string> data = new List<string>() { };
            int index = 0;

            Assert.ThrowsException<OneOrGreaterDecimalException>(() => DataParser.Decimal_OneOrGreater(data, index, "Decimal_OneOrGreater_WithInput_Null"));
        }

        [TestMethod]
        public void Decimal_OneOrGreater_WithInput_EmptyString()
        {
            List<string> data = new List<string>() { string.Empty };
            int index = 0;

            Assert.ThrowsException<OneOrGreaterDecimalException>(() => DataParser.Decimal_OneOrGreater(data, index, "Decimal_OneOrGreater_WithInput_EmptyString"));
        }

        [TestMethod]
        public void Decimal_OneOrGreater_WithInput_Whitespace()
        {
            List<string> data = new List<string>() { "   " };
            int index = 0;

            Assert.ThrowsException<OneOrGreaterDecimalException>(() => DataParser.Decimal_OneOrGreater(data, index, "Decimal_OneOrGreater_WithInput_Whitespace"));
        }

        [TestMethod]
        public void Decimal_OneOrGreater_WithInput_Alphabetical()
        {
            List<string> data = new List<string>() { "test" };
            int index = 0;

            Assert.ThrowsException<OneOrGreaterDecimalException>(() => DataParser.Decimal_OneOrGreater(data, index, "Decimal_OneOrGreater_WithInput_Alphabetical"));
        }

        [TestMethod]
        public void Decimal_OneOrGreater_WithInput_Decimal()
        {
            List<string> data = new List<string>() { "0.5" };
            int index = 0;

            Assert.ThrowsException<OneOrGreaterDecimalException>(() => DataParser.Decimal_OneOrGreater(data, index, "Decimal_OneOrGreater_WithInput_Decimal"));
        }

        [TestMethod]
        public void Decimal_OneOrGreater_WithInput_0()
        {
            List<string> data = new List<string>() { "0" };
            int index = 0;

            Assert.ThrowsException<OneOrGreaterDecimalException>(() => DataParser.Decimal_OneOrGreater(data, index, "Decimal_OneOrGreater_WithInput_0"));
        }

        [TestMethod]
        public void Decimal_OneOrGreater_WithInput_1()
        {
            List<string> data = new List<string>() { "1" };
            int index = 0;

            decimal value = DataParser.Decimal_OneOrGreater(data, index, "Decimal_OneOrGreater_WithInput_1");
            Assert.AreEqual<decimal>(1.0m, value);
        }

        [TestMethod]
        public void Decimal_OneOrGreater_WithInput_Neg1()
        {
            List<string> data = new List<string>() { "-1" };
            int index = 0;

            Assert.ThrowsException<OneOrGreaterDecimalException>(() => DataParser.Decimal_OneOrGreater(data, index, "Decimal_OneOrGreater_WithInput_Neg1"));
        }

        #endregion Decimal_OneOrGreater

        #region OptionalDecimal_OneOrGreater

        [TestMethod]
        public void OptionalDecimal_OneOrGreater_WithInput_Null_NoDefault()
        {
            List<string> data = new List<string>() { };
            int index = 0;

            decimal value = DataParser.OptionalDecimal_OneOrGreater(data, index, "OptionalDecimal_OneOrGreater_WithInput_Null_NoDefault");
            Assert.AreEqual<decimal>(1.0m, value);
        }

        [TestMethod]
        public void OptionalDecimal_OneOrGreater_WithInput_Null_WithDefault()
        {
            List<string> data = new List<string>() { };
            int index = 0;

            decimal value = DataParser.OptionalDecimal_OneOrGreater(data, index, "OptionalDecimal_OneOrGreater_WithInput_Null_WithDefault", 1.5m);
            Assert.AreEqual<decimal>(1.5m, value);
        }

        [TestMethod]
        public void OptionalDecimal_OneOrGreater_WithInput_EmptyString_NoDefault()
        {
            List<string> data = new List<string>() { string.Empty };
            int index = 0;

            decimal value = DataParser.OptionalDecimal_OneOrGreater(data, index, "OptionalDecimal_OneOrGreater_WithInput_EmptyString_NoDefault");
            Assert.AreEqual<decimal>(1.0m, value);
        }

        [TestMethod]
        public void OptionalDecimal_OneOrGreater_WithInput_EmptyString_WithDefault()
        {
            List<string> data = new List<string>() { string.Empty };
            int index = 0;

            decimal value = DataParser.OptionalDecimal_OneOrGreater(data, index, "OptionalDecimal_OneOrGreater_WithInput_EmptyString_WithDefault", 1.5m);
            Assert.AreEqual<decimal>(1.5m, value);
        }

        [TestMethod]
        public void OptionalDecimal_OneOrGreater_WithInput_Whitespace_NoDefault()
        {
            List<string> data = new List<string>() { "   " };
            int index = 0;

            decimal value = DataParser.OptionalDecimal_OneOrGreater(data, index, "OptionalDecimal_OneOrGreater_WithInput_Whitespace_NoDefault");
            Assert.AreEqual<decimal>(1.0m, value);
        }

        [TestMethod]
        public void OptionalDecimal_OneOrGreater_WithInput_Whitespace_WithDefault()
        {
            List<string> data = new List<string>() { "   " };
            int index = 0;

            decimal value = DataParser.OptionalDecimal_OneOrGreater(data, index, "OptionalDecimal_OneOrGreater_WithInput_Whitespace_WithDefault", 1.5m);
            Assert.AreEqual<decimal>(1.5m, value);
        }

        [TestMethod]
        public void OptionalDecimal_OneOrGreater_WithInput_0()
        {
            List<string> data = new List<string>() { "0" };
            int index = 0;

            Assert.ThrowsException<OneOrGreaterDecimalException>(() => DataParser.OptionalDecimal_OneOrGreater(data, index, "OptionalDecimal_OneOrGreater_WithInput_0"));
        }

        [TestMethod]
        public void OptionalDecimal_OneOrGreater_WithInput_1()
        {
            List<string> data = new List<string>() { "1" };
            int index = 0;

            decimal value = DataParser.OptionalDecimal_OneOrGreater(data, index, "OptionalDecimal_OneOrGreater_WithInput_1");
            Assert.AreEqual<decimal>(1.0m, value);
        }

        [TestMethod]
        public void OptionalDecimal_OneOrGreater_WithInput_Neg1()
        {
            List<string> data = new List<string>() { "-1" };
            int index = 0;

            Assert.ThrowsException<OneOrGreaterDecimalException>(() => DataParser.OptionalDecimal_OneOrGreater(data, index, "OptionalDecimal_OneOrGreater_WithInput_Neg1"));
        }

        #endregion OptionalDecimal_OneOrGreater

        #region Decimal_Negative

        [TestMethod]
        public void Decimal_Negative_WithInput_Null()
        {
            List<string> data = new List<string>() { };
            int index = 0;

            Assert.ThrowsException<NegativeDecimalException>(() => DataParser.Decimal_Negative(data, index, "Decimal_Negative_WithInput_Null"));
        }

        [TestMethod]
        public void Decimal_Negative_WithInput_EmptyString()
        {
            List<string> data = new List<string>() { string.Empty };
            int index = 0;

            Assert.ThrowsException<NegativeDecimalException>(() => DataParser.Decimal_Negative(data, index, "Decimal_Negative_WithInput_EmptyString"));
        }

        [TestMethod]
        public void Decimal_Negative_WithInput_Whitespace()
        {
            List<string> data = new List<string>() { "   " };
            int index = 0;

            Assert.ThrowsException<NegativeDecimalException>(() => DataParser.Decimal_Negative(data, index, "Decimal_Negative_WithInput_Whitespace"));
        }

        [TestMethod]
        public void Decimal_Negative_WithInput_Alphabetical()
        {
            List<string> data = new List<string>() { "test" };
            int index = 0;

            Assert.ThrowsException<NegativeDecimalException>(() => DataParser.Decimal_Negative(data, index, "Decimal_Negative_WithInput_Alphabetical"));
        }

        [TestMethod]
        public void Decimal_Negative_WithInput_Decimal()
        {
            List<string> data = new List<string>() { "0.5" };
            int index = 0;

            Assert.ThrowsException<NegativeDecimalException>(() => DataParser.Decimal_Negative(data, index, "Decimal_OneOrGreater_WithInput_Decimal"));
        }

        [TestMethod]
        public void Decimal_Negative_WithInput_0()
        {
            List<string> data = new List<string>() { "0" };
            int index = 0;

            Assert.ThrowsException<NegativeDecimalException>(() => DataParser.Decimal_Negative(data, index, "Decimal_Negative_WithInput_0"));
        }

        [TestMethod]
        public void Decimal_Negative_WithInput_1()
        {
            List<string> data = new List<string>() { "1" };
            int index = 0;

            Assert.ThrowsException<NegativeDecimalException>(() => DataParser.Decimal_Negative(data, index, "Decimal_Negative_WithInput_1"));
        }

        [TestMethod]
        public void Decimal_Negative_WithInput_Neg1()
        {
            List<string> data = new List<string>() { "-1" };
            int index = 0;

            decimal value = DataParser.Decimal_Negative(data, index, "Decimal_Negative_WithInput_Neg1");
            Assert.AreEqual<decimal>(-1.0m, value);
        }

        #endregion Decimal_Negative

        #region OptionalDecimal_Negative

        [TestMethod]
        public void OptionalDecimal_Negative_WithInput_Null_NoDefault()
        {
            List<string> data = new List<string>() { };
            int index = 0;

            decimal value = DataParser.OptionalDecimal_Negative(data, index, "OptionalDecimal_Negative_WithInput_Null_NoDefault");
            Assert.AreEqual<decimal>(-1.0m, value);
        }

        [TestMethod]
        public void OptionalDecimal_Negative_WithInput_Null_WithDefault()
        {
            List<string> data = new List<string>() { };
            int index = 0;

            decimal value = DataParser.OptionalDecimal_Negative(data, index, "OptionalDecimal_Negative_WithInput_Null_WithDefault", -0.5m);
            Assert.AreEqual<decimal>(-0.5m, value);
        }

        [TestMethod]
        public void OptionalDecimal_Negative_WithInput_EmptyString_NoDefault()
        {
            List<string> data = new List<string>() { string.Empty };
            int index = 0;

            decimal value = DataParser.OptionalDecimal_Negative(data, index, "OptionalDecimal_Negative_WithInput_EmptyString_NoDefault");
            Assert.AreEqual<decimal>(-1.0m, value);
        }

        [TestMethod]
        public void OptionalDecimal_Negative_WithInput_EmptyString_WithDefault()
        {
            List<string> data = new List<string>() { string.Empty };
            int index = 0;

            decimal value = DataParser.OptionalDecimal_Negative(data, index, "OptionalDecimal_Negative_WithInput_EmptyString_WithDefault", -0.5m);
            Assert.AreEqual<decimal>(-0.5m, value);
        }

        [TestMethod]
        public void OptionalDecimal_Negative_WithInput_Whitespace_NoDefault()
        {
            List<string> data = new List<string>() { "   " };
            int index = 0;

            decimal value = DataParser.OptionalDecimal_Negative(data, index, "OptionalDecimal_Negative_WithInput_Whitespace_NoDefault");
            Assert.AreEqual<decimal>(-1.0m, value);
        }

        [TestMethod]
        public void OptionalDecimal_Negative_WithInput_Whitespace_WithDefault()
        {
            List<string> data = new List<string>() { "   " };
            int index = 0;

            decimal value = DataParser.OptionalDecimal_Negative(data, index, "OptionalDecimal_Negative_WithInput_Whitespace_WithDefault", -0.5m);
            Assert.AreEqual<decimal>(-0.5m, value);
        }

        [TestMethod]
        public void OptionalDecimal_Negative_WithInput_0()
        {
            List<string> data = new List<string>() { "0" };
            int index = 0;

            Assert.ThrowsException<NegativeDecimalException>(() => DataParser.OptionalDecimal_Negative(data, index, "OptionalDecimal_Negative_WithInput_0"));
        }

        [TestMethod]
        public void OptionalDecimal_Negative_WithInput_1()
        {
            List<string> data = new List<string>() { "1" };
            int index = 0;

            Assert.ThrowsException<NegativeDecimalException>(() => DataParser.OptionalDecimal_Negative(data, index, "OptionalDecimal_Negative_WithInput_1"));
        }

        [TestMethod]
        public void OptionalDecimal_Negative_WithInput_Neg1()
        {
            List<string> data = new List<string>() { "-1" };
            int index = 0;

            decimal value = DataParser.OptionalDecimal_Negative(data, index, "OptionalDecimal_Negative_WithInput_Neg1");
            Assert.AreEqual<decimal>(-1.0m, value);
        }

        #endregion OptionalDecimal_Negative
    }
}