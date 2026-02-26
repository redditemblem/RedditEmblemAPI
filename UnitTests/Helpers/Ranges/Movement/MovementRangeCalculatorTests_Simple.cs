using Antlr4.Runtime.Misc;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
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
    public class MovementRangeCalculatorTests_Simple
    {
        #region Constants

        private const string MOVEMENT_STAT_NAME = "Mov";
        private const string MOVEMENT_TYPE_INFANTRY = "Infantry";

        #endregion Constants

        #region SetUp

        /// <summary>
        /// SetUp() constructs this as a 5x5 map of plains tiles.
        /// <code>
        /// □ □ □ □ □
        /// □ □ □ □ □
        /// □ □ x □ □
        /// □ □ □ □ □
        /// □ □ □ □ □
        /// </code>
        /// </summary>
        private IMapObj Map;
        /// <summary>
        /// SetUp() places this size 1 unit at {2,2} on <c>Map</c>.
        /// </summary>
        private IUnit Unit;

        [SetUp]
        public void SetUp()
        {
            IAffiliation affiliation = Substitute.For<IAffiliation>();

            IDictionary<string, int> moveCosts = new Dictionary<string, int>();
            moveCosts.Add(MOVEMENT_TYPE_INFANTRY, 1);
            ITerrainTypeStats plainsStats = Substitute.For<ITerrainTypeStats>();
            plainsStats.MovementCosts.Returns(moveCosts);

            ITerrainType plains = Substitute.For<ITerrainType>();
            plains.GetTerrainTypeStatsByAffiliation(affiliation).Returns(plainsStats);
            plains.WarpType.Returns(WarpType.None);
            plains.CannotStopOn.Returns(false);
            plains.RestrictAffiliations.Returns(new List<int>());

            ITile[][] tiles = new ITile[5][];
            for (int r = 0; r < 5; r++)
            {
                tiles[r] = new ITile[5];
                for (int c = 0; c < 5; c++)
                {
                    ITile tile = Substitute.For<ITile>();
                    tile.Coordinate.X.Returns(c + 1);
                    tile.Coordinate.Y.Returns(r + 1);
                    tile.TerrainType.Returns(plains);
                    tile.UnitData.Unit = null;
                    tile.UnitData.UnitsAffectingMovementCosts.Returns(new List<IUnit>());
                    tile.UnitData.UnitsObstructingMovement.Returns(new List<IUnit>());

                    tiles[r][c] = tile;
                }
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

            ITile unitOrigin = this.Map.Segments[0].Tiles[2][2];
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

        [Test]
        public void MovementRangeCalculator_Simple_1Movement()
        {
            int movement = 1;

            IModifiedStatValue mov = Substitute.For<IModifiedStatValue>();
            mov.FinalValue.Returns(movement);
            this.Unit.Stats.MatchGeneralStatName(MOVEMENT_STAT_NAME).Returns(mov);

            Assert.That(this.Unit.Ranges.Movement, Is.Empty);

            MovementRangeCalculator calc = new MovementRangeCalculator(this.Map, new List<IUnit> { this.Unit });
            calc.CalculateUnitMovementRanges();

            Assert.That(this.Unit.Ranges.Movement, Is.Not.Empty);

            ITile[][] tiles = Map.Segments[0].Tiles;
            IList<ICoordinate> expected = new List<ICoordinate>()
            {
                tiles[1][2].Coordinate,
                tiles[2][1].Coordinate,
                tiles[2][2].Coordinate,
                tiles[2][3].Coordinate,
                tiles[3][2].Coordinate
            };
            Assert.That(this.Unit.Ranges.Movement, Is.EquivalentTo(expected));
        }

        [Test]
        public void MovementRangeCalculator_Simple_2Movement()
        {
            int movement = 2;

            IModifiedStatValue mov = Substitute.For<IModifiedStatValue>();
            mov.FinalValue.Returns(movement);
            this.Unit.Stats.MatchGeneralStatName(MOVEMENT_STAT_NAME).Returns(mov);

            Assert.That(this.Unit.Ranges.Movement, Is.Empty);

            MovementRangeCalculator calc = new MovementRangeCalculator(this.Map, new List<IUnit> { this.Unit });
            calc.CalculateUnitMovementRanges();

            Assert.That(this.Unit.Ranges.Movement, Is.Not.Empty);

            ITile[][] tiles = Map.Segments[0].Tiles;
            IList<ICoordinate> expected = new List<ICoordinate>()
            {
                tiles[0][2].Coordinate,
                tiles[1][1].Coordinate,
                tiles[1][2].Coordinate,
                tiles[1][3].Coordinate,
                tiles[2][0].Coordinate,
                tiles[2][1].Coordinate,
                tiles[2][2].Coordinate,
                tiles[2][3].Coordinate,
                tiles[2][4].Coordinate,
                tiles[3][1].Coordinate,
                tiles[3][2].Coordinate,
                tiles[3][3].Coordinate,
                tiles[4][2].Coordinate
            };
            Assert.That(this.Unit.Ranges.Movement, Is.EquivalentTo(expected));
        }

        [Test]
        public void MovementRangeCalculator_Simple_3Movement()
        {
            int movement = 3;

            IModifiedStatValue mov = Substitute.For<IModifiedStatValue>();
            mov.FinalValue.Returns(movement);
            this.Unit.Stats.MatchGeneralStatName(MOVEMENT_STAT_NAME).Returns(mov);

            Assert.That(this.Unit.Ranges.Movement, Is.Empty);

            MovementRangeCalculator calc = new MovementRangeCalculator(this.Map, new List<IUnit> { this.Unit });
            calc.CalculateUnitMovementRanges();

            Assert.That(this.Unit.Ranges.Movement, Is.Not.Empty);

            //Everything but the map corners
            ITile[][] tiles = Map.Segments[0].Tiles;
            IList<ICoordinate> expected = tiles.SelectMany(r => r)
                                               .Where(c => c != tiles[0][0] && c != tiles[0][4] && c != tiles[4][0] && c != tiles[4][4])
                                               .Select(t => t.Coordinate)
                                               .ToList();

            Assert.That(this.Unit.Ranges.Movement, Is.EquivalentTo(expected));
        }
    }
}
