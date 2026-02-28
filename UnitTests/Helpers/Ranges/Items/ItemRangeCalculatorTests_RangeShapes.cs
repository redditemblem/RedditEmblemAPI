using NSubstitute;
using RedditEmblemAPI.Helpers.Ranges;
using RedditEmblemAPI.Helpers.Ranges.Items;
using RedditEmblemAPI.Models.Configuration.Map;
using RedditEmblemAPI.Models.Output.Map;
using RedditEmblemAPI.Models.Output.Map.Tiles;
using RedditEmblemAPI.Models.Output.System;
using RedditEmblemAPI.Models.Output.System.Skills;
using RedditEmblemAPI.Models.Output.Units;

namespace UnitTests.Helpers.Ranges.Items
{
    public class ItemRangeCalculatorTests_RangeShapes
    {
        #region SetUp

        /// <summary>
        /// SetUp() constructs this as a 7x7 map of plains tiles.
        /// <code>
        /// □ □ □ □ □ □ □
        /// □ □ □ □ □ □ □
        /// □ □ □ □ □ □ □
        /// □ □ □ x □ □ □
        /// □ □ □ □ □ □ □
        /// □ □ □ □ □ □ □
        /// □ □ □ □ □ □ □
        /// </code>
        /// </summary>
        private IMapObj Map;
        /// <summary>
        /// SetUp() places this size 1 unit at {4,4} on <c>Map</c>.
        /// </summary>
        private IUnit Unit;
        private IUnitInventoryItem Item;

