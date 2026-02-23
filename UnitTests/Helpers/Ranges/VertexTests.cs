using NSubstitute;
using RedditEmblemAPI.Helpers.Ranges;
using RedditEmblemAPI.Models.Output.Map;
using RedditEmblemAPI.Models.Output.Map.Tiles;
using RedditEmblemAPI.Models.Output.System;

namespace UnitTests.Helpers.Ranges
{
    public class VertexTests
    {
        #region Constructor

        [Test]
        public void Constructor_EmptyTileList()
        {
            IEnumerable<ITile> tiles = new List<ITile>();

            IVertex vertex = new Vertex(tiles);

            Assert.That(vertex.Neighbors.Count(), Is.EqualTo(4));
            Assert.That(vertex.Tiles, Is.Empty);
            Assert.That(vertex.WarpEntrances, Is.Empty);
            Assert.That(vertex.IsTraversableOnly, Is.False);

            Assert.That(vertex.MinDistanceTo, Is.EqualTo(int.MaxValue));
            Assert.That(vertex.PathCost, Is.EqualTo(int.MaxValue));
            Assert.That(vertex.IsVisited, Is.False);
            Assert.That(vertex.IsTerminus, Is.False);
        }

        [Test]
        public void Constructor()
        {
            IEnumerable<ITile> tiles = new List<ITile>()
            {
                Substitute.For<ITile>()
            };

            IVertex vertex = new Vertex(tiles);

            Assert.That(vertex.Neighbors.Count(), Is.EqualTo(4));
            Assert.That(vertex.Tiles, Is.EqualTo(tiles));
            Assert.That(vertex.Tiles.Count(), Is.EqualTo(1));
            Assert.That(vertex.WarpEntrances, Is.Empty);
            Assert.That(vertex.IsTraversableOnly, Is.False);

            Assert.That(vertex.MinDistanceTo, Is.EqualTo(int.MaxValue));
            Assert.That(vertex.PathCost, Is.EqualTo(int.MaxValue));
            Assert.That(vertex.IsVisited, Is.False);
            Assert.That(vertex.IsTerminus, Is.False);
        }

        [Test]
        public void Constructor_WithWarpEntrances()
        {
            ITile entrance = Substitute.For<ITile>();
            entrance.TerrainType.WarpType.Returns(WarpType.Entrance);

            ITile dual = Substitute.For<ITile>();
            dual.TerrainType.WarpType.Returns(WarpType.Dual);

            ITile exit = Substitute.For<ITile>();
            exit.TerrainType.WarpType.Returns(WarpType.Exit);

            IEnumerable<ITile> tiles = new List<ITile>()
            {
                entrance,
                dual,
                exit
            };

            IVertex vertex = new Vertex(tiles);

            IEnumerable<ITile> expected = new List<ITile>() { entrance, dual };

            Assert.That(vertex.Tiles, Is.EqualTo(tiles));
            Assert.That(vertex.Tiles.Count(), Is.EqualTo(3));
            Assert.That(vertex.WarpEntrances, Is.EqualTo(expected));
            Assert.That(vertex.WarpEntrances.Count(), Is.EqualTo(2));
        }

        [Test]
        public void Constructor_IsTraversableOnly()
        {
            ITile traversable = Substitute.For<ITile>();
            traversable.TerrainType.CannotStopOn.Returns(false);

            ITile notTraversable = Substitute.For<ITile>();
            notTraversable.TerrainType.CannotStopOn.Returns(true);

            IEnumerable<ITile> tiles = new List<ITile>()
            {
                traversable,
                notTraversable
            };

            IVertex vertex = new Vertex(tiles);

            Assert.That(vertex.Tiles, Is.EqualTo(tiles));
            Assert.That(vertex.Tiles.Count(), Is.EqualTo(2));
            Assert.That(vertex.IsTraversableOnly, Is.True);
        }

        #endregion Constructor

        #region BuildVertexMap

