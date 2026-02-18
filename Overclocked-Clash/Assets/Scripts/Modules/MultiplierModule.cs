using OverclockedClash.Data;

namespace OverclockedClash.Modules
{
    public class MultiplierModule : StatModule
    {
        public MultiplierModule(ModuleDefinition definition) : base(definition) { }

        protected override float ModifyInternal(float currentValue)
        {
            var mult = GetInput("multiplier");
            float factor = mult != null ? mult.GetValue<float>() : 1f;
            return currentValue * factor;
        }
    }
}
