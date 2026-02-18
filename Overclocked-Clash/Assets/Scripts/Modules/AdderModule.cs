using OverclockedClash.Data;

namespace OverclockedClash.Modules
{
    public class AdderModule : StatModule
    {
        public AdderModule(ModuleDefinition definition) : base(definition) { }

        protected override float ModifyInternal(float currentValue)
        {
            var add = GetInput("addValue");
            float delta = add != null ? add.GetValue<float>() : 0f;
            return currentValue + delta;
        }
    }
}
