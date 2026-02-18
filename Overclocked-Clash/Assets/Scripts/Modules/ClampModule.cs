using OverclockedClash.Data;
using UnityEngine;

namespace OverclockedClash.Modules
{
    public class ClampModule : StatModule
    {
        public ClampModule(ModuleDefinition definition) : base(definition) { }

        protected override float ModifyInternal(float currentValue)
        {
            var minPort = GetInput("min");
            var maxPort = GetInput("max");
            float min = minPort != null ? minPort.GetValue<float>() : float.MinValue;
            float max = maxPort != null ? maxPort.GetValue<float>() : float.MaxValue;
            return Mathf.Clamp(currentValue, min, max);
        }
    }
}
