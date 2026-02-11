using RedditEmblemAPI.Models.Configuration.Map;
using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.Map;

namespace UnitTests.Models.Map
{
    public class CoordinateTests
    {
        #region Constructor

        [Test]
        public void Constructor_NoParameters()
        {
            ICoordinate coord = new Coordinate();

            Assert.That(coord.X, Is.EqualTo(0));
            Assert.That(coord.Y, Is.EqualTo(0));
            Assert.That(coord.AsText, Is.Empty);
        }

        [TestCase(CoordinateFormat.XY, 0, 0, "")]
        [TestCase(CoordinateFormat.XY, 1, 1, "1,1")]
        [TestCase(CoordinateFormat.XY, 12, 12, "12,12")]
        [TestCase(CoordinateFormat.Alphanumerical, 0, 0, "")]
        [TestCase(CoordinateFormat.Alphanumerical, 1, 1, "A1")]
        [TestCase(CoordinateFormat.Alphanumerical, 2, 1, "B1")]
        [TestCase(CoordinateFormat.Alphanumerical, 26, 1, "Z1")]
        [TestCase(CoordinateFormat.Alphanumerical, 27, 1, "AA1")]
        public void Constructor_IntInput(CoordinateFormat format, int x, int y, string asText)
        {
            ICoordinate coord = new Coordinate(format, x, y);

            Assert.That(coord.X, Is.EqualTo(x));
            Assert.That(coord.Y, Is.EqualTo(y));
            Assert.That(coord.AsText, Is.EqualTo(asText));
        }

        [TestCase(CoordinateFormat.XY, "", 0, 0)]
        [TestCase(CoordinateFormat.XY, "1,1", 1, 1)]
        [TestCase(CoordinateFormat.XY, "12,12", 12, 12)]
        [TestCase(CoordinateFormat.Alphanumerical, "", 0, 0)]
        [TestCase(CoordinateFormat.Alphanumerical, "A1", 1, 1)]
        [TestCase(CoordinateFormat.Alphanumerical, "B1", 2, 1)]
        [TestCase(CoordinateFormat.Alphanumerical, "Z1", 26, 1)]
        [TestCase(CoordinateFormat.Alphanumerical, "AA1", 27, 1)]
        public void Constructor_StringInput(CoordinateFormat format, string input, int x, int y)
        {
            ICoordinate coord = new Coordinate(format, input);

            Assert.That(coord.X, Is.EqualTo(x));
            Assert.That(coord.Y, Is.EqualTo(y));
            Assert.That(coord.AsText, Is.EqualTo(input));
        }

        [TestCase(CoordinateFormat.XY)]
        [TestCase(CoordinateFormat.Alphanumerical)]
        public void Constructor_StringInput_Whitespace(CoordinateFormat format)
        {
            string input = UnitTestConsts.WHITESPACE_STRING;

            ICoordinate coord = new Coordinate(format, input);

            Assert.That(coord.X, Is.EqualTo(0));
            Assert.That(coord.Y, Is.EqualTo(0));
            Assert.That(coord.AsText, Is.Empty);
        }

        [TestCase("test")]
        [TestCase("0,0")] //out of bounds
        [TestCase("1")]
        [TestCase("1,")]
        [TestCase("1,1,1")]
        public void Constructor_StringInput_XY_InvalidInputs(string input)
        { 
            CoordinateFormat format = CoordinateFormat.XY;
            Assert.Throws<XYCoordinateFormattingException>(() => new Coordinate(format, input));
        }

        [TestCase("test")]
        [TestCase("A")]
        [TestCase("1")]
        [TestCase("A 1")]
        public void Constructor_StringInput_Alphanumeric_InvalidInputs(string input)
        {
            CoordinateFormat format = CoordinateFormat.Alphanumerical;
            Assert.Throws<AlphanumericCoordinateFormattingException>(() => new Coordinate(format, input));
        }

        [Test]
        public void Constructor_StringInput_Alphanumerical_AA1_Lowercase()
        {
            CoordinateFormat format = CoordinateFormat.Alphanumerical;
            string input = "aa1";

            ICoordinate coord = new Coordinate(format, input);

            Assert.That(coord.X, Is.EqualTo(27));
            Assert.That(coord.Y, Is.EqualTo(1));
            Assert.That(coord.AsText, Is.EqualTo("AA1"));
        }

