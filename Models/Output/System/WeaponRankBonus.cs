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
        public WeaponRankBonus(WeaponRankBonusesConfig config, IEnumerable<string> data)
        {
            this.Category = DataParser.String(data, config.Category, "Category");
            this.Rank = DataParser.OptionalString(data, config.Rank, "Rank");

            this.CombatStatModifiers = DataParser.NamedStatDictionary_Int_Any(config.CombatStatModifiers, data, false, "{0} Modifier");
            this.StatModifiers = DataParser.NamedStatDictionary_Int_Any(config.StatModifiers, data, false, "{0} Modifier");
        }

        #region Static Functions

        /// <summary>
        /// Iterates through the data in <paramref name="config"/>'s <c>Query</c> and builds a <c>WeaponRankBonus</c> from each valid row.
        /// </summary>
        /// <exception cref="WeaponRankBonusProcessingException"></exception>
        public static List<WeaponRankBonus> BuildList(WeaponRankBonusesConfig config)
        {
            List<WeaponRankBonus> weaponRankBonuses = new List<WeaponRankBonus>();
            if (config == null || config.Query == null)
                return weaponRankBonuses;

            foreach (List<object> row in config.Query.Data)
            {
                string category = string.Empty;
                string rank = string.Empty;
                try
                {
                    IEnumerable<string> bonus = row.Select(r => r.ToString());
                    category = DataParser.OptionalString(bonus, config.Category, "Category");
                    rank = DataParser.OptionalString(bonus, config.Rank, "Rank");

                    if (string.IsNullOrEmpty(category)) continue;

                    if (weaponRankBonuses.Any(w => w.Category == category && w.Rank == rank))
                        throw new NonUniqueObjectNameException("weapon rank bonus");

                    weaponRankBonuses.Add(new WeaponRankBonus(config, bonus));
                }
                catch (Exception ex)
                {
                    throw new WeaponRankBonusProcessingException(category, rank, ex);
                }
            }

            return weaponRankBonuses;
        }

        #endregion
    }
}
