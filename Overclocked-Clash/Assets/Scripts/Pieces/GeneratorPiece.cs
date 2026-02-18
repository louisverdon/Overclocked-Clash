using OverclockedClash.Core;
using OverclockedClash.Data;

namespace OverclockedClash.Pieces
{
    /// <summary>
    /// Pièce générateur : produit de l'énergie passivement chaque tick.
    /// Le moteur de combat doit appeler UnitCore.RechargeEnergy(GetEnergyPerTick()) chaque micro-tick.
    /// </summary>
    public class GeneratorPiece : PieceInstance
    {
        public GeneratorPiece(PieceDefinition definition) : base(definition) { }

        /// <summary>
        /// Énergie générée par tick (à passer à UnitCore.RechargeEnergy).
        /// </summary>
        public float GetEnergyPerTick()
        {
            return Definition?.energyPerTick ?? 0f;
        }
    }
}
