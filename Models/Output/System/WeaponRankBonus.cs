using RedditEmblemAPI.Models.Configuration.Common;
using RedditEmblemAPI.Models.Configuration.System.WeaponRankBonuses;
using RedditEmblemAPI.Models.Exceptions.Processing;
using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Services.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RedditEmblemAPI.Models.Output.System
{
    public class WeaponRankBonus
    {
        #region Attributes

        /// <summary>
        /// The item category to match.
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// The item rank to match.
        /// </summary>
        public string Rank { get; set; }

        /// <summary>
        /// List of modifiers that the weapon rank bonus can apply to a unit's combat stats.
        /// </summary>
        public IDictionary<string, int> CombatStatModifiers { get; set; }

        /// <summary>
        /// List of modifiers that the weapon rank bonus can apply to a unit's stats.
        /// </summary>
        public IDictionary<string, int> StatModifiers { get; set; }

        #endregion

        /// <summary>
        /// Constructor.
        /// </summary>
        public WeaponRankBonus(WeaponRankBonusesConfig config, IList<string> data)
        {
            this.Category = ParseHelper.SafeStringParse(data, config.Category, "Category", true);
            this.Rank = ParseHelper.SafeStringParse(data, config.Rank, "Rank", false);

            this.CombatStatModifiers = new Dictionary<string, int>();
            foreach (NamedStatConfig stat in config.CombatStatModifiers)
            {
                int val = ParseHelper.Int_Any(data, stat.Value, stat.SourceName + " Modifier");
                if (val == 0) continue;

                this.CombatStatModifiers.Add(stat.SourceName, val);
            }


            this.StatModifiers = new Dictionary<string, int>();
            foreach (NamedStatConfig stat in config.StatModifiers)
            {
                int val = ParseHelper.Int_Any(data, stat.Value, stat.SourceName + " Modifier");
                if (val == 0) continue;

                this.StatModifiers.Add(stat.SourceName, val);
            }
        }

        #region Static Functions

        public static IList<WeaponRankBonus> BuildList(WeaponRankBonusesConfig config)
        {
            IList<WeaponRankBonus> weaponRankBonuses = new List<WeaponRankBonus>();

            foreach (IList<object> row in config.Query.Data)
            {
                try
                {
                    IList<string> bonus = row.Select(r => r.ToString()).ToList();
                    string category = ParseHelper.SafeStringParse(bonus, config.Category, "Category", false);
                    string rank = ParseHelper.SafeStringParse(bonus, config.Rank, "Rank", false);

                    if (string.IsNullOrEmpty(category)) continue;

                    if (weaponRankBonuses.Any(w => w.Category == category && w.Rank == rank))
                        throw new NonUniqueObjectNameException("weapon rank bonus");

                    weaponRankBonuses.Add(new WeaponRankBonus(config, bonus));
                }
                catch (Exception ex)
                {
                    throw new WeaponRankBonusProcessingException((row.ElementAtOrDefault(config.Category) ?? string.Empty).ToString(),
                                                                 (row.ElementAtOrDefault(config.Rank) ?? string.Empty).ToString(), 
                                                                 ex);
                }
            }

            return weaponRankBonuses;
        }

        #endregion
    }
}
