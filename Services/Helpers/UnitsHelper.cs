using RedditEmblemAPI.Models.Configuration.Units;
using RedditEmblemAPI.Models.Exceptions.Processing;
using RedditEmblemAPI.Models.Exceptions.Unmatched;
using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output;
using RedditEmblemAPI.Models.Output.Map;
using RedditEmblemAPI.Models.Output.Map.Tiles;
using RedditEmblemAPI.Models.Output.System;
using RedditEmblemAPI.Models.Output.System.Skills;
using RedditEmblemAPI.Models.Output.System.Skills.Effects.UnitStats;
using RedditEmblemAPI.Models.Output.Units;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RedditEmblemAPI.Services.Helpers
{
    public class UnitsHelper
    {
        /// <summary>
        /// Parses Google Sheets data matrix to return a list of Unit output objects.
        /// </summary>
        /// <param name="data">Matrix of sheet Value values representing unit data</param>
        /// <param name="config">Parsed JSON configuration mapping Values to output</param>
        /// <returns></returns>
        public static IList<Unit> Process(UnitsConfig config, SystemInfo systemData, MapObj map)
        {
            IList<Unit> units = new List<Unit>();

            //Create units
            foreach (IList<object> row in config.Query.Data)
            {
                try
                {
                    //Convert objects to strings
                    IList<string> unit = row.Select(r => r.ToString()).ToList();
                    string unitName = DataParser.OptionalString(unit, config.Name, "Name");
                    if (string.IsNullOrEmpty(unitName)) continue;

                    if (units.Any(u => u.Name == unitName))
                        throw new NonUniqueObjectNameException("unit");

                    units.Add(new Unit(config, unit, systemData));
                }
                catch (Exception ex)
                {
                    throw new UnitProcessingException((row.ElementAtOrDefault(config.Name) ?? string.Empty).ToString(), ex);
                }
            }

            //Add units to the map
            foreach(Unit unit in units)
            {
                try
                {
                    unit.Location.Coordinate = new Coordinate(unit.Location.CoordinateString);
                    AddUnitToMap(unit, map, true);
                }
                catch (CoordinateFormattingException ex)
                {
                    //If the coordinates aren't in an <x,y> format, check if it's the name of another unit.
                    Unit pair = units.FirstOrDefault(u => u.Name == unit.Location.CoordinateString);

                    if (pair == null)
                        throw new UnitProcessingException(unit.Name, ex);

                    //Unit is paired with itself
                    if (pair.Name == unit.Name)
                        throw new UnitProcessingException(unit.Name, new UnitPairedWithSelfException(unit.Name));

                    //Unit is already paired with someone
                    if (unit.Location.PairedUnitObj != null)
                        throw new UnitProcessingException(unit.Name, new UnitAlreadyPairedException(unit.Name, unit.Location.PairedUnitObj.Name, pair.Name));

                    //Paired unit is already paired with someone
                    if (pair.Location.PairedUnitObj != null)
                        throw new UnitProcessingException(unit.Name, new UnitAlreadyPairedException(pair.Name, pair.Location.PairedUnitObj.Name, unit.Name));

                    //Bind paired units together
                    pair.Location.PairedUnitObj = unit;
                    unit.Location.PairedUnitObj = pair;
                    unit.Location.IsBackOfPair = true;
                    unit.Location.Coordinate = new Coordinate();
                }
            }


            foreach (Unit unit in units.Where(u => u.Location.IsBackOfPair))
            {
                //Replace back unit coordinate with front unit coord
                unit.Location.Coordinate = unit.Location.PairedUnitObj.Location.Coordinate;

                if (unit.Location.Coordinate.X < 1 || unit.Location.Coordinate.Y < 1)
                    continue;

                //If we're calculating paired unit ranges, add origin tiles
                if (map.Constants.CalculatePairedUnitRanges)
                    AddUnitToMap(unit, map, false);
            }

            //Apply skill and status condition effects
            foreach(Unit unit in units)
            {
                //Skill effects
                try
                {
                    foreach (Skill skill in unit.SkillList)
                        if (skill.Effect != null)
                            skill.Effect.Apply(unit, skill, map, units);
                }
                catch(Exception ex)
                {
                    throw new UnitSkillEffectProcessingException(unit.Name, ex);
                }

                //Status condition effects
                try
                {
                    foreach (UnitStatus status in unit.StatusConditions)
                        if (status.StatusObj.Effect != null)
                            status.StatusObj.Effect.Apply(unit, status.StatusObj);
                }
                catch(Exception ex)
                {
                    throw new UnitStatusConditionEffectProcessingException(unit.Name, ex);
                }
            }

            //Calculate combat stats
            //Always do this last
            foreach (Unit unit in units)
            {
                try
                {
                    unit.Stats.CalculateCombatStats(config.CombatStats, unit.Inventory, 
                        unit.SkillList.Select(s => s.Effect).OfType<ReplaceCombatStatFormulaVariableEffect>().ToList());
                }
                catch (Exception ex)
                {
                    throw new UnitCombatStatFormulaProcessingException(unit.Name, ex);
                }
            }

            return units;
        }

        /// <summary>
        /// Calculates the anchor and origin tiles for <paramref name="unit"/>.
        /// </summary>
        /// <param name="applyTileBinding">Should be true for any unit that's not in pair-up.</param>
        private static void AddUnitToMap(Unit unit, MapObj map, bool applyTileBinding)
        {
            //Ignore hidden units
            if (unit.Location.Coordinate.X < 1 || unit.Location.Coordinate.Y < 1)
                return;

            for (int y = 0; y < unit.Location.UnitSize; y++)
            {
                for (int x = 0; x < unit.Location.UnitSize; x++)
                {
                    Tile tile = map.GetTileByCoord(unit.Location.Coordinate.X + x, unit.Location.Coordinate.Y + y);

                    //Make sure this unit is not placed overlapping another
                    if (tile.UnitData.Unit != null && unit.Name != tile.UnitData.Unit.Name && applyTileBinding)
                        throw new UnitTileOverlapException(unit, tile.UnitData.Unit, tile.Coordinate);

                    unit.Location.OriginTiles.Add(tile);
                    unit.Ranges.Movement.Add(tile.Coordinate);

                    if (applyTileBinding)
                    {
                        tile.UnitData.Unit = unit;
                        tile.UnitData.IsUnitOrigin = true;

                        if (x == 0 && y == 0)
                        {
                            //Mark the anchor tile
                            unit.Location.AnchorTile = tile;
                            tile.UnitData.IsUnitAnchor = true;
                        }
                    }
                    
                    //Apply terrain type effects from the origin tile.
                    ApplyTileTerrainTypeToUnit(unit, tile);
                    ApplyTileTerrainEffectsToUnit(unit, tile);
                }
            }
        }

        private static void ApplyTileTerrainTypeToUnit(Unit unit, Tile tile)
        {
            //Apply combat stat modifiers
            foreach (KeyValuePair<string, int> modifier in tile.TerrainTypeObj.CombatStatModifiers)
            {
                ModifiedStatValue stat;
                if (!unit.Stats.Combat.TryGetValue(modifier.Key, out stat))
                    throw new UnmatchedStatException(modifier.Key);
                stat.Modifiers.TryAdd(tile.TerrainTypeObj.Name, modifier.Value);
            }

            //Apply stat modifiers
            foreach (KeyValuePair<string, int> modifier in tile.TerrainTypeObj.StatModifiers)
            {
                ModifiedStatValue stat;
                if (!unit.Stats.General.TryGetValue(modifier.Key, out stat))
                    throw new UnmatchedStatException(modifier.Key);
                stat.Modifiers.TryAdd(tile.TerrainTypeObj.Name, modifier.Value);
            }
        }

        private static void ApplyTileTerrainEffectsToUnit(Unit unit, Tile tile)
        {
            foreach(TileTerrainEffect effect in tile.TerrainEffects)
            {
                //Apply combat stat modifiers
                foreach(KeyValuePair<string, int> modifier in effect.TerrainEffect.CombatStatModifiers)
                {
                    ModifiedStatValue stat;
                    if (!unit.Stats.Combat.TryGetValue(modifier.Key, out stat))
                        throw new UnmatchedStatException(modifier.Key);
                    stat.Modifiers.TryAdd(effect.TerrainEffect.Name, modifier.Value);
                }

                //Apply stat modifiers
                foreach (KeyValuePair<string, int> modifier in effect.TerrainEffect.StatModifiers)
                {
                    ModifiedStatValue stat;
                    if (!unit.Stats.General.TryGetValue(modifier.Key, out stat))
                        throw new UnmatchedStatException(modifier.Key);
                    stat.Modifiers.TryAdd(effect.TerrainEffect.Name, modifier.Value);
                }
            }
        }
    }
}
