using OverclockedClash.Core;
using OverclockedClash.Data;

namespace OverclockedClash.Pieces
{
    /// <summary>
    /// Pièce batterie : étend la capacité max d'énergie de l'unité centrale.
    /// Le bot doit appeler ApplyEnergyBonuses() pour appliquer ce bonus au core.
    /// </summary>
    public class BatteryPiece : PieceInstance
    {
        public BatteryPiece(PieceDefinition definition) : base(definition) { }

        /// <summary>
        /// Bonus de capacité (à ajouter à UnitCore via AddCapacityBonus).
        /// </summary>
        public float GetCapacityBonus()
        {
            return Definition?.energyCapacityBonus ?? 0f;
        }
    }
}
