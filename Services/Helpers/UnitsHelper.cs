using RedditEmblemAPI.Models.Configuration.Units;
using RedditEmblemAPI.Models.Exceptions.Processing;
using RedditEmblemAPI.Models.Exceptions.Unmatched;
using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output;
using RedditEmblemAPI.Models.Output.Map;
using RedditEmblemAPI.Models.Output.System;
using RedditEmblemAPI.Models.Output.System.Skills;
using RedditEmblemAPI.Models.Output.System.StatusConditions;
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
                    string unitName = ParseHelper.SafeStringParse(unit, config.Name, "Name", false);
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
                    unit.Coordinate = new Coordinate(unit.CoordinateString);
                    AddUnitToMap(unit, map);
                }
                catch(CoordinateFormattingException ex)
                {
                    //If the coordinates aren't in an <x,y> format, check if it's the name of another unit.
                    Unit pair = units.FirstOrDefault(u => u.Name == unit.CoordinateString);

                    if (pair == null || pair.Name == unit.Name)
                        throw new UnitProcessingException(unit.Name, ex);

                    //Unit is already paired with someone
                    if (unit.PairedUnitObj != null)
                        throw new UnitProcessingException(unit.Name, new UnitAlreadyPairedException(unit.Name, unit.PairedUnitObj.Name, pair.Name));

                    //Paired unit is already paired with someone
                    if (pair.PairedUnitObj != null)
                        throw new UnitProcessingException(unit.Name, new UnitAlreadyPairedException(pair.Name, pair.PairedUnitObj.Name, unit.Name));

                    //Bind paired units together
                    pair.PairedUnitObj = unit;
                    unit.PairedUnitObj = pair;
                    unit.IsBackOfPair = true;
                    unit.Coordinate = new Coordinate(-1, -1);
                }
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
                    unit.CalculateCombatStats(config.CombatStats);
                }
                catch (Exception ex)
                {
                    throw new UnitProcessingException(unit.Name, ex);
                }
            }

            return units;
        }

        private static void AddUnitToMap(Unit unit, MapObj map)
        {
            //Ignore hidden units
            if (unit.Coordinate.X < 1 || unit.Coordinate.Y < 1)
                return;

            //Find tile corresponsing to units coordinates
            Tile tile = map.GetTileByCoord(unit.Coordinate);

            //Make sure this unit is not placed overlapping another
            if (tile.Unit != null)
                throw new UnitTileOverlapException(unit, tile.Unit, tile.Coordinate);

            //Two way bind the unit and tile objects
            unit.AnchorTile = tile;
            tile.Unit = unit;
            tile.IsUnitAnchor = true;

            unit.MovementRange.Add(new Coordinate(tile.Coordinate));

            //Apply terrain effect to the unit
            IList<string> effectsApplied = new List<string>();
            ApplyTileTerrainEffectsToUnit(unit, tile, effectsApplied);

            if (unit.UnitSize > 1)
            {
                //Calculate origin tile for multi-tile units
                int anchorOffset = (int)Math.Ceiling(unit.UnitSize / 2.0m) - 1;

                for(int y = 0; y < unit.UnitSize; y++)
                {
                    for (int x = 0; x < unit.UnitSize; x++)
                    {
                        Tile intersectTile = map.GetTileByCoord(unit.Coordinate.X + x, unit.Coordinate.Y + y);

                        //Make sure this unit is not placed overlapping another
                        if (intersectTile.Unit != null && unit.Name != intersectTile.Unit.Name)
                            throw new UnitTileOverlapException(unit, intersectTile.Unit, intersectTile.Coordinate);

                        intersectTile.Unit = unit;
                        if(!unit.MovementRange.Contains(intersectTile.Coordinate))
                            unit.MovementRange.Add(intersectTile.Coordinate);
                        
                        if (x == anchorOffset && y == anchorOffset)
                        {
                            unit.OriginTile = intersectTile;
                            intersectTile.IsUnitOrigin = true;

                            //Apply terrain type effects from the origin tile.
                            ApplyTileTerrainTypeToUnit(unit, intersectTile);
                        }

                        ApplyTileTerrainEffectsToUnit(unit, intersectTile, effectsApplied);
                    }
                }
            }
            else
            {
                //Single tile units have their anchor and origin in the same place.
                unit.OriginTile = tile;
                tile.IsUnitOrigin = true;

                //Apply terrain type effects from the origin tile.
                ApplyTileTerrainTypeToUnit(unit, tile);
            }   
        }

        private static void ApplyTileTerrainTypeToUnit(Unit unit, Tile tile)
        {
            //Apply combat stat modifiers
            foreach (KeyValuePair<string, int> modifier in tile.TerrainTypeObj.CombatStatModifiers)
            {
                ModifiedStatValue stat;
                if (!unit.CombatStats.TryGetValue(modifier.Key, out stat))
                    throw new UnmatchedStatException(modifier.Key);
                stat.Modifiers.Add(tile.TerrainTypeObj.Name, modifier.Value);
            }

            //Apply stat modifiers
            foreach (KeyValuePair<string, int> modifier in tile.TerrainTypeObj.StatModifiers)
            {
                ModifiedStatValue stat;
                if (!unit.Stats.TryGetValue(modifier.Key, out stat))
                    throw new UnmatchedStatException(modifier.Key);
                stat.Modifiers.Add(tile.TerrainTypeObj.Name, modifier.Value);
            }
        }

        private static void ApplyTileTerrainEffectsToUnit(Unit unit, Tile tile, IList<string> effectsApplied)
        {
            foreach(TileTerrainEffect effect in tile.TerrainEffects)
            {
                //Terrain effects cannot be applied twice
                if (effectsApplied.Contains(effect.TerrainEffect.Name))
                    continue;

                effectsApplied.Add(effect.TerrainEffect.Name);

                //Apply combat stat modifiers
                foreach(KeyValuePair<string, int> modifier in effect.TerrainEffect.CombatStatModifiers)
                {
                    ModifiedStatValue stat;
                    if (!unit.CombatStats.TryGetValue(modifier.Key, out stat))
                        throw new UnmatchedStatException(modifier.Key);
                    stat.Modifiers.Add(effect.TerrainEffect.Name, modifier.Value);
                }

                //Apply stat modifiers
                foreach (KeyValuePair<string, int> modifier in effect.TerrainEffect.StatModifiers)
                {
                    ModifiedStatValue stat;
                    if (!unit.Stats.TryGetValue(modifier.Key, out stat))
                        throw new UnmatchedStatException(modifier.Key);
                    stat.Modifiers.Add(effect.TerrainEffect.Name, modifier.Value);
                }
            }
        }
    }
}
