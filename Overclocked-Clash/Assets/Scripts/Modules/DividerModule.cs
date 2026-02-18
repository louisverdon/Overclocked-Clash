using OverclockedClash.Data;
using UnityEngine;

namespace OverclockedClash.Modules
{
    public class DividerModule : StatModule
    {
        public DividerModule(ModuleDefinition definition) : base(definition) { }

        protected override float ModifyInternal(float currentValue)
        {
            var div = GetInput("divisor");
            float divisor = div != null ? div.GetValue<float>() : 1f;
            if (Mathf.Approximately(divisor, 0f)) return currentValue;
            return currentValue / divisor;
        }
    }
}
