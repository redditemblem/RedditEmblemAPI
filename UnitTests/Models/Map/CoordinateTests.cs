using RedditEmblemAPI.Models.Configuration.Map;
using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.Map;

namespace UnitTests.Models.Map
{
    [TestClass]
    public class CoordinateTests
    {
        [TestMethod]
        public void CoordinateConstructor_NoParameters()
        {
            Coordinate coord = new Coordinate();
            Assert.AreEqual<int>(0, coord.X);
            Assert.AreEqual<int>(0, coord.Y);
            Assert.AreEqual<string>(string.Empty, coord.AsText);
        }

        #region CoordinateConstructor_IntInput

        [TestMethod]
        public void CoordinateConstructor_IntInput_XYFormat_0_0()
        {
            CoordinateFormat format = CoordinateFormat.XY;
            int x = 0;
            int y = 0;

            Coordinate coord = new Coordinate(format, x, y);
            Assert.AreEqual<int>(x, coord.X);
            Assert.AreEqual<int>(y, coord.Y);
            Assert.AreEqual<string>(string.Empty, coord.AsText);
        }

        [TestMethod]
        public void CoordinateConstructor_IntInput_XYFormat_1_1()
        {
            CoordinateFormat format = CoordinateFormat.XY;
            int x = 1;
            int y = 1;

            Coordinate coord = new Coordinate(format, x, y);
            Assert.AreEqual<int>(x, coord.X);
            Assert.AreEqual<int>(y, coord.Y);
            Assert.AreEqual<string>("1,1", coord.AsText);
        }

        [TestMethod]
        public void CoordinateConstructor_IntInput_AlphanumericFormat_0_0()
        {
            CoordinateFormat format = CoordinateFormat.Alphanumerical;
            int x = 0;
            int y = 0;

            Coordinate coord = new Coordinate(format, x, y);
            Assert.AreEqual<int>(x, coord.X);
            Assert.AreEqual<int>(y, coord.Y);
            Assert.AreEqual<string>(string.Empty, coord.AsText);
        }

        [TestMethod]
        public void CoordinateConstructor_IntInput_AlphanumericFormat_A1()
        {
            CoordinateFormat format = CoordinateFormat.Alphanumerical;
            int x = 1;
            int y = 1;

            Coordinate coord = new Coordinate(format, x, y);
            Assert.AreEqual<int>(x, coord.X);
            Assert.AreEqual<int>(y, coord.Y);
            Assert.AreEqual<string>("A1", coord.AsText);
        }

        [TestMethod]
        public void CoordinateConstructor_IntInput_AlphanumericFormat_B1()
        {
            CoordinateFormat format = CoordinateFormat.Alphanumerical;
            int x = 2;
            int y = 1;

            Coordinate coord = new Coordinate(format, x, y);
            Assert.AreEqual<int>(x, coord.X);
            Assert.AreEqual<int>(y, coord.Y);
            Assert.AreEqual<string>("B1", coord.AsText);
        }

        [TestMethod]
        public void CoordinateConstructor_IntInput_AlphanumericFormat_Z1()
        {
            CoordinateFormat format = CoordinateFormat.Alphanumerical;
            int x = 26;
            int y = 1;

            Coordinate coord = new Coordinate(format, x, y);
            Assert.AreEqual<int>(x, coord.X);
            Assert.AreEqual<int>(y, coord.Y);
            Assert.AreEqual<string>("Z1", coord.AsText);
        }

        [TestMethod]
        public void CoordinateConstructor_IntInput_AlphanumericFormat_AA1()
        {
            CoordinateFormat format = CoordinateFormat.Alphanumerical;
            int x = 27;
            int y = 1;

            Coordinate coord = new Coordinate(format, x, y);
            Assert.AreEqual<int>(x, coord.X);
            Assert.AreEqual<int>(y, coord.Y);
            Assert.AreEqual<string>("AA1", coord.AsText);
        }

        #endregion CoordinateConstructor_IntInput

        #region CoordianteConstructor_StringInput

