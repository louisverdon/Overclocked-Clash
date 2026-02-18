using OverclockedClash.Data;

namespace OverclockedClash.Modules
{
    /// <summary>
    /// Crée des instances de StatModule à partir des définitions (DataLoader.Modules).
    /// </summary>
    public static class StatModuleFactory
    {
        public static StatModule Create(ModuleDefinition definition)
        {
            if (definition == null) return null;
            string type = definition.type?.ToLowerInvariant() ?? "";

            return type switch
            {
                "multiplier" => new MultiplierModule(definition),
                "adder" => new AdderModule(definition),
                "divider" => new DividerModule(definition),
                "clamp" => new ClampModule(definition),
                "minmax" => new MinMaxModule(definition),
                _ => null
            };
        }
    }
}