        [Test]
        public void BuildVertexMap_UnitSize1_TwoSegments_3x3_2x2()
        {
            IMapObj map = Substitute.For<IMapObj>();
            IMapSegment segment1 = Substitute.For<IMapSegment>();
            IMapSegment segment2 = Substitute.For<IMapSegment>();
            map.Segments.Returns(new IMapSegment[] { segment1, segment2 });

            ITile tile1 = Substitute.For<ITile>();
            ITile tile2 = Substitute.For<ITile>();
            ITile tile3 = Substitute.For<ITile>();
            ITile tile4 = Substitute.For<ITile>();
            ITile tile5 = Substitute.For<ITile>();
            ITile tile6 = Substitute.For<ITile>();
            ITile tile7 = Substitute.For<ITile>();
            ITile tile8 = Substitute.For<ITile>();
            ITile tile9 = Substitute.For<ITile>();
            ITile tile10 = Substitute.For<ITile>();
            ITile tile11 = Substitute.For<ITile>();
            ITile tile12 = Substitute.For<ITile>();
            ITile tile13 = Substitute.For<ITile>();

            ITile[][] tiles1 = new ITile[3][]
            {
                new ITile[3]{ tile1, tile2, tile3 },
                new ITile[3]{ tile4, tile5, tile6 },
                new ITile[3]{ tile7, tile8, tile9 }
            };
            segment1.Tiles.Returns(tiles1);

            ITile[][] tiles2 = new ITile[2][]
            {
                new ITile[2]{ tile10, tile11 },
                new ITile[2]{ tile12, tile13 }
            };
            segment2.Tiles.Returns(tiles2);

            IList<IVertex> vertices = Vertex.BuildVertexMap(map, 1);

            Assert.That(vertices.Count, Is.EqualTo(13));

            //Validate each vertex individually :(
            IVertex vertex = vertices[0];
            Assert.That(vertex.Tiles.Count(), Is.EqualTo(1));
            Assert.That(vertex.Tiles.Contains(tile1), Is.True);
            Assert.That(vertex.Neighbors, Is.EqualTo(new IVertex[] { null, vertices[1], vertices[3], null }));

            vertex = vertices[1];
            Assert.That(vertex.Tiles.Count(), Is.EqualTo(1));
            Assert.That(vertex.Tiles.Contains(tile2), Is.True);
            Assert.That(vertex.Neighbors, Is.EqualTo(new IVertex[] { null, vertices[2], vertices[4], vertices[0] }));

            vertex = vertices[2];
            Assert.That(vertex.Tiles.Count(), Is.EqualTo(1));
            Assert.That(vertex.Tiles.Contains(tile3), Is.True);
            Assert.That(vertex.Neighbors, Is.EqualTo(new IVertex[] { null, null, vertices[5], vertices[1] }));

            vertex = vertices[3];
            Assert.That(vertex.Tiles.Count(), Is.EqualTo(1));
            Assert.That(vertex.Tiles.Contains(tile4), Is.True);
            Assert.That(vertex.Neighbors, Is.EqualTo(new IVertex[] { vertices[0], vertices[4], vertices[6], null }));

            vertex = vertices[4];
            Assert.That(vertex.Tiles.Count(), Is.EqualTo(1));
            Assert.That(vertex.Tiles.Contains(tile5), Is.True);
            Assert.That(vertex.Neighbors, Is.EqualTo(new IVertex[] { vertices[1], vertices[5], vertices[7], vertices[3] }));

            vertex = vertices[5];
            Assert.That(vertex.Tiles.Count(), Is.EqualTo(1));
            Assert.That(vertex.Tiles.Contains(tile6), Is.True);
            Assert.That(vertex.Neighbors, Is.EqualTo(new IVertex[] { vertices[2], null, vertices[8], vertices[4] }));

            vertex = vertices[6];
            Assert.That(vertex.Tiles.Count(), Is.EqualTo(1));
            Assert.That(vertex.Tiles.Contains(tile7), Is.True);
            Assert.That(vertex.Neighbors, Is.EqualTo(new IVertex[] { vertices[3], vertices[7], null, null }));

            vertex = vertices[7];
            Assert.That(vertex.Tiles.Count(), Is.EqualTo(1));
            Assert.That(vertex.Tiles.Contains(tile8), Is.True);
            Assert.That(vertex.Neighbors, Is.EqualTo(new IVertex[] { vertices[4], vertices[8], null, vertices[6] }));

            vertex = vertices[8];
            Assert.That(vertex.Tiles.Count(), Is.EqualTo(1));
            Assert.That(vertex.Tiles.Contains(tile9), Is.True);
            Assert.That(vertex.Neighbors, Is.EqualTo(new IVertex[] { vertices[5], null, null, vertices[7] }));

            vertex = vertices[9];
            Assert.That(vertex.Tiles.Count(), Is.EqualTo(1));
            Assert.That(vertex.Tiles.Contains(tile10), Is.True);
            Assert.That(vertex.Neighbors, Is.EqualTo(new IVertex[] { null, vertices[10], vertices[11], null }));

            vertex = vertices[10];
            Assert.That(vertex.Tiles.Count(), Is.EqualTo(1));
            Assert.That(vertex.Tiles.Contains(tile11), Is.True);
            Assert.That(vertex.Neighbors, Is.EqualTo(new IVertex[] { null, null, vertices[12], vertices[9] }));

            vertex = vertices[11];
            Assert.That(vertex.Tiles.Count(), Is.EqualTo(1));
            Assert.That(vertex.Tiles.Contains(tile12), Is.True);
            Assert.That(vertex.Neighbors, Is.EqualTo(new IVertex[] { vertices[9], vertices[12], null, null }));

            vertex = vertices[12];
            Assert.That(vertex.Tiles.Count(), Is.EqualTo(1));
            Assert.That(vertex.Tiles.Contains(tile13), Is.True);
            Assert.That(vertex.Neighbors, Is.EqualTo(new IVertex[] { vertices[10], null, null, vertices[11] }));
        }

