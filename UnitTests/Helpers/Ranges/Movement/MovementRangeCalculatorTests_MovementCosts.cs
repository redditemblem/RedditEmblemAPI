using NSubstitute;
using RedditEmblemAPI.Helpers.Ranges;
using RedditEmblemAPI.Models.Configuration.Map;
using RedditEmblemAPI.Models.Output;
using RedditEmblemAPI.Models.Output.Map;
using RedditEmblemAPI.Models.Output.Map.Tiles;
using RedditEmblemAPI.Models.Output.System;
using RedditEmblemAPI.Models.Output.System.Skills;
using RedditEmblemAPI.Models.Output.Units;

namespace UnitTests.Helpers.Ranges.Movement
{
    public class MovementRangeCalculatorTests_MovementCosts
    {
        #region Constants

        private const string MOVEMENT_STAT_NAME = "Mov";
        private const string MOVEMENT_TYPE_INFANTRY = "Infantry";

        #endregion Constants

        #region SetUp

        /// <summary>
        /// SetUp() constructs this as a 1x9 map of tiles.
        /// <code>
        /// x □ □ □ □ □ □
        /// </code>
        /// </summary>
        private IMapObj Map;
        /// <summary>
        /// SetUp() places this size 1 unit at {1,1} on <c>Map</c>.
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

            IDictionary<string, int> moveCost_2 = new Dictionary<string, int>();
            moveCost_2.Add(MOVEMENT_TYPE_INFANTRY, 2);
            ITerrainTypeStats moveCost2Stats = Substitute.For<ITerrainTypeStats>();
            moveCost2Stats.MovementCosts.Returns(moveCost_2);

            IDictionary<string, int> moveCost_3 = new Dictionary<string, int>();
            moveCost_3.Add(MOVEMENT_TYPE_INFANTRY, 3);
            ITerrainTypeStats moveCost3Stats = Substitute.For<ITerrainTypeStats>();
            moveCost3Stats.MovementCosts.Returns(moveCost_3);

            ITerrainType plains = Substitute.For<ITerrainType>();
            plains.GetTerrainTypeStatsByAffiliation(affiliation).Returns(moveCost1Stats);
            plains.WarpType.Returns(WarpType.None);
            plains.CannotStopOn.Returns(false);
            plains.RestrictAffiliations.Returns(new List<int>());

            ITerrainType traversableOnly = Substitute.For<ITerrainType>();
            traversableOnly.GetTerrainTypeStatsByAffiliation(affiliation).Returns(moveCost1Stats);
            traversableOnly.WarpType.Returns(WarpType.None);
            traversableOnly.CannotStopOn.Returns(true);
            traversableOnly.RestrictAffiliations.Returns(new List<int>());

            ITerrainType forest = Substitute.For<ITerrainType>();
            forest.GetTerrainTypeStatsByAffiliation(affiliation).Returns(moveCost2Stats);
            forest.WarpType.Returns(WarpType.None);
            forest.CannotStopOn.Returns(false);
            forest.RestrictAffiliations.Returns(new List<int>());

            ITerrainType mountain = Substitute.For<ITerrainType>();
            mountain.GetTerrainTypeStatsByAffiliation(affiliation).Returns(moveCost3Stats);
            mountain.WarpType.Returns(WarpType.None);
            mountain.CannotStopOn.Returns(false);
            mountain.RestrictAffiliations.Returns(new List<int>());

            ITile[][] tiles = new ITile[1][];
            tiles[0] = new ITile[9];
            for (int c = 0; c < 9; c++)
            {
                ITile tile = Substitute.For<ITile>();
                tile.Coordinate.X.Returns(c + 1);
                tile.Coordinate.Y.Returns(1);
                tile.UnitData.Unit = null;
                tile.UnitData.UnitsAffectingMovementCosts.Returns(new List<IUnit>());
                tile.UnitData.UnitsObstructingMovement.Returns(new List<IUnit>());

                tiles[0][c] = tile;
            }

            tiles[0][0].TerrainType.Returns(plains);
            tiles[0][1].TerrainType.Returns(plains);
            tiles[0][2].TerrainType.Returns(forest);
            tiles[0][3].TerrainType.Returns(mountain);
            tiles[0][4].TerrainType.Returns(forest);
            tiles[0][5].TerrainType.Returns(forest);
            tiles[0][6].TerrainType.Returns(plains);
            tiles[0][7].TerrainType.Returns(traversableOnly);
            tiles[0][8].TerrainType.Returns(plains);

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

            ITile unitOrigin = this.Map.Segments[0].Tiles[0][0];
            IUnit unit = Substitute.For<IUnit>();
            unit.Affiliation.Returns(affiliation);
            unit.Location.UnitSize.Returns(1);
            unit.Location.OriginTiles.Returns(new List<ITile> { unitOrigin });
            unit.GetFullSkillsList().Returns(new List<ISkill>());
            unit.StatusConditions.Returns(new List<IUnitStatus>());
            unit.Ranges.Movement.Returns(new List<ICoordinate>());
            unit.GetUnitMovementType().Returns(MOVEMENT_TYPE_INFANTRY);

            this.Unit = unit;
        }

        #endregion SetUp

        [TestCase(0, 0)] //origin
        [TestCase(1, 1)] //1st plains
        [TestCase(2, 1)] 
        [TestCase(3, 2)] //1st forest
        [TestCase(4, 2)]
        [TestCase(5, 2)]
        [TestCase(6, 3)] //1st mountain
        [TestCase(7, 3)]
        [TestCase(8, 4)] //2nd forest
        [TestCase(9, 4)] 
        [TestCase(10, 5)] //3rd forest
        [TestCase(11, 6)] //2nd plains
        [TestCase(12, 6)] //Cannot stop on
        [TestCase(13, 7)] //3rd plains
        [TestCase(14, 7)] //more move than needed
        [TestCase(15, 7)]
        public void MovementRangeCalculator_MovementCosts(int movement, int expectedNumberOfTilesMoved)
        {
            IModifiedStatValue mov = Substitute.For<IModifiedStatValue>();
            mov.FinalValue.Returns(movement);
            this.Unit.Stats.MatchGeneralStatName(MOVEMENT_STAT_NAME).Returns(mov);

            Assert.That(this.Unit.Ranges.Movement, Is.Empty);

            MovementRangeCalculator calc = new MovementRangeCalculator(this.Map, new List<IUnit> { this.Unit });
            calc.CalculateUnitMovementRanges();

            Assert.That(this.Unit.Ranges.Movement, Is.Not.Empty);

            IList<ICoordinate> expected = new List<ICoordinate>();
            ITile[][] tiles = Map.Segments[0].Tiles;

            int i = 1;
            for (int c = 0; c < expectedNumberOfTilesMoved+i && c < tiles[0].Length; c++)
            {
                if (!tiles[0][c].TerrainType.CannotStopOn)
                    expected.Add(tiles[0][c].Coordinate);
                else i++;
            }

            Assert.That(this.Unit.Ranges.Movement, Is.EquivalentTo(expected));
        }
    }
}