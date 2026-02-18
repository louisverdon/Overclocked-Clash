using OverclockedClash.Combat;
using OverclockedClash.Core;

namespace OverclockedClash.Pieces
{
    /// <summary>
    /// Comportement arme : si trigger actif et cooldown à 0, consomme l'énergie et applique les dégâts à la cible (core ennemi pour le POC).
    /// </summary>
    public class WeaponBehavior : PieceBehavior
    {
        public override void Execute(PieceInstance piece, CombatContext context)
        {
            if (context?.EnemyBot == null || context.DamageSystem == null) return;
            if (context.EnergyConsumer == null) return;

            int cooldown = context.CooldownRemaining.TryGetValue(piece, out int cd) ? cd : 0;
            if (cooldown > 0) return;

            var trigger = piece.GetInput("trigger");
            if (trigger == null || !trigger.GetValue<bool>()) return;

            float damage = StatCalculator.GetFinalStat(piece, "damage");
            if (damage <= 0) return;

            float cost = piece.GetEnergyCost();
            if (cost > 0 && !context.EnergyConsumer.TryConsumeEnergy(cost))
                return;

            if (context.EnemyBot.Core != null && !context.EnemyBot.Core.IsDestroyed())
                context.DamageSystem.ApplyDamage(context.EnemyBot.Core, damage);

            int newCooldown = (int)StatCalculator.GetFinalStat(piece, "cooldown");
            if (newCooldown < 0) newCooldown = 0;
            context.CooldownRemaining[piece] = newCooldown;
        }
    }
}
