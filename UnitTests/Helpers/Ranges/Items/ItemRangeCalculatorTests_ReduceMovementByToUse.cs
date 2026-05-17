using NSubstitute;
using RedditEmblemAPI.Helpers.Ranges;
using RedditEmblemAPI.Helpers.Ranges.Items;
using RedditEmblemAPI.Models.Configuration.Map;
using RedditEmblemAPI.Models.Output;
using RedditEmblemAPI.Models.Output.Map;
using RedditEmblemAPI.Models.Output.Map.Tiles;
using RedditEmblemAPI.Models.Output.System;
using RedditEmblemAPI.Models.Output.Units;

namespace UnitTests.Helpers.Ranges.Items
{
    internal class ItemRangeCalculatorTests_ReduceMovementByToUse
    {
        #region SetUp

        /// <summary>
        /// SetUp() constructs this as a 7x1 map of plains tiles.
        /// <code>
        /// □
        /// □
        /// □
        /// x
        /// □
        /// □
        /// □
        /// </code>
        /// </summary>
        private IMapObj Map;
        /// <summary>
        /// SetUp() places this size 1 unit at {4,1} on <c>Map</c>.
        /// </summary>
        private IUnit Unit;
        /// <summary>
        /// SetUp() creates this as a 1-range, standard range item.
        /// </summary>
        private IUnitInventoryItem Item;

        [SetUp]
        public void SetUp()
        {
            ITerrainType plains = Substitute.For<ITerrainType>();
            plains.BlocksItems.Returns(false);

            IMapObj map = Substitute.For<IMapObj>();

            MapConstantsConfig config = new MapConstantsConfig();
            config.UnitMovementStatName = "Mov";
            map.Constants.Returns(config);

            ITile[][] tiles = new ITile[7][];
            for (int r = 0; r < 7; r++)
            {
                tiles[r] = new ITile[1];
                for (int c = 0; c < 1; c++)
                {
                    ICoordinate coordinate = Substitute.For<ICoordinate>();
                    coordinate.X.Returns(c + 1);
                    coordinate.Y.Returns(r + 1);

                    ITile tile = Substitute.For<ITile>();
                    tile.Coordinate.Returns(coordinate);
                    tile.TerrainType.Returns(plains);
                    tile.UnitData.UnitsObstructingItems.Returns(new List<IUnit>());

                    ITile[] neighbors = new ITile[4];
                    if (r > 0)
                    {
                        ITile north = tiles[r - 1][c];
                        neighbors[(int)CardinalDirection.North] = north;
                        north.Neighbors[(int)CardinalDirection.South] = tile;
                    }
                    tile.Neighbors.Returns(neighbors);

                    tiles[r][c] = tile;

                    map.GetTileByCoord(coordinate).Returns(tile);
                }
            }

            IMapSegment segment = Substitute.For<IMapSegment>();
            segment.Tiles.Returns(tiles);
            map.Segments.Returns(new IMapSegment[1] { segment });

            this.Map = map;

            ITile unitOrigin = this.Map.Segments[0].Tiles[3][0];
            IUnit unit = Substitute.For<IUnit>();
            unit.Location.UnitSize.Returns(1);
            unit.Location.OriginTiles.Returns(new List<ITile> { unitOrigin });
            unit.StatusConditions.Returns(new List<IUnitStatus>());
            unit.Ranges.Attack.Returns(new List<ICoordinate>());
            unit.Ranges.Utility.Returns(new List<ICoordinate>());

            IItem item = Substitute.For<IItem>();
            item.DealsDamage.Returns(true);
            item.Range.Shape.Returns(ItemRangeShape.Standard);

            IUnitInventoryItem unitItem = Substitute.For<IUnitInventoryItem>();
            unitItem.CanEquip.Returns(true);
            unitItem.Item.Returns(item);
            unitItem.MinRange.FinalValue.Returns(1);
            unitItem.MaxRange.FinalValue.Returns(1);

            unit.Inventory.GetAllItems().Returns(new List<IUnitInventoryItem> { unitItem });

            this.Unit = unit;
            this.Item = unitItem;
        }

        #endregion SetUp

        [TestCase(0)]
        [TestCase(1)]
        public void UnitMovement0(int reduceMovementBy)
        {
            int unitMovement = 0;

            IModifiedStatValue moveStat = Substitute.For<IModifiedStatValue>();
            moveStat.FinalValue.Returns(unitMovement);
            this.Unit.Stats.MatchGeneralStatName("Mov").Returns(moveStat);

            ITile[][] tiles = this.Map.Segments[0].Tiles;
            IDictionary<ICoordinate, int> unitMovementRange = new Dictionary<ICoordinate, int> { 
                { tiles[3][0].Coordinate, 0 } 
            };
            this.Unit.Ranges.MovementWithMinimumCost.Returns(unitMovementRange);

            this.Item.Item.Range.ReduceMovementByToUse.Returns(reduceMovementBy);

            Assert.That(this.Unit.Ranges.Attack, Is.Empty);

            ItemRangeCalculator calc = new ItemRangeCalculator(this.Map, new IUnit[] { this.Unit });
            calc.CalculateUnitItemRanges();

            Assert.That(this.Unit.Ranges.Attack, Is.Not.Empty);

            IList<ICoordinate> expected = new List<ICoordinate>()
            {
                tiles[2][0].Coordinate,
                tiles[4][0].Coordinate
            };
            Assert.That(this.Unit.Ranges.Attack, Is.EquivalentTo(expected));
        }

