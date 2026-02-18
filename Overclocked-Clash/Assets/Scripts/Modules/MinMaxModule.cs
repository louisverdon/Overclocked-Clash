using OverclockedClash.Data;
using UnityEngine;

namespace OverclockedClash.Modules
{
    /// <summary>
    /// Retourne le min ou le max de deux valeurs (valueA, valueB).
    /// Mode 0 = min, 1 = max (port optionnel "mode" number).
    /// </summary>
    public class MinMaxModule : StatModule
    {
        public bool UseMax { get; set; }

        public MinMaxModule(ModuleDefinition definition) : base(definition) { }

        protected override float ModifyInternal(float currentValue)
        {
            var a = GetInput("valueA");
            var b = GetInput("valueB");
            var modePort = GetInput("mode");
            bool useMax = UseMax;
            if (modePort != null)
                useMax = modePort.GetValue<float>() >= 0.5f;
            float va = a != null ? a.GetValue<float>() : currentValue;
            float vb = b != null ? b.GetValue<float>() : currentValue;
            return useMax ? Mathf.Max(va, vb) : Mathf.Min(va, vb);
        }
    }
}
