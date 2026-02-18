using OverclockedClash.Data;

namespace OverclockedClash.Logic
{
    /// <summary>
    /// Crée des instances de LogicNode à partir des définitions (DataLoader.LogicNodes).
    /// </summary>
    public static class LogicNodeFactory
    {
        public static LogicNode Create(LogicNodeDefinition definition)
        {
            if (definition == null) return null;
            string type = definition.type?.ToLowerInvariant() ?? "";

            return type switch
            {
                "and" => new Nodes.AndNode(definition),
                "or" => new Nodes.OrNode(definition),
                "not" => new Nodes.NotNode(definition),
                "relay" => new Nodes.RelayNode(definition),
                "toggle" => new Nodes.ToggleNode(definition),
                "compare" => new Nodes.CompareNode(definition),
                "delay" => new Nodes.DelayNode(definition),
                "add" => new Nodes.AddNode(definition),
                "subtract" => new Nodes.SubtractNode(definition),
                "multiply" => new Nodes.MultiplyNode(definition),
                "divide" => new Nodes.DivideNode(definition),
                _ => null
            };
        }
    }
}
