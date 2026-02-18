using OverclockedClash.Data;

namespace OverclockedClash.Logic.Nodes
{
    /// <summary>
    /// Répéteur booléen : sortie = entrée.
    /// </summary>
    public class RelayNode : LogicNode
    {
        public RelayNode(LogicNodeDefinition definition) : base(definition) { }

        public override void Evaluate()
        {
            var input = GetInput("input");
            var output = GetOutput("output");
            if (input == null || output == null) return;

            output.SetValue(input.GetValue<bool>());
        }
    }
}
