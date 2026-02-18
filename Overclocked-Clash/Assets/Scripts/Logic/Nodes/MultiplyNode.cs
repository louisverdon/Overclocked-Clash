using OverclockedClash.Data;

namespace OverclockedClash.Logic.Nodes
{
    public class MultiplyNode : LogicNode
    {
        public MultiplyNode(LogicNodeDefinition definition) : base(definition) { }

        public override void Evaluate()
        {
            var a = GetInput("valueA");
            var b = GetInput("valueB");
            var output = GetOutput("output");
            if (a == null || b == null || output == null) return;

            output.SetValue(a.GetValue<float>() * b.GetValue<float>());
        }
    }
}
