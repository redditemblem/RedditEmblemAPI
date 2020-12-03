using RedditEmblemAPI.Models.Configuration.Units;
using RedditEmblemAPI.Models.Exceptions.Processing;
using RedditEmblemAPI.Models.Exceptions.Unmatched;
using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output;
using RedditEmblemAPI.Models.Output.Map;
using RedditEmblemAPI.Models.Output.System;
using RedditEmblemAPI.Models.Output.System.Skills;
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
        public static IList<Unit> Process(UnitsConfig config, SystemInfo systemData, List<List<Tile>> map)
        {
            IList<Unit> units = new List<Unit>();

            //Create the units and add them to the map
            foreach (IList<object> row in config.Query.Data)
            {
                try
                {
                    //Convert objects to strings
                    IList<string> unit = row.Select(r => r.ToString()).ToList();
                    string unitName = ParseHelper.SafeStringParse(unit, config.Name, "Name", false);
                    if (string.IsNullOrEmpty(unitName)) continue;

                    Unit temp = new Unit(config, unit, systemData);
                    AddUnitToMap(temp, map);

                    units.Add(temp);
                }
                catch (Exception ex)
                {
                    throw new UnitProcessingException((row.ElementAtOrDefault(config.Name) ?? string.Empty).ToString(), ex);
                }
            }

            //Apply skill effects
            foreach(Unit unit in units)
            {
                try
                {
                    foreach (Skill skill in unit.SkillList)
                        if (skill.Effect != null)
                            skill.Effect.Apply(unit, skill, units);
                }
                catch(Exception ex)
                {
                    throw new UnitSkillEffectProcessingException(unit.Name, ex);
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

        private static void AddUnitToMap(Unit unit, List<List<Tile>> map)
        {
            //Ignore hidden units
            if (unit.Coordinate.X < 1 || unit.Coordinate.Y < 1)
                return;

            //Find tile corresponsing to units coordinates
            IList<Tile> row = map.ElementAtOrDefault<IList<Tile>>(unit.Coordinate.Y - 1) ?? throw new TileOutOfBoundsException(unit.Coordinate);
            Tile tile = row.ElementAtOrDefault<Tile>(unit.Coordinate.X - 1) ?? throw new TileOutOfBoundsException(unit.Coordinate);

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
                        IList<Tile> intersectRow = map.ElementAtOrDefault<IList<Tile>>(unit.Coordinate.Y + y - 1) ?? throw new TileOutOfBoundsException(unit.Coordinate.X + x, unit.Coordinate.Y + y);
                        Tile intersectTile = intersectRow.ElementAtOrDefault<Tile>(unit.Coordinate.X + x - 1) ?? throw new TileOutOfBoundsException(unit.Coordinate.X + x, unit.Coordinate.Y + y);

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