        [Test]
        public void UnitMovement1_Reduction0()
        {
            int unitMovement = 1;
            int reduceMovementBy = 0;

            IModifiedStatValue moveStat = Substitute.For<IModifiedStatValue>();
            moveStat.FinalValue.Returns(unitMovement);
            this.Unit.Stats.MatchGeneralStatName("Mov").Returns(moveStat);

            ITile[][] tiles = this.Map.Segments[0].Tiles;
            IDictionary<ICoordinate, int> unitMovementRange = new Dictionary<ICoordinate, int> {
                { tiles[2][0].Coordinate, 1 },
                { tiles[3][0].Coordinate, 0 },
                { tiles[4][0].Coordinate, 1 }
            };
            this.Unit.Ranges.MovementWithMinimumCost.Returns(unitMovementRange);

            this.Item.Item.Range.ReduceMovementByToUse.Returns(reduceMovementBy);

            Assert.That(this.Unit.Ranges.Attack, Is.Empty);

            ItemRangeCalculator calc = new ItemRangeCalculator(this.Map, new IUnit[] { this.Unit });
            calc.CalculateUnitItemRanges();

            Assert.That(this.Unit.Ranges.Attack, Is.Not.Empty);

            IList<ICoordinate> expected = new List<ICoordinate>()
            {
                tiles[1][0].Coordinate,
                tiles[5][0].Coordinate
            };
            Assert.That(this.Unit.Ranges.Attack, Is.EquivalentTo(expected));
        }

        [Test]
        public void UnitMovement1_Reduction1()
        {
            int unitMovement = 1;
            int reduceMovementBy = 1;

            IModifiedStatValue moveStat = Substitute.For<IModifiedStatValue>();
            moveStat.FinalValue.Returns(unitMovement);
            this.Unit.Stats.MatchGeneralStatName("Mov").Returns(moveStat);

            ITile[][] tiles = this.Map.Segments[0].Tiles;
            IDictionary<ICoordinate, int> unitMovementRange = new Dictionary<ICoordinate, int> {
                { tiles[2][0].Coordinate, 1 },
                { tiles[3][0].Coordinate, 0 },
                { tiles[4][0].Coordinate, 1 }
            };
            this.Unit.Ranges.MovementWithMinimumCost.Returns(unitMovementRange);

            this.Item.Item.Range.ReduceMovementByToUse.Returns(reduceMovementBy);

            Assert.That(this.Unit.Ranges.Attack, Is.Empty);

            ItemRangeCalculator calc = new ItemRangeCalculator(this.Map, new IUnit[] { this.Unit });
            calc.CalculateUnitItemRanges();

            Assert.That(this.Unit.Ranges.Attack, Is.Empty);
        }

        [Test]
        public void UnitMovement2_Reduction0()
        {
            int unitMovement = 2;
            int reduceMovementBy = 0;

            IModifiedStatValue moveStat = Substitute.For<IModifiedStatValue>();
            moveStat.FinalValue.Returns(unitMovement);
            this.Unit.Stats.MatchGeneralStatName("Mov").Returns(moveStat);

            ITile[][] tiles = this.Map.Segments[0].Tiles;
            IDictionary<ICoordinate, int> unitMovementRange = new Dictionary<ICoordinate, int> {
                { tiles[1][0].Coordinate, 2 },
                { tiles[2][0].Coordinate, 1 },
                { tiles[3][0].Coordinate, 0 },
                { tiles[4][0].Coordinate, 1 },
                { tiles[5][0].Coordinate, 2 }
            };
            this.Unit.Ranges.MovementWithMinimumCost.Returns(unitMovementRange);

            this.Item.Item.Range.ReduceMovementByToUse.Returns(reduceMovementBy);

            Assert.That(this.Unit.Ranges.Attack, Is.Empty);

            ItemRangeCalculator calc = new ItemRangeCalculator(this.Map, new IUnit[] { this.Unit });
            calc.CalculateUnitItemRanges();

            Assert.That(this.Unit.Ranges.Attack, Is.Not.Empty);

            IList<ICoordinate> expected = new List<ICoordinate>()
            {
                tiles[0][0].Coordinate,
                tiles[6][0].Coordinate
            };
            Assert.That(this.Unit.Ranges.Attack, Is.EquivalentTo(expected));
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public void UnitMovement2_WithReduction(int reduceMovementBy)
        {
            int unitMovement = 2;

            IModifiedStatValue moveStat = Substitute.For<IModifiedStatValue>();
            moveStat.FinalValue.Returns(unitMovement);
            this.Unit.Stats.MatchGeneralStatName("Mov").Returns(moveStat);

            ITile[][] tiles = this.Map.Segments[0].Tiles;
            IDictionary<ICoordinate, int> unitMovementRange = new Dictionary<ICoordinate, int> {
                { tiles[1][0].Coordinate, 2 },
                { tiles[2][0].Coordinate, 1 },
                { tiles[3][0].Coordinate, 0 },
                { tiles[4][0].Coordinate, 1 },
                { tiles[5][0].Coordinate, 2 }
            };
            this.Unit.Ranges.MovementWithMinimumCost.Returns(unitMovementRange);

            this.Item.Item.Range.ReduceMovementByToUse.Returns(reduceMovementBy);

            Assert.That(this.Unit.Ranges.Attack, Is.Empty);

            ItemRangeCalculator calc = new ItemRangeCalculator(this.Map, new IUnit[] { this.Unit });
            calc.CalculateUnitItemRanges();

            Assert.That(this.Unit.Ranges.Attack, Is.Empty);
        }
    }
}
