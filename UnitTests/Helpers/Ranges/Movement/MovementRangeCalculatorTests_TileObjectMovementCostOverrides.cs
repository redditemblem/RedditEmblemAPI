using NSubstitute;
using NSubstitute.ReturnsExtensions;
using RedditEmblemAPI.Helpers.Ranges.Movement;
using RedditEmblemAPI.Models.Configuration.Map;
using RedditEmblemAPI.Models.Output;
using RedditEmblemAPI.Models.Output.Map;
using RedditEmblemAPI.Models.Output.Map.Tiles;
using RedditEmblemAPI.Models.Output.System;
using RedditEmblemAPI.Models.Output.System.Skills;
using RedditEmblemAPI.Models.Output.Units;

namespace UnitTests.Helpers.Ranges.Movement
{
    public class MovementRangeCalculatorTests_TileObjectMovementCostOverrides
    {
        #region Constants

        private const string MOVEMENT_STAT_NAME = "Mov";
        private const string MOVEMENT_TYPE_INFANTRY = "Infantry";

        #endregion Constants

        #region SetUp

        /// <summary>
        /// SetUp() constructs this as a 1x4 map of tiles.
        /// <code>
        /// x □ □ □
        /// </code>
        /// </summary>
        private IMapObj Map;
        /// <summary>
        /// SetUp() places this size 1 unit at {1,1} on <c>Map</c>. Unit has 3 movement.
        /// </summary>
        private IUnit Unit;

        [SetUp]
        public void SetUp()
        {
            IAffiliation affiliation = Substitute.For<IAffiliation>();

            IDictionary<string, int> moveCost_1 = new Dictionary<string, int>();
            moveCost_1.Add(MOVEMENT_TYPE_INFANTRY, 1);
            ITerrainTypeStats moveCost1Stats = Substitute.For<ITerrainTypeStats>();
            moveCost1Stats.MovementCosts.Returns(moveCost_1);

            ITerrainType plains = Substitute.For<ITerrainType>();
            plains.GetTerrainTypeStatsByAffiliation(affiliation).Returns(moveCost1Stats);
            plains.WarpType.Returns(WarpType.None);
            plains.CannotStopOn.Returns(false);
            plains.RestrictAffiliations.Returns(new List<int>());

            ITile[][] tiles = new ITile[1][];
            tiles[0] = new ITile[4];
            for (int c = 0; c < 4; c++)
            {
                ITile tile = Substitute.For<ITile>();
                tile.Coordinate.X.Returns(c + 1);
                tile.Coordinate.Y.Returns(1);
                tile.TerrainType.Returns(plains);
                tile.UnitData.Unit = null;
                tile.UnitData.UnitsAffectingMovementCosts.Returns(new List<IUnit>());
                tile.UnitData.UnitsObstructingMovement.Returns(new List<IUnit>());
                tile.TileObjects.Returns(new List<ITileObjectInstance>());

                tiles[0][c] = tile;
            }

            IMapSegment segment = Substitute.For<IMapSegment>();
            segment.Tiles.Returns(tiles);

            MapConstantsConfig config = new MapConstantsConfig()
            {
                UnitMovementStatName = MOVEMENT_STAT_NAME
            };
            IMapObj map = Substitute.For<IMapObj>();
            map.Constants.Returns(config);
            map.Segments.Returns(new IMapSegment[1] { segment });

            this.Map = map;

            //Mock a unit
            ITile unitOrigin = this.Map.Segments[0].Tiles[0][0];
            IUnit unit = Substitute.For<IUnit>();
            unit.Affiliation.Returns(affiliation);
            unit.Location.UnitSize.Returns(1);
            unit.Location.OriginTiles.Returns(new List<ITile> { unitOrigin });
            unit.GetFullSkillsList().Returns(new List<ISkill>());
            unit.StatusConditions.Returns(new List<IUnitStatus>());
            unit.Ranges.MovementWithMinimumCost.Returns(new Dictionary<ICoordinate, int>());
            unit.GetUnitMovementType().Returns(MOVEMENT_TYPE_INFANTRY);

            IModifiedStatValue mov = Substitute.For<IModifiedStatValue>();
            mov.FinalValue.Returns(3);
            unit.Stats.MatchGeneralStatName(MOVEMENT_STAT_NAME).Returns(mov);

            this.Unit = unit;
        }

        #endregion SetUp

        //Control test that doesn't have a tile object
        [Test]
        public void MovementRangeCalculator_TileObjectMovementCostOverrides_NoTileObject()
        {
            Assert.That(this.Unit.Ranges.MovementWithMinimumCost, Is.Empty);

            MovementRangeCalculator calc = new MovementRangeCalculator(this.Map, new List<IUnit> { this.Unit });
            calc.CalculateUnitMovementRanges();

            Assert.That(this.Unit.Ranges.MovementWithMinimumCost, Is.Not.Empty);

            ITile[][] tiles = this.Map.Segments[0].Tiles;
            IList<ICoordinate> expected = new List<ICoordinate>
            {
                tiles[0][0].Coordinate,
                tiles[0][1].Coordinate,
                tiles[0][2].Coordinate,
                tiles[0][3].Coordinate
            };
            Assert.That(this.Unit.Ranges.MovementWithMinimumCost.Keys, Is.EquivalentTo(expected));
        }

