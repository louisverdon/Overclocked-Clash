using OverclockedClash.Core;
using OverclockedClash.Data;

namespace OverclockedClash.Testing
{
    /// <summary>
    /// Cible de test : bot minimal 20 PV, immobile, sans défense.
    /// Utilisée par TestEngine pour valider qu'un bot peut détruire une cible.
    /// </summary>
    public static class TestTarget
    {
        public const int TargetHP = 20;

        /// <summary>
        /// Crée un bot cible (unité centrale seule, 20 PV). Pas de mouvement, pas d'arme.
        /// </summary>
        public static BotInstance CreateTargetBot()
        {
            var def = new PieceDefinition
            {
                id = "test_target",
                name = "Cible de test",
                category = "core",
                maxHP = TargetHP,
                energyCost = 0,
                energyCapacity = 1,
                energyRegen = 0,
                baseDamage = 0,
                baseCooldown = 0,
                detectionRange = 0,
                baseSpeed = 0,
                inputs = new PortDefinition[]
                {
                    new PortDefinition { name = "playerInputA", type = "bool" },
                    new PortDefinition { name = "playerInputB", type = "bool" },
                    new PortDefinition { name = "playerInputC", type = "bool" }
                },
                outputs = new PortDefinition[]
                {
                    new PortDefinition { name = "energyLevel", type = "number" },
                    new PortDefinition { name = "coreHP", type = "number" }
                },
                anchorPoints = 0,
                behavior = "core"
            };

            var core = new UnitCore(def);
            var bot = new BotInstance();
            bot.AddPiece(core);
            return bot;
        }
    }
}
