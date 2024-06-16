using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Configuration.System
{
    /// <summary>
    /// Container class for deserialized JSON <c>"InterfaceLabels"</c> object data.
    /// </summary>
    public class InterfaceLabelsConfig
    {
        public string Adjutants { get; set; } = "Adjutants";
        public string Battalion { get; set; } = "Battalion";
        public string BattleStyle { get; set; } = "Battle Style";
        public string Class { get; set; } = "Class";
        public string CombatArts { get; set; } = "Combat Arts";
        public string Emblem { get; set; } = "Emblem";
        public string Gambit { get; set; } = "Gambit";
        public string Inventory { get; set; } = "Inventory";
        public IEnumerable<string> InventorySubsections { get; set; } = new List<string>();
        public string Skills { get; set; } = "Skills";
        public IEnumerable<string> SkillSubsections { get; set; } = new List<string>();
        public string StatusConditions { get; set; } = "Status Conditions";
        public string WeaponRanks { get; set; } = "Weapon Ranks";
    }
}
