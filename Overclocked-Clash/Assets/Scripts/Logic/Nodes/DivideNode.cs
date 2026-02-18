using OverclockedClash.Data;
using UnityEngine;

namespace OverclockedClash.Logic.Nodes
{
    public class DivideNode : LogicNode
    {
        public DivideNode(LogicNodeDefinition definition) : base(definition) { }

        public override void Evaluate()
        {
            var a = GetInput("valueA");
            var b = GetInput("valueB");
            var output = GetOutput("output");
            if (a == null || b == null || output == null) return;

            float vb = b.GetValue<float>();
            float result = Mathf.Approximately(vb, 0f) ? 0f : (a.GetValue<float>() / vb);
            output.SetValue(result);
        }
    }
}
