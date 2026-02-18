using OverclockedClash.Data;

namespace OverclockedClash.Logic.Nodes
{
    /// <summary>
    /// Bascule / Latch : à chaque front montant du trigger, la sortie alterne (true <-> false).
    /// </summary>
    public class ToggleNode : LogicNode
    {
        private bool _state;
        private bool _lastTrigger;

        public ToggleNode(LogicNodeDefinition definition) : base(definition) { }

        public override void Evaluate()
        {
            var trigger = GetInput("trigger");
            var output = GetOutput("output");
            if (trigger == null || output == null) return;

            bool t = trigger.GetValue<bool>();
            if (t && !_lastTrigger)
                _state = !_state;
            _lastTrigger = t;
            output.SetValue(_state);
        }
    }
}
