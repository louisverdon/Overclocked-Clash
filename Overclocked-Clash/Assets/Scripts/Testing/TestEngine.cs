using System.Collections.Generic;
using OverclockedClash.Combat;
using OverclockedClash.Core;

namespace OverclockedClash.Testing
{
    /// <summary>
    /// Moteur de test : valide qu'un bot peut détruire une cible fixe (20 PV) en 200 micro-ticks.
    /// </summary>
    public static class TestEngine
    {
        public const int MaxTicks = 200;

        /// <summary>
        /// Valide le bot : combat contre une cible fixe 20 PV. Retourne le résultat du test.
        /// </summary>
        /// <param name="bot">Bot à tester (sera cloné, non modifié).</param>
        /// <returns>TestResult avec IsValid = true si la cible est détruite en 200 ticks.</returns>
        public static TestResult ValidateBot(BotInstance bot)
        {
            var result = new TestResult { TicksExecuted = 0 };

            if (bot == null)
            {
                result.ErrorMessage = "Bot is null.";
                return result;
            }

            if (bot.Core == null)
            {
                result.ErrorMessage = "Bot has no core.";
                return result;
            }

            BotInstance clonedBot = bot.Clone();
            BotInstance target = TestTarget.CreateTargetBot();
            var engine = new CombatEngine();
            engine.Initialize(clonedBot, target);

            // Activer les triggers des armes (le clone n'a pas de graphe logique, donc on force le tir)
            foreach (var piece in engine.Bot1.Pieces)
            {
                if (piece.Definition?.behavior == "weapon")
                    piece.GetInput("trigger")?.SetValue(true);
            }

            var inputs = new Dictionary<int, PlayerInput>
            {
                [0] = new PlayerInput(true, true, true)
            };

            for (int i = 0; i < MaxTicks; i++)
            {
                engine.ExecuteMicroTick(inputs);
                result.TicksExecuted++;

                if (engine.IsFinished)
                    break;
            }

            result.IsValid = engine.Bot2 != null && engine.Bot2.IsDestroyed();
            float targetCurrentHP = engine.Bot2?.Core?.CurrentHP ?? 0;
            result.DamageDealt = TestTarget.TargetHP - targetCurrentHP;
            if (result.DamageDealt < 0) result.DamageDealt = 0;

            if (!result.IsValid)
                result.ErrorMessage = $"Target not destroyed after {result.TicksExecuted} ticks. Damage dealt: {result.DamageDealt}.";

            return result;
        }
    }
}