        [TestMethod]
        public void CoordinateConstructor_StringInput_XY_EmptyString()
        {
            CoordinateFormat format = CoordinateFormat.XY;
            string input = string.Empty;

            Coordinate coord = new Coordinate(format, input);
            Assert.AreEqual<int>(0, coord.X);
            Assert.AreEqual<int>(0, coord.Y);
            Assert.AreEqual<string>(input, coord.AsText);
        }

        [TestMethod]
        public void CoordinateConstructor_StringInput_XY_Whitespace()
        {
            CoordinateFormat format = CoordinateFormat.XY;
            string input = UnitTestConsts.WHITESPACE_STRING;

            Coordinate coord = new Coordinate(format, input);
            Assert.AreEqual<int>(0, coord.X);
            Assert.AreEqual<int>(0, coord.Y);
            Assert.AreEqual<string>(string.Empty, coord.AsText);
        }

        [TestMethod]
        public void CoordinateConstructor_StringInput_XY_Test()
        {
            CoordinateFormat format = CoordinateFormat.XY;
            string input = "test";

            Assert.ThrowsException<XYCoordinateFormattingException>(() => new Coordinate(format, input));
        }

        [TestMethod]
        public void CoordinateConstructor_StringInput_XY_0_0()
        {
            CoordinateFormat format = CoordinateFormat.XY;
            string input = "0,0";

            Assert.ThrowsException<XYCoordinateFormattingException>(() => new Coordinate(format, input));

        }

        [TestMethod]
        public void CoordinateConstructor_StringInput_XY_NoComma()
        {
            CoordinateFormat format = CoordinateFormat.XY;
            string input = "1";

            Assert.ThrowsException<XYCoordinateFormattingException>(() => new Coordinate(format, input));
        }

        [TestMethod]
        public void CoordinateConstructor_StringInput_XY_CommaAndBlank()
        {
            CoordinateFormat format = CoordinateFormat.XY;
            string input = "1,";

            Assert.ThrowsException<XYCoordinateFormattingException>(() => new Coordinate(format, input));
        }

        [TestMethod]
        public void CoordinateConstructor_StringInput_XY_TwoCommas()
        {
            CoordinateFormat format = CoordinateFormat.XY;
            string input = "1,1,1";

            Assert.ThrowsException<XYCoordinateFormattingException>(() => new Coordinate(format, input));
        }

        [TestMethod]
        public void CoordinateConstructor_StringInput_XY_1_1()
        {
            CoordinateFormat format = CoordinateFormat.XY;
            string input = "1,1";

            Coordinate coord = new Coordinate(format, input);
            Assert.AreEqual<int>(1, coord.X);
            Assert.AreEqual<int>(1, coord.Y);
            Assert.AreEqual<string>(input, coord.AsText);
        }

        [TestMethod]
        public void CoordinateConstructor_StringInput_XY_12_12()
        {
            CoordinateFormat format = CoordinateFormat.XY;
            string input = "12,12";

            Coordinate coord = new Coordinate(format, input);
            Assert.AreEqual<int>(12, coord.X);
            Assert.AreEqual<int>(12, coord.Y);
            Assert.AreEqual<string>(input, coord.AsText);
        }

        [TestMethod]
        public void CoordinateConstructor_StringInput_Alphanumeric_EmptyString()
        {
            CoordinateFormat format = CoordinateFormat.Alphanumerical;
            string input = string.Empty;

            Coordinate coord = new Coordinate(format, input);
            Assert.AreEqual<int>(0, coord.X);
            Assert.AreEqual<int>(0, coord.Y);
            Assert.AreEqual<string>(input, coord.AsText);
        }

        [TestMethod]
        public void CoordinateConstructor_StringInput_Alphanumeric_Whitespace()
        {
            CoordinateFormat format = CoordinateFormat.Alphanumerical;
            string input = UnitTestConsts.WHITESPACE_STRING;

            Coordinate coord = new Coordinate(format, input);
            Assert.AreEqual<int>(0, coord.X);
            Assert.AreEqual<int>(0, coord.Y);
            Assert.AreEqual<string>(string.Empty, coord.AsText);
        }

