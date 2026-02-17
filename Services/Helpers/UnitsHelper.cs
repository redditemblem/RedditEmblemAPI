using RedditEmblemAPI.Models.Configuration.Units;
using RedditEmblemAPI.Models.Exceptions.Processing;
using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.Map;
using RedditEmblemAPI.Models.Output.Map.Tiles;
using RedditEmblemAPI.Models.Output.System;
using RedditEmblemAPI.Models.Output.System.Skills;
using RedditEmblemAPI.Models.Output.System.Skills.Effects;
using RedditEmblemAPI.Models.Output.System.StatusConditions;
using RedditEmblemAPI.Models.Output.Units;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RedditEmblemAPI.Services.Helpers
{
    public static class UnitsHelper
    {
        /// <summary>
        /// Parses Google Sheets data matrix to return a list of Unit output objects.
        /// </summary>
        /// <param name="config">Parsed JSON configuration mapping Values to output</param>
        public static List<IUnit> Process(UnitsConfig config, SystemInfo system, IMapObj map)
        {
            List<IUnit> units = Unit.BuildList(config, system);

            //Add units to the map
            foreach (IUnit unit in units)
            {
                try
                {
                    unit.Location.Coordinate = new Coordinate(map.Constants.CoordinateFormat, unit.Location.CoordinateString);
                    AddUnitToMap(unit, map, true);
                }
                catch (Exception ex)
                when (ex is XYCoordinateFormattingException || ex is AlphanumericCoordinateFormattingException)
                {
                    //If the coordinates aren't in a known format, check if it's the name of another unit.
                    IUnit pair = units.FirstOrDefault(u => u.Name == unit.Location.CoordinateString);

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


            foreach (IUnit unit in units.Where(u => u.Location.IsBackOfPair))
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
            foreach (IUnit unit in units)
            {
                //Skill effects
                foreach (ISkill skill in unit.GetFullSkillsList().Where(s => s.Effects.Any(e => e.ExecutionOrder == SkillEffectExecutionOrder.Standard)))
                {
                    foreach(ISkillEffect effect in skill.Effects.Where(e => e.ExecutionOrder == SkillEffectExecutionOrder.Standard))
                    {
                        try { effect.Apply(unit, skill, map, units); }
                        catch(Exception ex) { throw new UnitSkillEffectProcessingException(unit.Name, skill.Name, ex); }
                    }
                }
   
                //Status condition effects
                foreach (UnitStatus status in unit.StatusConditions)
                {
                    try
                    {
                        foreach (IStatusConditionEffect effect in status.Status.Effects)
                            effect.Apply(unit, status, system.Tags);
                    }
                    catch (Exception ex)
                    {
                        throw new UnitStatusConditionEffectProcessingException(unit.Name, status.Status.Name, ex);
                    }
                }
            }

            //Calculate combat stats and item ranges
            //Always do this last
            foreach (IUnit unit in units)
            {
                //Combat stats
                try
                {
                    unit.Stats.CalculateCombatStats(config.CombatStats, unit);
                }
                catch (Exception ex)
                {
                    throw new UnitCombatStatFormulaProcessingException(unit.Name, ex);
                }

                //Item ranges
                IList<IUnitInventoryItem> inventoryItems = unit.Inventory.GetAllItems();
                if (unit.Emblem != null && unit.Emblem.IsEngaged)
                    inventoryItems = inventoryItems.Union(unit.Emblem.EngageWeapons).ToList();

                foreach (IUnitInventoryItem item in inventoryItems)
                {
                    try
                    {
                        if (item.HasRangeThatRequiresCalculation)
                            item.CalculateItemRanges(unit);
                    }
                    catch (Exception ex)
                    {
                        throw new UnitInventoryItemRangeFormulaProcessingException(unit.Name, item.FullName, ex);
                    }

                    int maxRange = (int)decimal.Floor(item.MaxRange.FinalValue);
                    if (item.Item.Range.Shape == ItemRangeShape.Square || item.Item.Range.Shape == ItemRangeShape.Saltire || item.Item.Range.Shape == ItemRangeShape.Star)
                        maxRange *= 2;
                    if (maxRange > map.Constants.ItemMaxRangeAllowedForCalculation && maxRange < 99)
                        item.MaxRangeExceedsCalculationLimit = true;
                }

                //Skill effects
                foreach (Skill skill in unit.GetFullSkillsList().Where(s => s.Effects.Any(e => e.ExecutionOrder == SkillEffectExecutionOrder.AfterFinalStatCalculations)))
                {
                    foreach (SkillEffect effect in skill.Effects.Where(e => e.ExecutionOrder == SkillEffectExecutionOrder.AfterFinalStatCalculations))
                    {
                        try { effect.Apply(unit, skill, map, units); }
                        catch (Exception ex) { throw new UnitSkillEffectProcessingException(unit.Name, skill.Name, ex); }
                    }
                }
            }

            return units;
        }

        /// <summary>
        /// Calculates the anchor and origin tiles for <paramref name="unit"/>.
        /// </summary>
        /// <param name="applyTileBinding">Should be true for any unit that's not in pair-up.</param>
        private static void AddUnitToMap(IUnit unit, IMapObj map, bool applyTileBinding)
        {
            //Ignore hidden units
            if (unit.Location.Coordinate.X < 1 || unit.Location.Coordinate.Y < 1)
                return;

            for (int y = 0; y < unit.Location.UnitSize; y++)
            {
                for (int x = 0; x < unit.Location.UnitSize; x++)
                {
                    ITile tile = map.GetTileByCoord(unit.Location.Coordinate.X + x, unit.Location.Coordinate.Y + y);

                    //Make sure this unit is not placed overlapping another
                    if (tile.UnitData.Unit != null && unit.Name != tile.UnitData.Unit.Name && applyTileBinding)
                        throw new UnitTileOverlapException(unit, tile.UnitData.Unit, tile.Coordinate);

                    unit.Location.OriginTiles.Add(tile);
                    if(map.Constants.CalculateRanges)
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

                    ApplyTileModifiersToUnit(unit, tile);
                }
            }
        }

        /// <summary>
        /// Applies modifiers from the <paramref name="tile"/>'s terrain type and tile effects to <paramref name="unit"/>.
        /// </summary>
        private static void ApplyTileModifiersToUnit(IUnit unit, ITile tile)
        {
            ITerrainTypeStats stats = tile.TerrainType.GetTerrainTypeStatsByAffiliation(unit.Affiliation);
            unit.Stats.ApplyCombatStatModifiers(stats.CombatStatModifiers, tile.TerrainType.Name);
            unit.Stats.ApplyGeneralStatModifiers(stats.StatModifiers, tile.TerrainType.Name);
            
            foreach (TileObjectInstance effect in tile.TileObjects)
            {
                unit.Stats.ApplyCombatStatModifiers(effect.TileObject.CombatStatModifiers, effect.TileObject.Name);
                unit.Stats.ApplyGeneralStatModifiers(effect.TileObject.StatModifiers, effect.TileObject.Name);
            }
        }

    }
}
