using OverclockedClash.Data;
using OverclockedClash.Modules;

namespace OverclockedClash.Core
{
    /// <summary>
    /// Calcule la valeur finale d'une stat pour une pièce en appliquant tous les modules actifs.
    /// </summary>
    public static class StatCalculator
    {
        /// <summary>
        /// Retourne la stat de base depuis la définition (sans modules).
        /// </summary>
        public static float GetBaseStat(PieceInstance piece, string statName)
        {
            if (piece?.Definition == null) return 0f;
            return GetBaseStatFromDefinition(piece.Definition, statName);
        }

        /// <summary>
        /// Calcule la stat finale en chaînant tous les modules actifs sur cette pièce pour cette stat.
        /// Ne s'applique que si le signal d'activation du module est ON.
        /// </summary>
        public static float GetFinalStat(PieceInstance piece, string statName)
        {
            if (piece == null) return 0f;
            float value = GetBaseStatFromDefinition(piece.Definition, statName);

            foreach (var module in piece.GetModulesForStat(statName))
            {
                if (module.IsActive())
                    value = module.Modify(value);
            }

            return value;
        }

        private static float GetBaseStatFromDefinition(PieceDefinition def, string statName)
        {
            if (def == null || string.IsNullOrEmpty(statName)) return 0f;
            switch (statName.ToLowerInvariant())
            {
                case "damage": return def.baseDamage;
                case "cooldown": return def.baseCooldown;
                case "speed": return def.baseSpeed;
                case "range": return def.detectionRange;
                case "energycost": return def.energyCost;
                default: return 0f;
            }
        }
    }
}