        [TestMethod]
        public void CoordinateConstructor_StringInput_Alphanumeric_Test()
        {
            CoordinateFormat format = CoordinateFormat.Alphanumerical;
            string input = "test";

            Assert.ThrowsException<AlphanumericCoordinateFormattingException>(() => new Coordinate(format, input));
        }

        [TestMethod]
        public void CoordinateConstructor_StringInput_Alphanumerical_LetterOnly()
        {
            CoordinateFormat format = CoordinateFormat.Alphanumerical;
            string input = "A";

            Assert.ThrowsException<AlphanumericCoordinateFormattingException>(() => new Coordinate(format, input));
        }

        [TestMethod]
        public void CoordinateConstructor_StringInput_Alphanumerical_NumberOnly()
        {
            CoordinateFormat format = CoordinateFormat.Alphanumerical;
            string input = "1";

            Assert.ThrowsException<AlphanumericCoordinateFormattingException>(() => new Coordinate(format, input));
        }

        [TestMethod]
        public void CoordinateConstructor_StringInput_Alphanumerical_A1()
        {
            CoordinateFormat format = CoordinateFormat.Alphanumerical;
            string input = "A1";

            Coordinate coord = new Coordinate(format, input);
            Assert.AreEqual<int>(1, coord.X);
            Assert.AreEqual<int>(1, coord.Y);
            Assert.AreEqual<string>(input, coord.AsText);
        }

        [TestMethod]
        public void CoordinateConstructor_StringInput_Alphanumerical_A1_WithInnerSpace()
        {
            CoordinateFormat format = CoordinateFormat.Alphanumerical;
            string input = "A 1";

            Assert.ThrowsException<AlphanumericCoordinateFormattingException>(() => new Coordinate(format, input));
        }

        [TestMethod]
        public void CoordinateConstructor_StringInput_Alphanumerical_Z1()
        {
            CoordinateFormat format = CoordinateFormat.Alphanumerical;
            string input = "Z1";

            Coordinate coord = new Coordinate(format, input);
            Assert.AreEqual<int>(26, coord.X);
            Assert.AreEqual<int>(1, coord.Y);
            Assert.AreEqual<string>(input, coord.AsText);
        }

        [TestMethod]
        public void CoordinateConstructor_StringInput_Alphanumerical_AA1()
        {
            CoordinateFormat format = CoordinateFormat.Alphanumerical;
            string input = "AA1";

            Coordinate coord = new Coordinate(format, input);
            Assert.AreEqual<int>(27, coord.X);
            Assert.AreEqual<int>(1, coord.Y);
            Assert.AreEqual<string>(input, coord.AsText);
        }

        [TestMethod]
        public void CoordinateConstructor_StringInput_Alphanumerical_AA1_Lowercase()
        {
            CoordinateFormat format = CoordinateFormat.Alphanumerical;
            string input = "aa1";

            Coordinate coord = new Coordinate(format, input);
            Assert.AreEqual<int>(27, coord.X);
            Assert.AreEqual<int>(1, coord.Y);
            Assert.AreEqual<string>("AA1", coord.AsText);
        }

        #endregion CoordianteConstructor_StringInput

        #region CoordinateConstructor_CoordinateInput

        [TestMethod]
        public void CoordinateConstructor_CoordinateInput_XY()
        {
            Coordinate input = new Coordinate(CoordinateFormat.XY, 1, 1);
            Coordinate coord = new Coordinate(input);

            Assert.AreEqual<Coordinate>(input, coord);
        }

        [TestMethod]
        public void CoordinateConstructor_CoordinateInput_Alphanumeric()
        {
            Coordinate input = new Coordinate(CoordinateFormat.Alphanumerical, 1, 1);
            Coordinate coord = new Coordinate(input);

            Assert.AreEqual<Coordinate>(input, coord);
        }

        #endregion CoordinateConstructor_CoordinateInput

        #region DistanceFrom

        [TestMethod]
        public void Coordinate_DistanceFrom_Self()
        {
            Coordinate coord = new Coordinate();

            Assert.AreEqual<int>(0, coord.DistanceFrom(coord));
        }

