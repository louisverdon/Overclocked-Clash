using OverclockedClash.Combat;
using OverclockedClash.Core;

namespace OverclockedClash.Pieces
{
    /// <summary>
    /// Comportement radar : si enable, met à jour targetDetected et targetDistance selon la distance au bot ennemi.
    /// </summary>
    public class RadarBehavior : PieceBehavior
    {
        public override void Execute(PieceInstance piece, CombatContext context)
        {
            if (context == null) return;

            var enable = piece.GetInput("enable");
            if (enable != null && !enable.GetValue<bool>())
            {
                piece.GetOutput("targetDetected")?.SetValue(false);
                piece.GetOutput("targetDistance")?.SetValue(0f);
                return;
            }

            float range = StatCalculator.GetFinalStat(piece, "range");
            float distance = UnityEngine.Mathf.Abs(context.BotPositionX - context.EnemyPositionX);
            bool detected = distance <= range;

            piece.GetOutput("targetDetected")?.SetValue(detected);
            piece.GetOutput("targetDistance")?.SetValue(distance);
        }
    }
}
