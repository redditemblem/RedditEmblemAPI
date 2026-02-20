using NSubstitute;
using RedditEmblemAPI.Models.Configuration.Map;
using RedditEmblemAPI.Models.Output.Map;

namespace UnitTests.Models.Map
{
    public class MapSegmentTests
    {
        #region SetUp

        private IImageLoader ImageLoader;

        [SetUp]
        public void SetUp()
        {
            this.ImageLoader = Substitute.For<IImageLoader>();
        }

        #endregion SetUp

        [TestCase(16, false, false, 80, 96, 5, 6)]
        [TestCase(16, true, false, 80, 96, 4, 5)]
        [TestCase(16, false, true, 80, 96, 4, 5)]
        [TestCase(16, true, true, 80, 96, 3, 4)]
        [TestCase(16, false, false, 79, 95, 4, 5)] //horizontal and vertical dimensions 1 pixel short
        [TestCase(16, false, false, 81, 97, 5, 6)] //horizontal and vertical dimensions 1 pixel over
        public void Constructor(int tileSize, bool hasHeaderTopLeft, bool hasHeaderBottomRight, int imageHeightInPixels, int imageWidthInPixels, int expectedHeightInTiles, int expectedWidthInTiles)
        {
            MapConstantsConfig config = new MapConstantsConfig()
            {
                TileSize = tileSize,
                HasHeaderTopLeft = hasHeaderTopLeft,
                HasHeaderBottomRight = hasHeaderBottomRight
            };
            int beginningOfHorizontalRange = 1;

            this.ImageLoader.When(s => s.GetImageDimensionsByUrl(UnitTestConsts.IMAGE_URL, out Arg.Any<int>(), out Arg.Any<int>())).Do(s =>
            {
                s[1] = imageHeightInPixels;
                s[2] = imageWidthInPixels;
            });

            IMapSegment segment = new MapSegment(config, this.ImageLoader, UnitTestConsts.IMAGE_URL, beginningOfHorizontalRange);

            Assert.That(segment.ImageURL, Is.EqualTo(UnitTestConsts.IMAGE_URL));
            Assert.That(segment.HeightInPixels, Is.EqualTo(imageHeightInPixels));
            Assert.That(segment.WidthInPixels, Is.EqualTo(imageWidthInPixels));
            Assert.That(segment.HeightInTiles, Is.EqualTo(expectedHeightInTiles));
            Assert.That(segment.WidthInTiles, Is.EqualTo(expectedWidthInTiles));
            Assert.That(segment.HorizontalTileRangeWithinMap.Start.Value, Is.EqualTo(beginningOfHorizontalRange));
            Assert.That(segment.HorizontalTileRangeWithinMap.End.Value, Is.EqualTo(beginningOfHorizontalRange + expectedWidthInTiles - 1));
            Assert.That(segment.Tiles.Count(), Is.EqualTo(expectedHeightInTiles));
        }

        #region CoordinateFallsWithinRange

        public void CoordinateFallsWithinRange(int tileSize, bool hasHeaderTopLeft, bool hasHeaderBottomRight, int imageHeightInPixels, int imageWidthInPixels, int expectedHeightInTiles, int expectedWidthInTiles)
        {
            MapConstantsConfig config = new MapConstantsConfig()
            {
                TileSize = tileSize,
                HasHeaderTopLeft = hasHeaderTopLeft,
                HasHeaderBottomRight = hasHeaderBottomRight
            };
            int beginningOfHorizontalRange = 1;

            this.ImageLoader.When(s => s.GetImageDimensionsByUrl(UnitTestConsts.IMAGE_URL, out Arg.Any<int>(), out Arg.Any<int>())).Do(s =>
            {
                s[1] = imageHeightInPixels;
                s[2] = imageWidthInPixels;
            });

            IMapSegment segment = new MapSegment(config, this.ImageLoader, UnitTestConsts.IMAGE_URL, beginningOfHorizontalRange);

            Assert.That(segment.ImageURL, Is.EqualTo(UnitTestConsts.IMAGE_URL));
            Assert.That(segment.HeightInPixels, Is.EqualTo(imageHeightInPixels));
            Assert.That(segment.WidthInPixels, Is.EqualTo(imageWidthInPixels));
            Assert.That(segment.HeightInTiles, Is.EqualTo(expectedHeightInTiles));
            Assert.That(segment.WidthInTiles, Is.EqualTo(expectedWidthInTiles));
            Assert.That(segment.HorizontalTileRangeWithinMap.Start.Value, Is.EqualTo(beginningOfHorizontalRange));
            Assert.That(segment.HorizontalTileRangeWithinMap.End.Value, Is.EqualTo(beginningOfHorizontalRange + expectedWidthInTiles - 1));
            Assert.That(segment.Tiles.Count(), Is.EqualTo(expectedHeightInTiles));
        }

        #endregion CoordinateFallsWithinRange
    }
}
