using System.Collections.Generic;
using OverclockedClash.Core;
using OverclockedClash.Data;

namespace OverclockedClash.Modules
{
    /// <summary>
    /// Classe abstraite pour les modules qui modifient une stat d'une pièce.
    /// Lecture du signal d'activation et des paramètres via des ports (connectés au graphe logique).
    /// </summary>
    public abstract class StatModule
    {
        private readonly List<PortInstance> _inputPorts;
        private string _statName;

        public ModuleDefinition Definition { get; }
        public string StatName { get => _statName; set => _statName = value; }
        public IReadOnlyList<PortInstance> InputPorts => _inputPorts;

        protected StatModule(ModuleDefinition definition)
        {
            Definition = definition ?? throw new System.ArgumentNullException(nameof(definition));
            _inputPorts = new List<PortInstance>();

            if (definition.inputs != null)
            {
                foreach (var p in definition.inputs)
                {
                    var pt = PortTypeExtensions.FromString(p.type);
                    _inputPorts.Add(new PortInstance(p.name, pt, isInput: true));
                }
            }
        }

        protected PortInstance GetInput(string name)
        {
            foreach (var p in _inputPorts)
                if (p.Name == name) return p;
            return null;
        }

        /// <summary>
        /// True si le signal d'activation est ON (port "activate" à true).
        /// </summary>
        public bool IsActive()
        {
            var activate = GetInput("activate");
            return activate != null && activate.GetValue<bool>();
        }

        /// <summary>
        /// Retourne la valeur modifiée. Si non actif, retourne currentValue inchangé.
        /// </summary>
        public float Modify(float currentValue)
        {
            if (!IsActive()) return currentValue;
            return ModifyInternal(currentValue);
        }

        protected abstract float ModifyInternal(float currentValue);
    }
}
