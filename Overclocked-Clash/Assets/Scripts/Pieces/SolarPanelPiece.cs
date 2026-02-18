using OverclockedClash.Core;
using OverclockedClash.Data;

namespace OverclockedClash.Pieces
{
    /// <summary>
    /// Pièce panneau solaire : variante de générateur (énergie par tick).
    /// Même principe que GeneratorPiece ; peut avoir des modificateurs différents plus tard (ex. ensoleillement).
    /// </summary>
    public class SolarPanelPiece : PieceInstance
    {
        public SolarPanelPiece(PieceDefinition definition) : base(definition) { }

        /// <summary>
        /// Énergie générée par tick.
        /// </summary>
        public float GetEnergyPerTick()
        {
            return Definition?.energyPerTick ?? 0f;
        }
    }
}
