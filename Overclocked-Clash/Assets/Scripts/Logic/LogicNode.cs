using System.Collections.Generic;
using OverclockedClash.Core;
using OverclockedClash.Data;

namespace OverclockedClash.Logic
{
    /// <summary>
    /// Classe abstraite de base pour tous les nœuds logiques.
    /// Chaque nœud possède des ports d'entrée/sortie et recalcule ses sorties dans Evaluate().
    /// </summary>
    public abstract class LogicNode
    {
        private readonly List<PortInstance> _inputPorts;
        private readonly List<PortInstance> _outputPorts;

        public LogicNodeDefinition Definition { get; }
        public IReadOnlyList<PortInstance> InputPorts => _inputPorts;
        public IReadOnlyList<PortInstance> OutputPorts => _outputPorts;

        protected LogicNode(LogicNodeDefinition definition)
        {
            Definition = definition ?? throw new System.ArgumentNullException(nameof(definition));
            _inputPorts = new List<PortInstance>();
            _outputPorts = new List<PortInstance>();

            if (definition.inputs != null)
            {
                foreach (var p in definition.inputs)
                {
                    var pt = PortTypeExtensions.FromString(p.type);
                    _inputPorts.Add(new PortInstance(p.name, pt, isInput: true));
                }
            }
            if (definition.outputs != null)
            {
                foreach (var p in definition.outputs)
                {
                    var pt = PortTypeExtensions.FromString(p.type);
                    _outputPorts.Add(new PortInstance(p.name, pt, isInput: false));
                }
            }
        }

        protected PortInstance GetInput(string name)
        {
            foreach (var p in _inputPorts)
                if (p.Name == name) return p;
            return null;
        }

        protected PortInstance GetOutput(string name)
        {
            foreach (var p in _outputPorts)
                if (p.Name == name) return p;
            return null;
        }

        /// <summary>
        /// Recalcule les sorties à partir des entrées et écrit les valeurs dans les ports de sortie.
        /// N'appelle pas Propagate ; le graphe s'en charge après chaque nœud.
        /// </summary>
        public abstract void Evaluate();

        /// <summary>
        /// Propage les valeurs des ports de sortie vers les ports connectés.
        /// </summary>
        public void PropagateOutputs()
        {
            foreach (var port in _outputPorts)
                port.Propagate();
        }
    }
}
