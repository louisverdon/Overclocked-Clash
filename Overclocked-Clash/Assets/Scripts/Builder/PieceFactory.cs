using OverclockedClash.Core;
using OverclockedClash.Data;
using OverclockedClash.Pieces;

namespace OverclockedClash.Builder
{
    /// <summary>
    /// Crée des instances de pièces à partir des définitions (pour l'éditeur de bot et le BotBuilder).
    /// </summary>
    public static class PieceFactory
    {
        public static PieceInstance Create(PieceDefinition definition)
        {
            if (definition == null) return null;
            string behavior = definition.behavior?.ToLowerInvariant() ?? "";

            return behavior switch
            {
                "core" => new UnitCore(definition),
                "battery" => new BatteryPiece(definition),
                "generator" => new GeneratorPiece(definition),
                "solar" => new SolarPanelPiece(definition),
                _ => new PieceInstance(definition)
            };
        }
    }
}
