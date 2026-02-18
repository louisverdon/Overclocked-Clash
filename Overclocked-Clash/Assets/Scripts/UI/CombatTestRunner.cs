using System.Collections.Generic;
using UnityEngine;
using OverclockedClash.Combat;
using OverclockedClash.Core;
using OverclockedClash.Testing;

namespace OverclockedClash.UI
{
    /// <summary>
    /// Exécute un combat de test et retourne/affiche les résultats.
    /// RunCombat(bot) : exécute le combat en mode synchrone, retourne TestResult.
    /// Peut aussi être utilisé comme MonoBehaviour dans une scène CombatTest dédiée.
    /// </summary>
    public class CombatTestRunner : MonoBehaviour
    {
        /// <summary>
        /// Exécute un combat contre la cible de test (20 PV) et retourne le résultat.
        /// </summary>
        public static TestResult RunCombat(BotInstance bot)
        {
            var target = TestTarget.CreateTargetBot();
            var engine = new CombatEngine();
            engine.Initialize(bot.Clone(), target);

            foreach (var piece in engine.Bot1.Pieces)
            {
                if (piece.Definition?.behavior == "weapon")
                    piece.GetInput("trigger")?.SetValue(true);
            }

            var inputs = new Dictionary<int, PlayerInput>
            {
                [0] = new PlayerInput(true, true, true)
            };

            int tick = 0;
            while (!engine.IsFinished && tick < 200)
            {
                engine.ExecuteMicroTick(inputs);
                tick++;
            }

            bool won = engine.Bot2 != null && engine.Bot2.IsDestroyed();
            float damage = 20f - (engine.Bot2?.Core?.CurrentHP ?? 0);

            var result = new TestResult
            {
                IsValid = won,
                TicksExecuted = tick,
                DamageDealt = damage
            };
            Debug.Log($"[CombatTest] Terminé en {tick} micro-ticks. Victoire: {won}. Dégâts infligés: {damage}");
            return result;
        }
    }
}
