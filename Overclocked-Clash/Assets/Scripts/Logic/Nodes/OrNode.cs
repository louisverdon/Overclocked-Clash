using OverclockedClash.Data;

namespace OverclockedClash.Logic.Nodes
{
    public class OrNode : LogicNode
    {
        public OrNode(LogicNodeDefinition definition) : base(definition) { }

        public override void Evaluate()
        {
            var a = GetInput("inputA");
            var b = GetInput("inputB");
            var output = GetOutput("output");
            if (a == null || b == null || output == null) return;

            bool va = a.GetValue<bool>();
            bool vb = b.GetValue<bool>();
            output.SetValue(va || vb);
        }
    }
}
