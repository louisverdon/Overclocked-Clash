using System.Collections.Generic;
using OverclockedClash.Data;

namespace OverclockedClash.Logic.Nodes
{
    /// <summary>
    /// Retarde un signal booléen de N ticks (delayTicks, lu sur l'entrée number).
    /// </summary>
    public class DelayNode : LogicNode
    {
        private readonly Queue<bool> _buffer = new Queue<bool>();
        private int _delayTicks = 1;

        public DelayNode(LogicNodeDefinition definition) : base(definition) { }

        public override void Evaluate()
        {
            var input = GetInput("input");
            var delayIn = GetInput("delayTicks");
            var output = GetOutput("output");
            if (input == null || output == null) return;

            if (delayIn != null)
            {
                int d = (int)delayIn.GetValue<float>();
                if (d < 0) d = 0;
                _delayTicks = d;
            }

            bool value = input.GetValue<bool>();
            if (_delayTicks == 0)
            {
                output.SetValue(value);
                return;
            }

            _buffer.Enqueue(value);

            bool outValue = false;
            while (_buffer.Count > _delayTicks)
                outValue = _buffer.Dequeue();

            output.SetValue(outValue);
        }
    }
}