        [Test]
        public void BuildVertexMap_UnitSize2_TwoSegments_3x3_2x2()
        {
            IMapObj map = Substitute.For<IMapObj>();
            IMapSegment segment1 = Substitute.For<IMapSegment>();
            IMapSegment segment2 = Substitute.For<IMapSegment>();
            map.Segments.Returns(new IMapSegment[] { segment1, segment2 });

            ITile tile1 = Substitute.For<ITile>();
            ITile tile2 = Substitute.For<ITile>();
            ITile tile3 = Substitute.For<ITile>();
            ITile tile4 = Substitute.For<ITile>();
            ITile tile5 = Substitute.For<ITile>();
            ITile tile6 = Substitute.For<ITile>();
            ITile tile7 = Substitute.For<ITile>();
            ITile tile8 = Substitute.For<ITile>();
            ITile tile9 = Substitute.For<ITile>();
            ITile tile10 = Substitute.For<ITile>();
            ITile tile11 = Substitute.For<ITile>();
            ITile tile12 = Substitute.For<ITile>();
            ITile tile13 = Substitute.For<ITile>();

            ITile[][] tiles1 = new ITile[3][]
            {
                new ITile[3]{ tile1, tile2, tile3 },
                new ITile[3]{ tile4, tile5, tile6 },
                new ITile[3]{ tile7, tile8, tile9 }
            };
            segment1.Tiles.Returns(tiles1);

            ITile[][] tiles2 = new ITile[2][]
            {
                new ITile[2]{ tile10, tile11 },
                new ITile[2]{ tile12, tile13 }
            };
            segment2.Tiles.Returns(tiles2);

            IList<IVertex> vertices = Vertex.BuildVertexMap(map, 2);

            Assert.That(vertices.Count, Is.EqualTo(5));

            //Validate each vertex individually
            IVertex vertex = vertices[0];
            Assert.That(vertex.Tiles, Is.EqualTo(new List<ITile>() { tile1, tile2, tile4, tile5 }));
            Assert.That(vertex.Neighbors, Is.EqualTo(new IVertex[] { null, vertices[1], vertices[2], null }));

            vertex = vertices[1];
            Assert.That(vertex.Tiles, Is.EqualTo(new List<ITile>() { tile2, tile3, tile5, tile6 }));
            Assert.That(vertex.Neighbors, Is.EqualTo(new IVertex[] { null, null, vertices[3], vertices[0] }));

            vertex = vertices[2];
            Assert.That(vertex.Tiles, Is.EqualTo(new List<ITile>() { tile4, tile5, tile7, tile8 }));
            Assert.That(vertex.Neighbors, Is.EqualTo(new IVertex[] { vertices[0], vertices[3], null, null }));

            vertex = vertices[3];
            Assert.That(vertex.Tiles, Is.EqualTo(new List<ITile>() { tile5, tile6, tile8, tile9 }));
            Assert.That(vertex.Neighbors, Is.EqualTo(new IVertex[] { vertices[1], null, null, vertices[2] }));

            vertex = vertices[4];
            Assert.That(vertex.Tiles, Is.EqualTo(new List<ITile>() { tile10, tile11, tile12, tile13 }));
            Assert.That(vertex.Neighbors, Is.EqualTo(new IVertex[] { null, null, null, null }));
        }