        [TestMethod]
        public void Coordinate_DistanceFrom_North()
        {
            Coordinate coord1 = new Coordinate(CoordinateFormat.XY, 2, 2);
            Coordinate coord2 = new Coordinate(CoordinateFormat.XY, 2, 1);

            Assert.AreEqual<int>(1, coord1.DistanceFrom(coord2));
        }

        [TestMethod]
        public void Coordinate_DistanceFrom_East()
        {
            Coordinate coord1 = new Coordinate(CoordinateFormat.XY, 2, 2);
            Coordinate coord2 = new Coordinate(CoordinateFormat.XY, 3, 2);

            Assert.AreEqual<int>(1, coord1.DistanceFrom(coord2));
        }

        [TestMethod]
        public void Coordinate_DistanceFrom_South()
        {
            Coordinate coord1 = new Coordinate(CoordinateFormat.XY, 2, 2);
            Coordinate coord2 = new Coordinate(CoordinateFormat.XY, 2, 3);

            Assert.AreEqual<int>(1, coord1.DistanceFrom(coord2));
        }

        [TestMethod]
        public void Coordinate_DistanceFrom_West()
        {
            Coordinate coord1 = new Coordinate(CoordinateFormat.XY, 2, 2);
            Coordinate coord2 = new Coordinate(CoordinateFormat.XY, 1, 2);

            Assert.AreEqual<int>(1, coord1.DistanceFrom(coord2));
        }

        [TestMethod]
        public void Coordinate_DistanceFrom_Northeast()
        {
            Coordinate coord1 = new Coordinate(CoordinateFormat.XY, 2, 2);
            Coordinate coord2 = new Coordinate(CoordinateFormat.XY, 3, 1);

            Assert.AreEqual<int>(2, coord1.DistanceFrom(coord2));
        }

        [TestMethod]
        public void Coordinate_DistanceFrom_Northwest()
        {
            Coordinate coord1 = new Coordinate(CoordinateFormat.XY, 2, 2);
            Coordinate coord2 = new Coordinate(CoordinateFormat.XY, 1, 1);

            Assert.AreEqual<int>(2, coord1.DistanceFrom(coord2));
        }

        [TestMethod]
        public void Coordinate_DistanceFrom_Southeast()
        {
            Coordinate coord1 = new Coordinate(CoordinateFormat.XY, 2, 2);
            Coordinate coord2 = new Coordinate(CoordinateFormat.XY, 3, 3);

            Assert.AreEqual<int>(2, coord1.DistanceFrom(coord2));
        }

        [TestMethod]
        public void Coordinate_DistanceFrom_Southwest()
        {
            Coordinate coord1 = new Coordinate(CoordinateFormat.XY, 2, 2);
            Coordinate coord2 = new Coordinate(CoordinateFormat.XY, 1, 3);

            Assert.AreEqual<int>(2, coord1.DistanceFrom(coord2));
        }

        #endregion DistanceFrom

        #region Equals

        [TestMethod]
        public void Coordinate_Equals_Self()
        {
            Coordinate coord = new Coordinate(CoordinateFormat.XY, 0, 0);

            Assert.IsTrue(coord.Equals(coord));
        }

        [TestMethod]
        public void Coordinate_Equals_SameX()
        {
            Coordinate coord1 = new Coordinate(CoordinateFormat.XY, 0, 0);
            Coordinate coord2 = new Coordinate(CoordinateFormat.XY, 0, 1);

            Assert.IsFalse(coord1.Equals(coord2));
        }

        [TestMethod]
        public void Coordinate_Equals_SameY()
        {
            Coordinate coord1 = new Coordinate(CoordinateFormat.XY, 0, 0);
            Coordinate coord2 = new Coordinate(CoordinateFormat.XY, 1, 0);

            Assert.IsFalse(coord1.Equals(coord2));
        }

        [TestMethod]
        public void Coordinate_Equals_Different()
        {
            Coordinate coord1 = new Coordinate(CoordinateFormat.XY, 0, 0);
            Coordinate coord2 = new Coordinate(CoordinateFormat.XY, 1, 1);

            Assert.IsFalse(coord1.Equals(coord2));
        }

        #endregion Equals
    }
}