using OverclockedClash.Data;

namespace OverclockedClash.Logic.Nodes
{
    public enum CompareType
    {
        Greater = 0,   // >
        Less = 1,       // <
        Equal = 2,      // =
        NotEqual = 3    // !=
    }

    /// <summary>
    /// Comparateur : result = (valueA op valueB) selon comparisonType (0=> 1=>< 2=>= 3=>!=).
    /// </summary>
    public class CompareNode : LogicNode
    {
        public CompareType Comparison { get; set; } = CompareType.Greater;

        public CompareNode(LogicNodeDefinition definition) : base(definition) { }

        public override void Evaluate()
        {
            var a = GetInput("valueA");
            var b = GetInput("valueB");
            var ct = GetInput("comparisonType");
            var output = GetOutput("result");
            if (a == null || b == null || output == null) return;

            float va = a.GetValue<float>();
            float vb = b.GetValue<float>();
            var comp = Comparison;
            if (ct != null)
            {
                int index = (int)ct.GetValue<float>();
                if (index >= 0 && index <= 3)
                    comp = (CompareType)index;
            }

            bool result = comp switch
            {
                CompareType.Greater => va > vb,
                CompareType.Less => va < vb,
                CompareType.Equal => UnityEngine.Mathf.Approximately(va, vb),
                CompareType.NotEqual => !UnityEngine.Mathf.Approximately(va, vb),
                _ => false
            };
            output.SetValue(result);
        }
    }
}