        [Test]
        public void BuildVertexMap_UnitSize3_SingleSegment_4x5()
        {
            IMapObj map = Substitute.For<IMapObj>();
            IMapSegment segment = Substitute.For<IMapSegment>();
            map.Segments.Returns(new IMapSegment[] { segment });

            ITile tile1 = Substitute.For<ITile>();
            ITile tile2 = Substitute.For<ITile>();
            ITile tile3 = Substitute.For<ITile>();
            ITile tile4 = Substitute.For<ITile>();
            ITile tile5 = Substitute.For<ITile>();
            ITile tile6 = Substitute.For<ITile>();
            ITile tile7 = Substitute.For<ITile>();
            ITile tile8 = Substitute.For<ITile>();
            ITile tile9 = Substitute.For<ITile>();
            ITile tile10 = Substitute.For<ITile>();
            ITile tile11 = Substitute.For<ITile>();
            ITile tile12 = Substitute.For<ITile>();
            ITile tile13 = Substitute.For<ITile>();
            ITile tile14 = Substitute.For<ITile>();
            ITile tile15 = Substitute.For<ITile>();
            ITile tile16 = Substitute.For<ITile>();
            ITile tile17 = Substitute.For<ITile>();
            ITile tile18 = Substitute.For<ITile>();
            ITile tile19 = Substitute.For<ITile>();
            ITile tile20 = Substitute.For<ITile>();

            ITile[][] tiles = new ITile[5][]
            {
                new ITile[4]{ tile1, tile2, tile3, tile4 },
                new ITile[4]{ tile5, tile6, tile7, tile8 },
                new ITile[4]{ tile9, tile10, tile11, tile12 },
                new ITile[4]{ tile13, tile14, tile15, tile16 },
                new ITile[4]{ tile17, tile18, tile19, tile20 }
            };
            segment.Tiles.Returns(tiles);

            IList<IVertex> vertices = Vertex.BuildVertexMap(map, 3);

            Assert.That(vertices.Count, Is.EqualTo(6));

            //Validate each vertex individually
            IVertex vertex = vertices[0];
            Assert.That(vertex.Tiles, Is.EqualTo(new List<ITile>() { tile1, tile2, tile3, tile5, tile6, tile7, tile9, tile10, tile11 }));
            Assert.That(vertex.Neighbors, Is.EqualTo(new IVertex[] { null, vertices[1], vertices[2], null }));

            vertex = vertices[1];
            Assert.That(vertex.Tiles, Is.EqualTo(new List<ITile>() { tile2, tile3, tile4, tile6, tile7, tile8, tile10, tile11, tile12 }));
            Assert.That(vertex.Neighbors, Is.EqualTo(new IVertex[] { null, null, vertices[3], vertices[0] }));

            vertex = vertices[2];
            Assert.That(vertex.Tiles, Is.EqualTo(new List<ITile>() { tile5, tile6, tile7, tile9, tile10, tile11, tile13, tile14, tile15 }));
            Assert.That(vertex.Neighbors, Is.EqualTo(new IVertex[] { vertices[0], vertices[3], vertices[4], null }));

            vertex = vertices[3];
            Assert.That(vertex.Tiles, Is.EqualTo(new List<ITile>() { tile6, tile7, tile8, tile10, tile11, tile12, tile14, tile15, tile16 }));
            Assert.That(vertex.Neighbors, Is.EqualTo(new IVertex[] { vertices[1], null, vertices[5], vertices[2] }));

            vertex = vertices[4];
            Assert.That(vertex.Tiles, Is.EqualTo(new List<ITile>() { tile9, tile10, tile11, tile13, tile14, tile15, tile17, tile18, tile19 }));
            Assert.That(vertex.Neighbors, Is.EqualTo(new IVertex[] { vertices[2], vertices[5], null, null }));

            vertex = vertices[5];
            Assert.That(vertex.Tiles, Is.EqualTo(new List<ITile>() { tile10, tile11, tile12, tile14, tile15, tile16, tile18, tile19, tile20 }));
            Assert.That(vertex.Neighbors, Is.EqualTo(new IVertex[] { vertices[3], null, null, vertices[4] }));
        }

        [Test]
        public void BuildVertexMap_UnitSize2_TwoSegments_SegmentSmallerThanUnitSize()
        {
            IMapObj map = Substitute.For<IMapObj>();
            IMapSegment segment1 = Substitute.For<IMapSegment>();
            IMapSegment segment2 = Substitute.For<IMapSegment>();
            map.Segments.Returns(new IMapSegment[] { segment1, segment2 });

            ITile tile1 = Substitute.For<ITile>();
            ITile tile2 = Substitute.For<ITile>();
            ITile tile3 = Substitute.For<ITile>();
            ITile tile4 = Substitute.For<ITile>();
            ITile tile5 = Substitute.For<ITile>();

            ITile[][] tiles1 = new ITile[2][]
            {
                new ITile[2]{ tile1, tile2 },
                new ITile[2]{ tile3, tile4 }
            };
            segment1.Tiles.Returns(tiles1);

            ITile[][] tiles2 = new ITile[1][]
            {
                new ITile[1]{ tile5 }
            };
            segment2.Tiles.Returns(tiles2);

            IList<IVertex> vertices = Vertex.BuildVertexMap(map, 2);

            Assert.That(vertices.Count, Is.EqualTo(1));

            IVertex vertex = vertices[0];
            Assert.That(vertex.Tiles, Is.EqualTo(new List<ITile>() { tile1, tile2, tile3, tile4 }));
            Assert.That(vertex.Neighbors, Is.EqualTo(new IVertex[] { null, null, null, null }));
        }

        #endregion BuildVertexMap
    }
}