        [TestCase(CoordinateFormat.XY, 1, 1)]
        [TestCase(CoordinateFormat.Alphanumerical, 1, 1)]
        public void Constructor_CoordinateInput(CoordinateFormat format, int x, int y)
        {
            ICoordinate input = new Coordinate(format, x, y);
            ICoordinate coord = new Coordinate(input);

            Assert.That(coord, Is.EqualTo(input));
        }

        #endregion Constructor

        #region DistanceFrom

        [Test]
        public void DistanceFrom_Self()
        {
            ICoordinate coord = new Coordinate();

            Assert.That(coord.DistanceFrom(coord), Is.EqualTo(0));
        }

        [TestCase(2, 2, 2, 1, 1)] //north
        [TestCase(2, 2, 3, 2, 1)] //east
        [TestCase(2, 2, 2, 3, 1)] //south
        [TestCase(2, 2, 1, 2, 1)] //west
        [TestCase(2, 2, 3, 1, 2)] //northeast
        [TestCase(2, 2, 1, 1, 2)] //northwest
        [TestCase(2, 2, 3, 3, 2)] //southeast
        [TestCase(2, 2, 1, 3, 2)] //southwest
        public void DistanceFrom(int x1, int y1, int x2, int y2, int expectedDistance)
        {
            ICoordinate coord1 = new Coordinate(CoordinateFormat.XY, x1, y1);
            ICoordinate coord2 = new Coordinate(CoordinateFormat.XY, x2, y2);

            Assert.That(coord1.DistanceFrom(coord2), Is.EqualTo(expectedDistance));
        }

        #endregion DistanceFrom

        #region ToString

        [Test]
        public void ToString_DefaultConstructor()
        {
            ICoordinate coord = new Coordinate();

            Assert.That(coord.ToString(), Is.Empty);
        }

        [TestCase(CoordinateFormat.XY, 1, 1, "1,1")]
        [TestCase(CoordinateFormat.Alphanumerical, 1, 1, "A1")]
        public void ToString_Constructor_IntInput(CoordinateFormat format, int x, int y, string expected)
        {
            ICoordinate coord = new Coordinate(format, x, y);

            Assert.That(coord.AsText, Is.EqualTo(expected));
            Assert.That(coord.ToString(), Is.EqualTo(expected));
        }

        [TestCase(CoordinateFormat.XY, "1,1")]
        [TestCase(CoordinateFormat.Alphanumerical, "A1")]
        public void ToString_Constructor_StringInput(CoordinateFormat format, string input)
        {
            ICoordinate coord = new Coordinate(format, input);

            Assert.That(coord.AsText, Is.EqualTo(input));
            Assert.That(coord.ToString(), Is.EqualTo(input));
        }

        #endregion ToString

        #region Equals

        [Test]
        public void Equals_Self()
        {
            Coordinate coord = new Coordinate(CoordinateFormat.XY, 0, 0);

            Assert.That(coord.Equals(coord), Is.True);
        }

        [Test]
        public void Equals_SameX()
        {
            Coordinate coord1 = new Coordinate(CoordinateFormat.XY, 0, 0);
            Coordinate coord2 = new Coordinate(CoordinateFormat.XY, 0, 1);

            Assert.That(coord1.Equals(coord2), Is.False);
        }

        [Test]
        public void Equals_SameY()
        {
            Coordinate coord1 = new Coordinate(CoordinateFormat.XY, 0, 0);
            Coordinate coord2 = new Coordinate(CoordinateFormat.XY, 1, 0);

            Assert.That(coord1.Equals(coord2), Is.False);
        }

        [Test]
        public void Equals_Different()
        {
            Coordinate coord1 = new Coordinate(CoordinateFormat.XY, 0, 0);
            Coordinate coord2 = new Coordinate(CoordinateFormat.XY, 1, 1);

            Assert.That(coord1.Equals(coord2), Is.False);
        }

        #endregion Equals
    }
}