        [Test]
        public void MovementRangeCalculator_TileObjectMovementCostOverrides_SingleTileObject_NullCostOverride()
        {
            ITileObjectInstance tileObjInstance = Substitute.For<ITileObjectInstance>();
            tileObjInstance.TileObject.MovementCostOverride.ReturnsNull();

            ITile secondTile = this.Map.Segments[0].Tiles[0][1];
            secondTile.TileObjects.Returns(new List<ITileObjectInstance> { tileObjInstance });

            Assert.That(this.Unit.Ranges.MovementWithMinimumCost, Is.Empty);

            MovementRangeCalculator calc = new MovementRangeCalculator(this.Map, new List<IUnit> { this.Unit });
            calc.CalculateUnitMovementRanges();

            Assert.That(this.Unit.Ranges.MovementWithMinimumCost, Is.Not.Empty);

            ITile[][] tiles = this.Map.Segments[0].Tiles;
            IList<ICoordinate> expected = new List<ICoordinate>
            {
                tiles[0][0].Coordinate,
                tiles[0][1].Coordinate,
                tiles[0][2].Coordinate,
                tiles[0][3].Coordinate
            };
            Assert.That(this.Unit.Ranges.MovementWithMinimumCost.Keys, Is.EquivalentTo(expected));
        }

        [TestCase(1, 4)]
        [TestCase(2, 3)]
        [TestCase(3, 2)]
        [TestCase(4, 1)] //cost exceeds unit movement
        [TestCase(99, 1)]
        public void MovementRangeCalculator_TileObjectMovementCostOverrides_SingleTileObject(int moveCostOverride, int expectedNumberOfTiles)
        {
            ITileObjectInstance tileObjInstance = Substitute.For<ITileObjectInstance>();
            tileObjInstance.TileObject.MovementCostOverride.Returns(moveCostOverride);

            ITile secondTile = this.Map.Segments[0].Tiles[0][1];
            secondTile.TileObjects.Returns(new List<ITileObjectInstance> { tileObjInstance });

            Assert.That(this.Unit.Ranges.MovementWithMinimumCost, Is.Empty);

            MovementRangeCalculator calc = new MovementRangeCalculator(this.Map, new List<IUnit> { this.Unit });
            calc.CalculateUnitMovementRanges();

            Assert.That(this.Unit.Ranges.MovementWithMinimumCost, Is.Not.Empty);

            ITile[][] tiles = this.Map.Segments[0].Tiles;
            IList<ICoordinate> expected = new List<ICoordinate>();
            for (int i = 0; i < expectedNumberOfTiles; i++)
                expected.Add(tiles[0][i].Coordinate);

            Assert.That(this.Unit.Ranges.MovementWithMinimumCost.Keys, Is.EquivalentTo(expected));
        }

        [TestCase(1, 1, 4)] //max value should always get picked, regardless of which tile object has it
        [TestCase(2, 1, 3)]
        [TestCase(1, 2, 3)]
        [TestCase(3, 1, 2)]
        [TestCase(1, 3, 2)]
        [TestCase(4, 1, 1)]
        [TestCase(1, 4, 1)]
        [TestCase(99, 1, 1)]
        [TestCase(1, 99, 1)]
        public void MovementRangeCalculator_TileObjectMovementCostOverrides_MultipleTileObjects(int moveCostOverride1, int moveCostOverride2, int expectedNumberOfTiles)
        {
            ITileObjectInstance tileObjInstance1 = Substitute.For<ITileObjectInstance>();
            tileObjInstance1.TileObject.MovementCostOverride.Returns(moveCostOverride1);

            ITileObjectInstance tileObjInstance2 = Substitute.For<ITileObjectInstance>();
            tileObjInstance2.TileObject.MovementCostOverride.Returns(moveCostOverride2);

            ITile secondTile = this.Map.Segments[0].Tiles[0][1];
            secondTile.TileObjects.Returns(new List<ITileObjectInstance> { tileObjInstance1, tileObjInstance2 });

            Assert.That(this.Unit.Ranges.MovementWithMinimumCost, Is.Empty);

            MovementRangeCalculator calc = new MovementRangeCalculator(this.Map, new List<IUnit> { this.Unit });
            calc.CalculateUnitMovementRanges();

            Assert.That(this.Unit.Ranges.MovementWithMinimumCost, Is.Not.Empty);

            ITile[][] tiles = this.Map.Segments[0].Tiles;
            IList<ICoordinate> expected = new List<ICoordinate>();
            for (int i = 0; i < expectedNumberOfTiles; i++)
                expected.Add(tiles[0][i].Coordinate);

            Assert.That(this.Unit.Ranges.MovementWithMinimumCost.Keys, Is.EquivalentTo(expected));
        }
    }
}
