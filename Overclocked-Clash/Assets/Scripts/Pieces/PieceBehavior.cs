using OverclockedClash.Combat;
using OverclockedClash.Core;

namespace OverclockedClash.Pieces
{
    /// <summary>
    /// Comportement abstrait d'une pièce pendant le combat. Chaque type (weapon, radar, movement) a une implémentation.
    /// </summary>
    public abstract class PieceBehavior
    {
        /// <summary>
        /// Exécute le comportement : vérifie l'énergie, consomme si besoin, applique l'effet (dégâts, mouvement, détection).
        /// </summary>
        public abstract void Execute(PieceInstance piece, CombatContext context);
    }
}
