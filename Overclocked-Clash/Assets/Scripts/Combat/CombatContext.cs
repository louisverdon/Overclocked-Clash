using System.Collections.Generic;
using OverclockedClash.Core;

namespace OverclockedClash.Combat
{
    /// <summary>
    /// Contexte passé aux comportements de pièces pendant un micro-tick : bot courant, ennemi, énergie, dégâts, positions, cooldowns.
    /// </summary>
    public class CombatContext
    {
        public BotInstance CurrentBot { get; set; }
        public BotInstance EnemyBot { get; set; }
        public IEnergyConsumer EnergyConsumer => CurrentBot?.Core;
        public DamageSystem DamageSystem { get; set; }
        public int CurrentTick { get; set; }

        /// <summary> Position X du bot courant (mouvement déterministe). </summary>
        public float BotPositionX { get; set; }
        /// <summary> Position X du bot ennemi. </summary>
        public float EnemyPositionX { get; set; }

        /// <summary> Cooldown restant par pièce (ticks). Mis à jour par les comportements (ex. arme). </summary>
        public Dictionary<PieceInstance, int> CooldownRemaining { get; } = new Dictionary<PieceInstance, int>();
    }
}
