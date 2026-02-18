using OverclockedClash.Core;

namespace OverclockedClash.Combat
{
    /// <summary>
    /// Gère l'application des dégâts aux pièces. Déterministe (pas de Random).
    /// </summary>
    public class DamageSystem
    {
        /// <summary>
        /// Applique des dégâts à une pièce. Si la pièce est détruite, aucun effet supplémentaire (le bot sera considéré détruit si c'était le core).
        /// </summary>
        public void ApplyDamage(PieceInstance target, float damage)
        {
            if (target == null || damage <= 0) return;
            int amount = (int)damage;
            if (amount <= 0) return;
            target.TakeDamage(amount);
        }
    }
}