        [SetUp]
        public void SetUp()
        {
            ITerrainType plains = Substitute.For<ITerrainType>();
            plains.BlocksItems.Returns(false);

            IMapObj map = Substitute.For<IMapObj>();

            ITile[][] tiles = new ITile[7][];
            for (int r = 0; r < 7; r++)
            {
                tiles[r] = new ITile[7];
                for (int c = 0; c < 7; c++)
                {
                    ICoordinate coordinate = Substitute.For<ICoordinate>();
                    coordinate.X.Returns(c + 1);
                    coordinate.Y.Returns(r + 1);

                    ITile tile = Substitute.For<ITile>();
                    tile.Coordinate.Returns(coordinate);
                    tile.TerrainType.Returns(plains);
                    tile.UnitData.UnitsObstructingItems.Returns(new List<IUnit>());

                    ITile[] neighbors = new ITile[4];
                    if(r > 0)
                    {
                        ITile north = tiles[r - 1][c];
                        neighbors[(int)CardinalDirection.North] = north;
                        north.Neighbors[(int)CardinalDirection.South] = tile;
                    }
                    if(c > 0)
                    {
                        ITile west = tiles[r][c - 1];
                        neighbors[(int)CardinalDirection.West] = west;
                        west.Neighbors[(int)CardinalDirection.East] = tile;
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

            ITile unitOrigin = this.Map.Segments[0].Tiles[3][3];
            IUnit unit = Substitute.For<IUnit>();
            unit.Location.UnitSize.Returns(1);
            unit.Location.OriginTiles.Returns(new List<ITile> { unitOrigin });

            ICoordinate unitCoord = unitOrigin.Coordinate;
            unit.Ranges.Movement.Returns(new List<ICoordinate> { unitCoord });
            unit.Ranges.Attack.Returns(new List<ICoordinate>());
            unit.Ranges.Utility.Returns(new List<ICoordinate>());

            IItem item = Substitute.For<IItem>();
            item.DealsDamage.Returns(true);

            IUnitInventoryItem unitItem = Substitute.For<IUnitInventoryItem>();
            unitItem.CanEquip.Returns(true);
            unitItem.Item.Returns(item);

            unit.Inventory.GetAllItems().Returns(new List<IUnitInventoryItem> { unitItem });

            this.Unit = unit;
            this.Item = unitItem;
        }

        #endregion SetUp

        #region ItemRangeShape.Standard

        [Test]
        public void Standard_1Range()
        {
            int range = 1;
            this.Item.MinRange.FinalValue.Returns(range);
            this.Item.MaxRange.FinalValue.Returns(range);
            this.Item.Item.Range.Shape.Returns(ItemRangeShape.Standard);

            Assert.That(this.Unit.Ranges.Attack, Is.Empty);

            ItemRangeCalculator calc = new ItemRangeCalculator(this.Map, new IUnit[] { this.Unit });
            calc.CalculateUnitItemRanges();

            Assert.That(this.Unit.Ranges.Attack, Is.Not.Empty);

            ITile[][] tiles = this.Map.Segments[0].Tiles;
            IList<ICoordinate> expected = new List<ICoordinate>()
            {
                tiles[2][3].Coordinate,
                tiles[3][2].Coordinate,
                tiles[3][4].Coordinate,
                tiles[4][3].Coordinate
            };
            Assert.That(this.Unit.Ranges.Attack, Is.EquivalentTo(expected));
        }

        [Test]
        public void Standard_2Range()
        {
            int range = 2;
            this.Item.MinRange.FinalValue.Returns(range);
            this.Item.MaxRange.FinalValue.Returns(range);
            this.Item.Item.Range.Shape.Returns(ItemRangeShape.Standard);

            Assert.That(this.Unit.Ranges.Attack, Is.Empty);

            ItemRangeCalculator calc = new ItemRangeCalculator(this.Map, new IUnit[] { this.Unit });
            calc.CalculateUnitItemRanges();

            Assert.That(this.Unit.Ranges.Attack, Is.Not.Empty);

            ITile[][] tiles = this.Map.Segments[0].Tiles;
            IList<ICoordinate> expected = new List<ICoordinate>()
            {
                tiles[1][3].Coordinate,
                tiles[2][2].Coordinate,
                tiles[2][4].Coordinate,
                tiles[3][1].Coordinate,
                tiles[3][5].Coordinate,
                tiles[4][2].Coordinate,
                tiles[4][4].Coordinate,
                tiles[5][3].Coordinate
            };
            Assert.That(this.Unit.Ranges.Attack, Is.EquivalentTo(expected));
        }

        [Test]
        public void Standard_3Range()
        {
            int range = 3;
            this.Item.MinRange.FinalValue.Returns(range);
            this.Item.MaxRange.FinalValue.Returns(range);
            this.Item.Item.Range.Shape.Returns(ItemRangeShape.Standard);

            Assert.That(this.Unit.Ranges.Attack, Is.Empty);

            ItemRangeCalculator calc = new ItemRangeCalculator(this.Map, new IUnit[] { this.Unit });
            calc.CalculateUnitItemRanges();

            Assert.That(this.Unit.Ranges.Attack, Is.Not.Empty);

            ITile[][] tiles = this.Map.Segments[0].Tiles;
            IList<ICoordinate> expected = new List<ICoordinate>()
            {
                tiles[0][3].Coordinate,
                tiles[1][2].Coordinate,
                tiles[1][4].Coordinate,
                tiles[2][1].Coordinate,
                tiles[2][5].Coordinate,
                tiles[3][0].Coordinate,
                tiles[3][6].Coordinate,
                tiles[4][1].Coordinate,
                tiles[4][5].Coordinate,
                tiles[5][2].Coordinate,
                tiles[5][4].Coordinate,
                tiles[6][3].Coordinate
            };
            Assert.That(this.Unit.Ranges.Attack, Is.EquivalentTo(expected));
        }

        [Test]
        public void Standard_1_2Range()
        {
            int minRange = 1;
            int maxRange = 2;
            this.Item.MinRange.FinalValue.Returns(minRange);
            this.Item.MaxRange.FinalValue.Returns(maxRange);
            this.Item.Item.Range.Shape.Returns(ItemRangeShape.Standard);

            Assert.That(this.Unit.Ranges.Attack, Is.Empty);

            ItemRangeCalculator calc = new ItemRangeCalculator(this.Map, new IUnit[] { this.Unit });
            calc.CalculateUnitItemRanges();

            Assert.That(this.Unit.Ranges.Attack, Is.Not.Empty);

            ITile[][] tiles = this.Map.Segments[0].Tiles;
            IList<ICoordinate> expected = new List<ICoordinate>()
            {
                //1 range set
                tiles[2][3].Coordinate,
                tiles[3][2].Coordinate,
                tiles[3][4].Coordinate,
                tiles[4][3].Coordinate,
                //2 range set
                tiles[1][3].Coordinate,
                tiles[2][2].Coordinate,
                tiles[2][4].Coordinate,
                tiles[3][1].Coordinate,
                tiles[3][5].Coordinate,
                tiles[4][2].Coordinate,
                tiles[4][4].Coordinate,
                tiles[5][3].Coordinate
            };
            Assert.That(this.Unit.Ranges.Attack, Is.EquivalentTo(expected));
        }

        [Test]
        public void Standard_2_3Range()
        {
            int minRange = 2;
            int maxRange = 3;
            this.Item.MinRange.FinalValue.Returns(minRange);
            this.Item.MaxRange.FinalValue.Returns(maxRange);
            this.Item.Item.Range.Shape.Returns(ItemRangeShape.Standard);

            Assert.That(this.Unit.Ranges.Attack, Is.Empty);

            ItemRangeCalculator calc = new ItemRangeCalculator(this.Map, new IUnit[] { this.Unit });
            calc.CalculateUnitItemRanges();

            Assert.That(this.Unit.Ranges.Attack, Is.Not.Empty);

            ITile[][] tiles = this.Map.Segments[0].Tiles;
            IList<ICoordinate> expected = new List<ICoordinate>()
            {
                //2 range set
                tiles[1][3].Coordinate,
                tiles[2][2].Coordinate,
                tiles[2][4].Coordinate,
                tiles[3][1].Coordinate,
                tiles[3][5].Coordinate,
                tiles[4][2].Coordinate,
                tiles[4][4].Coordinate,
                tiles[5][3].Coordinate,
                //3 range set
                tiles[0][3].Coordinate,
                tiles[1][2].Coordinate,
                tiles[1][4].Coordinate,
                tiles[2][1].Coordinate,
                tiles[2][5].Coordinate,
                tiles[3][0].Coordinate,
                tiles[3][6].Coordinate,
                tiles[4][1].Coordinate,
                tiles[4][5].Coordinate,
                tiles[5][2].Coordinate,
                tiles[5][4].Coordinate,
                tiles[6][3].Coordinate

            };
            Assert.That(this.Unit.Ranges.Attack, Is.EquivalentTo(expected));
        }

        #endregion ItemRangeShape.Standard

        #region ItemRangeShape.Square

        [Test]
        public void Square_1Range()
        {
            int range = 1;
            this.Item.MinRange.FinalValue.Returns(range);
            this.Item.MaxRange.FinalValue.Returns(range);
            this.Item.Item.Range.Shape.Returns(ItemRangeShape.Square);

            Assert.That(this.Unit.Ranges.Attack, Is.Empty);

            ItemRangeCalculator calc = new ItemRangeCalculator(this.Map, new IUnit[] { this.Unit });
            calc.CalculateUnitItemRanges();

            Assert.That(this.Unit.Ranges.Attack, Is.Not.Empty);

            ITile[][] tiles = this.Map.Segments[0].Tiles;
            IList<ICoordinate> expected = new List<ICoordinate>()
            {   
                tiles[2][2].Coordinate,
                tiles[2][3].Coordinate,
                tiles[2][4].Coordinate,
                tiles[3][2].Coordinate,
                tiles[3][4].Coordinate,
                tiles[4][2].Coordinate,
                tiles[4][3].Coordinate,
                tiles[4][4].Coordinate
            };
            Assert.That(this.Unit.Ranges.Attack, Is.EquivalentTo(expected));
        }

        [Test]
        public void Square_2Range()
        {
            int range = 2;
            this.Item.MinRange.FinalValue.Returns(range);
            this.Item.MaxRange.FinalValue.Returns(range);
            this.Item.Item.Range.Shape.Returns(ItemRangeShape.Square);

            Assert.That(this.Unit.Ranges.Attack, Is.Empty);

            ItemRangeCalculator calc = new ItemRangeCalculator(this.Map, new IUnit[] { this.Unit });
            calc.CalculateUnitItemRanges();

            Assert.That(this.Unit.Ranges.Attack, Is.Not.Empty);

            ITile[][] tiles = this.Map.Segments[0].Tiles;
            IList<ICoordinate> expected = new List<ICoordinate>()
            {
                tiles[1][1].Coordinate,
                tiles[1][2].Coordinate,
                tiles[1][3].Coordinate,
                tiles[1][4].Coordinate,
                tiles[1][5].Coordinate,
                tiles[2][1].Coordinate,
                tiles[2][5].Coordinate,
                tiles[3][1].Coordinate,
                tiles[3][5].Coordinate,
                tiles[4][1].Coordinate,
                tiles[4][5].Coordinate,
                tiles[5][1].Coordinate,
                tiles[5][2].Coordinate,
                tiles[5][3].Coordinate,
                tiles[5][4].Coordinate,
                tiles[5][5].Coordinate
            };
            Assert.That(this.Unit.Ranges.Attack, Is.EquivalentTo(expected));
        }

        [Test]
        public void Square_3Range()
        {
            int range = 3;
            this.Item.MinRange.FinalValue.Returns(range);
            this.Item.MaxRange.FinalValue.Returns(range);
            this.Item.Item.Range.Shape.Returns(ItemRangeShape.Square);

            Assert.That(this.Unit.Ranges.Attack, Is.Empty);

            ItemRangeCalculator calc = new ItemRangeCalculator(this.Map, new IUnit[] { this.Unit });
            calc.CalculateUnitItemRanges();

            Assert.That(this.Unit.Ranges.Attack, Is.Not.Empty);

            ITile[][] tiles = this.Map.Segments[0].Tiles;
            IList<ICoordinate> expected = new List<ICoordinate>()
            {
                tiles[0][0].Coordinate,
                tiles[0][1].Coordinate,
                tiles[0][2].Coordinate,
                tiles[0][3].Coordinate,
                tiles[0][4].Coordinate,
                tiles[0][5].Coordinate,
                tiles[0][6].Coordinate,
                tiles[1][0].Coordinate,
                tiles[1][6].Coordinate,
                tiles[2][0].Coordinate,
                tiles[2][6].Coordinate,
                tiles[3][0].Coordinate,
                tiles[3][6].Coordinate,
                tiles[4][0].Coordinate,
                tiles[4][6].Coordinate,
                tiles[5][0].Coordinate,
                tiles[5][6].Coordinate,
                tiles[6][0].Coordinate,
                tiles[6][1].Coordinate,
                tiles[6][2].Coordinate,
                tiles[6][3].Coordinate,
                tiles[6][4].Coordinate,
                tiles[6][5].Coordinate,
                tiles[6][6].Coordinate
            };
            Assert.That(this.Unit.Ranges.Attack, Is.EquivalentTo(expected));
        }

        [Test]
        public void Square_1_2Range()
        {
            int minRange = 1;
            int maxRange = 2;
            this.Item.MinRange.FinalValue.Returns(minRange);
            this.Item.MaxRange.FinalValue.Returns(maxRange);
            this.Item.Item.Range.Shape.Returns(ItemRangeShape.Square);

            Assert.That(this.Unit.Ranges.Attack, Is.Empty);

            ItemRangeCalculator calc = new ItemRangeCalculator(this.Map, new IUnit[] { this.Unit });
            calc.CalculateUnitItemRanges();

            Assert.That(this.Unit.Ranges.Attack, Is.Not.Empty);

            ITile[][] tiles = this.Map.Segments[0].Tiles;
            IList<ICoordinate> expected = new List<ICoordinate>()
            {
                //1 range set
                tiles[2][2].Coordinate,
                tiles[2][3].Coordinate,
                tiles[2][4].Coordinate,
                tiles[3][2].Coordinate,
                tiles[3][4].Coordinate,
                tiles[4][2].Coordinate,
                tiles[4][3].Coordinate,
                tiles[4][4].Coordinate,
                //2 range set
                tiles[1][1].Coordinate,
                tiles[1][2].Coordinate,
                tiles[1][3].Coordinate,
                tiles[1][4].Coordinate,
                tiles[1][5].Coordinate,
                tiles[2][1].Coordinate,
                tiles[2][5].Coordinate,
                tiles[3][1].Coordinate,
                tiles[3][5].Coordinate,
                tiles[4][1].Coordinate,
                tiles[4][5].Coordinate,
                tiles[5][1].Coordinate,
                tiles[5][2].Coordinate,
                tiles[5][3].Coordinate,
                tiles[5][4].Coordinate,
                tiles[5][5].Coordinate
            };
            Assert.That(this.Unit.Ranges.Attack, Is.EquivalentTo(expected));
        }

        [Test]
        public void Square_2_3Range()
        {
            int minRange = 2;
            int maxRange = 3;
            this.Item.MinRange.FinalValue.Returns(minRange);
            this.Item.MaxRange.FinalValue.Returns(maxRange);
            this.Item.Item.Range.Shape.Returns(ItemRangeShape.Square);

            Assert.That(this.Unit.Ranges.Attack, Is.Empty);

            ItemRangeCalculator calc = new ItemRangeCalculator(this.Map, new IUnit[] { this.Unit });
            calc.CalculateUnitItemRanges();

            Assert.That(this.Unit.Ranges.Attack, Is.Not.Empty);

            ITile[][] tiles = this.Map.Segments[0].Tiles;
            IList<ICoordinate> expected = new List<ICoordinate>()
            {
                //2 range set
                tiles[1][1].Coordinate,
                tiles[1][2].Coordinate,
                tiles[1][3].Coordinate,
                tiles[1][4].Coordinate,
                tiles[1][5].Coordinate,
                tiles[2][1].Coordinate,
                tiles[2][5].Coordinate,
                tiles[3][1].Coordinate,
                tiles[3][5].Coordinate,
                tiles[4][1].Coordinate,
                tiles[4][5].Coordinate,
                tiles[5][1].Coordinate,
                tiles[5][2].Coordinate,
                tiles[5][3].Coordinate,
                tiles[5][4].Coordinate,
                tiles[5][5].Coordinate,
                //3 range set
                tiles[0][0].Coordinate,
                tiles[0][1].Coordinate,
                tiles[0][2].Coordinate,
                tiles[0][3].Coordinate,
                tiles[0][4].Coordinate,
                tiles[0][5].Coordinate,
                tiles[0][6].Coordinate,
                tiles[1][0].Coordinate,
                tiles[1][6].Coordinate,
                tiles[2][0].Coordinate,
                tiles[2][6].Coordinate,
                tiles[3][0].Coordinate,
                tiles[3][6].Coordinate,
                tiles[4][0].Coordinate,
                tiles[4][6].Coordinate,
                tiles[5][0].Coordinate,
                tiles[5][6].Coordinate,
                tiles[6][0].Coordinate,
                tiles[6][1].Coordinate,
                tiles[6][2].Coordinate,
                tiles[6][3].Coordinate,
                tiles[6][4].Coordinate,
                tiles[6][5].Coordinate,
                tiles[6][6].Coordinate
            };
            Assert.That(this.Unit.Ranges.Attack, Is.EquivalentTo(expected));
        }

        #endregion ItemRangeShape.Standard

        #region ItemRangeShape.Cross

        [Test]
        public void Cross_1Range()
        {
            int range = 1;
            this.Item.MinRange.FinalValue.Returns(range);
            this.Item.MaxRange.FinalValue.Returns(range);
            this.Item.Item.Range.Shape.Returns(ItemRangeShape.Cross);

            Assert.That(this.Unit.Ranges.Attack, Is.Empty);

            ItemRangeCalculator calc = new ItemRangeCalculator(this.Map, new IUnit[] { this.Unit });
            calc.CalculateUnitItemRanges();

            Assert.That(this.Unit.Ranges.Attack, Is.Not.Empty);

            ITile[][] tiles = this.Map.Segments[0].Tiles;
            IList<ICoordinate> expected = new List<ICoordinate>()
            {
                tiles[2][3].Coordinate,
                tiles[3][2].Coordinate,
                tiles[3][4].Coordinate,
                tiles[4][3].Coordinate
            };
            Assert.That(this.Unit.Ranges.Attack, Is.EquivalentTo(expected));
        }

        [Test]
        public void Cross_2Range()
        {
            int range = 2;
            this.Item.MinRange.FinalValue.Returns(range);
            this.Item.MaxRange.FinalValue.Returns(range);
            this.Item.Item.Range.Shape.Returns(ItemRangeShape.Cross);

            Assert.That(this.Unit.Ranges.Attack, Is.Empty);

            ItemRangeCalculator calc = new ItemRangeCalculator(this.Map, new IUnit[] { this.Unit });
            calc.CalculateUnitItemRanges();

            Assert.That(this.Unit.Ranges.Attack, Is.Not.Empty);

            ITile[][] tiles = this.Map.Segments[0].Tiles;
            IList<ICoordinate> expected = new List<ICoordinate>()
            {
                tiles[1][3].Coordinate,
                tiles[3][1].Coordinate,
                tiles[3][5].Coordinate,
                tiles[5][3].Coordinate
            };
            Assert.That(this.Unit.Ranges.Attack, Is.EquivalentTo(expected));
        }

        [Test]
        public void Cross_3Range()
        {
            int range = 3;
            this.Item.MinRange.FinalValue.Returns(range);
            this.Item.MaxRange.FinalValue.Returns(range);
            this.Item.Item.Range.Shape.Returns(ItemRangeShape.Cross);

            Assert.That(this.Unit.Ranges.Attack, Is.Empty);

            ItemRangeCalculator calc = new ItemRangeCalculator(this.Map, new IUnit[] { this.Unit });
            calc.CalculateUnitItemRanges();

            Assert.That(this.Unit.Ranges.Attack, Is.Not.Empty);

            ITile[][] tiles = this.Map.Segments[0].Tiles;
            IList<ICoordinate> expected = new List<ICoordinate>()
            {
                tiles[0][3].Coordinate,
                tiles[3][0].Coordinate,
                tiles[3][6].Coordinate,
                tiles[6][3].Coordinate
            };
            Assert.That(this.Unit.Ranges.Attack, Is.EquivalentTo(expected));
        }

        [Test]
        public void Cross_1_2Range()
        {
            int minRange = 1;
            int maxRange = 2;
            this.Item.MinRange.FinalValue.Returns(minRange);
            this.Item.MaxRange.FinalValue.Returns(maxRange);
            this.Item.Item.Range.Shape.Returns(ItemRangeShape.Cross);

            Assert.That(this.Unit.Ranges.Attack, Is.Empty);

            ItemRangeCalculator calc = new ItemRangeCalculator(this.Map, new IUnit[] { this.Unit });
            calc.CalculateUnitItemRanges();

            Assert.That(this.Unit.Ranges.Attack, Is.Not.Empty);

            ITile[][] tiles = this.Map.Segments[0].Tiles;
            IList<ICoordinate> expected = new List<ICoordinate>()
            {
                //1 range set
                tiles[2][3].Coordinate,
                tiles[3][2].Coordinate,
                tiles[3][4].Coordinate,
                tiles[4][3].Coordinate,
                //2 range set
                tiles[1][3].Coordinate,
                tiles[3][1].Coordinate,
                tiles[3][5].Coordinate,
                tiles[5][3].Coordinate
            };
            Assert.That(this.Unit.Ranges.Attack, Is.EquivalentTo(expected));
        }

        [Test]
        public void Cross_2_3Range()
        {
            int minRange = 2;
            int maxRange = 3;
            this.Item.MinRange.FinalValue.Returns(minRange);
            this.Item.MaxRange.FinalValue.Returns(maxRange);
            this.Item.Item.Range.Shape.Returns(ItemRangeShape.Cross);

            Assert.That(this.Unit.Ranges.Attack, Is.Empty);

            ItemRangeCalculator calc = new ItemRangeCalculator(this.Map, new IUnit[] { this.Unit });
            calc.CalculateUnitItemRanges();

            Assert.That(this.Unit.Ranges.Attack, Is.Not.Empty);

            ITile[][] tiles = this.Map.Segments[0].Tiles;
            IList<ICoordinate> expected = new List<ICoordinate>()
            {
                //2 range set
                tiles[1][3].Coordinate,
                tiles[3][1].Coordinate,
                tiles[3][5].Coordinate,
                tiles[5][3].Coordinate,
                //3 range set
                tiles[0][3].Coordinate,
                tiles[3][0].Coordinate,
                tiles[3][6].Coordinate,
                tiles[6][3].Coordinate
            };
            Assert.That(this.Unit.Ranges.Attack, Is.EquivalentTo(expected));
        }

        #endregion ItemRangeShape.Cross

        #region ItemRangeShape.Saltire

        [Test]
        public void Saltire_1Range()
        {
            int range = 1;
            this.Item.MinRange.FinalValue.Returns(range);
            this.Item.MaxRange.FinalValue.Returns(range);
            this.Item.Item.Range.Shape.Returns(ItemRangeShape.Saltire);

            Assert.That(this.Unit.Ranges.Attack, Is.Empty);

            ItemRangeCalculator calc = new ItemRangeCalculator(this.Map, new IUnit[] { this.Unit });
            calc.CalculateUnitItemRanges();

            Assert.That(this.Unit.Ranges.Attack, Is.Not.Empty);

            ITile[][] tiles = this.Map.Segments[0].Tiles;
            IList<ICoordinate> expected = new List<ICoordinate>()
            {
                tiles[2][2].Coordinate,
                tiles[2][4].Coordinate,
                tiles[4][2].Coordinate,
                tiles[4][4].Coordinate
            };
            Assert.That(this.Unit.Ranges.Attack, Is.EquivalentTo(expected));
        }

        [Test]
        public void Saltire_2Range()
        {
            int range = 2;
            this.Item.MinRange.FinalValue.Returns(range);
            this.Item.MaxRange.FinalValue.Returns(range);
            this.Item.Item.Range.Shape.Returns(ItemRangeShape.Saltire);

            Assert.That(this.Unit.Ranges.Attack, Is.Empty);

            ItemRangeCalculator calc = new ItemRangeCalculator(this.Map, new IUnit[] { this.Unit });
            calc.CalculateUnitItemRanges();

            Assert.That(this.Unit.Ranges.Attack, Is.Not.Empty);

            ITile[][] tiles = this.Map.Segments[0].Tiles;
            IList<ICoordinate> expected = new List<ICoordinate>()
            {
                tiles[1][1].Coordinate,
                tiles[1][5].Coordinate,
                tiles[5][1].Coordinate,
                tiles[5][5].Coordinate
            };
            Assert.That(this.Unit.Ranges.Attack, Is.EquivalentTo(expected));
        }

        [Test]
        public void Saltire_3Range()
        {
            int range = 3;
            this.Item.MinRange.FinalValue.Returns(range);
            this.Item.MaxRange.FinalValue.Returns(range);
            this.Item.Item.Range.Shape.Returns(ItemRangeShape.Saltire);

            Assert.That(this.Unit.Ranges.Attack, Is.Empty);

            ItemRangeCalculator calc = new ItemRangeCalculator(this.Map, new IUnit[] { this.Unit });
            calc.CalculateUnitItemRanges();

            Assert.That(this.Unit.Ranges.Attack, Is.Not.Empty);

            ITile[][] tiles = this.Map.Segments[0].Tiles;
            IList<ICoordinate> expected = new List<ICoordinate>()
            {
                tiles[0][0].Coordinate,
                tiles[0][6].Coordinate,
                tiles[6][0].Coordinate,
                tiles[6][6].Coordinate
            };
            Assert.That(this.Unit.Ranges.Attack, Is.EquivalentTo(expected));
        }

        [Test]
        public void Saltire_1_2Range()
        {
            int minRange = 1;
            int maxRange = 2;
            this.Item.MinRange.FinalValue.Returns(minRange);
            this.Item.MaxRange.FinalValue.Returns(maxRange);
            this.Item.Item.Range.Shape.Returns(ItemRangeShape.Saltire);

            Assert.That(this.Unit.Ranges.Attack, Is.Empty);

            ItemRangeCalculator calc = new ItemRangeCalculator(this.Map, new IUnit[] { this.Unit });
            calc.CalculateUnitItemRanges();

            Assert.That(this.Unit.Ranges.Attack, Is.Not.Empty);

            ITile[][] tiles = this.Map.Segments[0].Tiles;
            IList<ICoordinate> expected = new List<ICoordinate>()
            {
                //1 range set
                tiles[2][2].Coordinate,
                tiles[2][4].Coordinate,
                tiles[4][2].Coordinate,
                tiles[4][4].Coordinate,
                //2 range set
                tiles[1][1].Coordinate,
                tiles[1][5].Coordinate,
                tiles[5][1].Coordinate,
                tiles[5][5].Coordinate
            };
            Assert.That(this.Unit.Ranges.Attack, Is.EquivalentTo(expected));
        }

        [Test]
        public void Saltire_2_3Range()
        {
            int minRange = 2;
            int maxRange = 3;
            this.Item.MinRange.FinalValue.Returns(minRange);
            this.Item.MaxRange.FinalValue.Returns(maxRange);
            this.Item.Item.Range.Shape.Returns(ItemRangeShape.Saltire);

            Assert.That(this.Unit.Ranges.Attack, Is.Empty);

            ItemRangeCalculator calc = new ItemRangeCalculator(this.Map, new IUnit[] { this.Unit });
            calc.CalculateUnitItemRanges();

            Assert.That(this.Unit.Ranges.Attack, Is.Not.Empty);

            ITile[][] tiles = this.Map.Segments[0].Tiles;
            IList<ICoordinate> expected = new List<ICoordinate>()
            {
                //2 range set
                tiles[1][1].Coordinate,
                tiles[1][5].Coordinate,
                tiles[5][1].Coordinate,
                tiles[5][5].Coordinate,
                //3 range set
                tiles[0][0].Coordinate,
                tiles[0][6].Coordinate,
                tiles[6][0].Coordinate,
                tiles[6][6].Coordinate
            };
            Assert.That(this.Unit.Ranges.Attack, Is.EquivalentTo(expected));
        }

        #endregion ItemRangeShape.Saltire

        #region ItemRangeShape.Star

        [Test]
        public void Star_1Range()
        {
            int range = 1;
            this.Item.MinRange.FinalValue.Returns(range);
            this.Item.MaxRange.FinalValue.Returns(range);
            this.Item.Item.Range.Shape.Returns(ItemRangeShape.Star);

            Assert.That(this.Unit.Ranges.Attack, Is.Empty);

            ItemRangeCalculator calc = new ItemRangeCalculator(this.Map, new IUnit[] { this.Unit });
            calc.CalculateUnitItemRanges();

            Assert.That(this.Unit.Ranges.Attack, Is.Not.Empty);

            ITile[][] tiles = this.Map.Segments[0].Tiles;
            IList<ICoordinate> expected = new List<ICoordinate>()
            {
                //Same as 1 range square
                tiles[2][2].Coordinate,
                tiles[2][3].Coordinate,
                tiles[2][4].Coordinate,
                tiles[3][2].Coordinate,
                tiles[3][4].Coordinate,
                tiles[4][2].Coordinate,
                tiles[4][3].Coordinate,
                tiles[4][4].Coordinate
            };
            Assert.That(this.Unit.Ranges.Attack, Is.EquivalentTo(expected));
        }

        [Test]
        public void Star_2Range()
        {
            int range = 2;
            this.Item.MinRange.FinalValue.Returns(range);
            this.Item.MaxRange.FinalValue.Returns(range);
            this.Item.Item.Range.Shape.Returns(ItemRangeShape.Star);

            Assert.That(this.Unit.Ranges.Attack, Is.Empty);

            ItemRangeCalculator calc = new ItemRangeCalculator(this.Map, new IUnit[] { this.Unit });
            calc.CalculateUnitItemRanges();

            Assert.That(this.Unit.Ranges.Attack, Is.Not.Empty);

            ITile[][] tiles = this.Map.Segments[0].Tiles;
            IList<ICoordinate> expected = new List<ICoordinate>()
            {
                //Same as 2 range cross
                tiles[1][3].Coordinate,
                tiles[3][1].Coordinate,
                tiles[3][5].Coordinate,
                tiles[5][3].Coordinate,
                //Plus 2 range saltire
                tiles[1][1].Coordinate,
                tiles[1][5].Coordinate,
                tiles[5][1].Coordinate,
                tiles[5][5].Coordinate

            };
            Assert.That(this.Unit.Ranges.Attack, Is.EquivalentTo(expected));
        }

        [Test]
        public void Star_3Range()
        {
            int range = 3;
            this.Item.MinRange.FinalValue.Returns(range);
            this.Item.MaxRange.FinalValue.Returns(range);
            this.Item.Item.Range.Shape.Returns(ItemRangeShape.Star);

            Assert.That(this.Unit.Ranges.Attack, Is.Empty);

            ItemRangeCalculator calc = new ItemRangeCalculator(this.Map, new IUnit[] { this.Unit });
            calc.CalculateUnitItemRanges();

            Assert.That(this.Unit.Ranges.Attack, Is.Not.Empty);

            ITile[][] tiles = this.Map.Segments[0].Tiles;
            IList<ICoordinate> expected = new List<ICoordinate>()
            {
                //Same as 3 range cross
                tiles[0][3].Coordinate,
                tiles[3][0].Coordinate,
                tiles[3][6].Coordinate,
                tiles[6][3].Coordinate,
                //Plus 3 range saltire
                tiles[0][0].Coordinate,
                tiles[0][6].Coordinate,
                tiles[6][0].Coordinate,
                tiles[6][6].Coordinate
            };
            Assert.That(this.Unit.Ranges.Attack, Is.EquivalentTo(expected));
        }

        [Test]
        public void Star_1_2Range()
        {
            int minRange = 1;
            int maxRange = 2;
            this.Item.MinRange.FinalValue.Returns(minRange);
            this.Item.MaxRange.FinalValue.Returns(maxRange);
            this.Item.Item.Range.Shape.Returns(ItemRangeShape.Star);

            Assert.That(this.Unit.Ranges.Attack, Is.Empty);

            ItemRangeCalculator calc = new ItemRangeCalculator(this.Map, new IUnit[] { this.Unit });
            calc.CalculateUnitItemRanges();

            Assert.That(this.Unit.Ranges.Attack, Is.Not.Empty);

            ITile[][] tiles = this.Map.Segments[0].Tiles;
            IList<ICoordinate> expected = new List<ICoordinate>()
            {
                //1 range set
                tiles[2][2].Coordinate,
                tiles[2][3].Coordinate,
                tiles[2][4].Coordinate,
                tiles[3][2].Coordinate,
                tiles[3][4].Coordinate,
                tiles[4][2].Coordinate,
                tiles[4][3].Coordinate,
                tiles[4][4].Coordinate,
                //2 range set
                tiles[1][3].Coordinate,
                tiles[3][1].Coordinate,
                tiles[3][5].Coordinate,
                tiles[5][3].Coordinate,
                tiles[1][1].Coordinate,
                tiles[1][5].Coordinate,
                tiles[5][1].Coordinate,
                tiles[5][5].Coordinate
            };
            Assert.That(this.Unit.Ranges.Attack, Is.EquivalentTo(expected));
        }

        [Test]
        public void Star_2_3Range()
        {
            int minRange = 2;
            int maxRange = 3;
            this.Item.MinRange.FinalValue.Returns(minRange);
            this.Item.MaxRange.FinalValue.Returns(maxRange);
            this.Item.Item.Range.Shape.Returns(ItemRangeShape.Star);

            Assert.That(this.Unit.Ranges.Attack, Is.Empty);

            ItemRangeCalculator calc = new ItemRangeCalculator(this.Map, new IUnit[] { this.Unit });
            calc.CalculateUnitItemRanges();

            Assert.That(this.Unit.Ranges.Attack, Is.Not.Empty);

            ITile[][] tiles = this.Map.Segments[0].Tiles;
            IList<ICoordinate> expected = new List<ICoordinate>()
            {
                //2 range set
                tiles[1][3].Coordinate,
                tiles[3][1].Coordinate,
                tiles[3][5].Coordinate,
                tiles[5][3].Coordinate,
                tiles[1][1].Coordinate,
                tiles[1][5].Coordinate,
                tiles[5][1].Coordinate,
                tiles[5][5].Coordinate,
                //3 range set
                tiles[0][3].Coordinate,
                tiles[3][0].Coordinate,
                tiles[3][6].Coordinate,
                tiles[6][3].Coordinate,
                tiles[0][0].Coordinate,
                tiles[0][6].Coordinate,
                tiles[6][0].Coordinate,
                tiles[6][6].Coordinate
            };
            Assert.That(this.Unit.Ranges.Attack, Is.EquivalentTo(expected));
        }

        #endregion ItemRangeShape.Star
    }
}
