using OverclockedClash.Data;

namespace OverclockedClash.Logic.Nodes
{
    public class NotNode : LogicNode
    {
        public NotNode(LogicNodeDefinition definition) : base(definition) { }

        public override void Evaluate()
        {
            var input = GetInput("input");
            var output = GetOutput("output");
            if (input == null || output == null) return;

            output.SetValue(!input.GetValue<bool>());
        }
    }
}
