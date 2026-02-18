using OverclockedClash.Combat;
using OverclockedClash.Core;
using UnityEngine;

namespace OverclockedClash.Pieces
{
    /// <summary>
    /// Comportement mouvement : consomme l'énergie et déplace le bot selon forward/backward et speed (déterministe).
    /// </summary>
    public class MovementBehavior : PieceBehavior
    {
        public override void Execute(PieceInstance piece, CombatContext context)
        {
            if (context?.EnergyConsumer == null) return;

            float cost = piece.GetEnergyCost();
            if (cost > 0 && !context.EnergyConsumer.TryConsumeEnergy(cost))
                return;

            var forward = piece.GetInput("forward");
            var backward = piece.GetInput("backward");
            var speedMult = piece.GetInput("speedMultiplier");
            bool fwd = forward != null && forward.GetValue<bool>();
            bool bwd = backward != null && backward.GetValue<bool>();
            float mult = speedMult != null ? speedMult.GetValue<float>() : 1f;
            if (mult <= 0) mult = 1f;

            float speed = StatCalculator.GetFinalStat(piece, "speed");
            float delta = 0f;
            if (fwd && !bwd) delta = speed * mult;
            else if (bwd && !fwd) delta = -speed * mult;

            context.BotPositionX += delta;
        }
    }
}
