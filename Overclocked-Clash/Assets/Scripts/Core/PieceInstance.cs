using System.Collections.Generic;
using UnityEngine;
using OverclockedClash.Data;

namespace OverclockedClash.Core
{
    /// <summary>
    /// Représente une pièce instanciée dans un bot (d'après une PieceDefinition).
    /// </summary>
    public class PieceInstance
    {
        public PieceDefinition Definition { get; private set; }
        public int CurrentHP { get; private set; }
        public int MaxHP => Definition?.maxHP ?? 0;

        private List<PortInstance> _inputPorts;
        private List<PortInstance> _outputPorts;

        public IReadOnlyList<PortInstance> InputPorts => _inputPorts;
        public IReadOnlyList<PortInstance> OutputPorts => _outputPorts;

        public PieceInstance(PieceDefinition definition)
        {
            Definition = definition ?? throw new System.ArgumentNullException(nameof(definition));
            CurrentHP = definition.maxHP;
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

        public void TakeDamage(int amount)
        {
            CurrentHP = Mathf.Max(0, CurrentHP - amount);
        }

        public bool IsDestroyed()
        {
            return CurrentHP <= 0;
        }

        /// <summary>
        /// Retourne un port (entrée ou sortie) par son nom, ou null.
        /// </summary>
        public PortInstance GetPort(string name)
        {
            foreach (var p in _inputPorts)
                if (p.Name == name) return p;
            foreach (var p in _outputPorts)
                if (p.Name == name) return p;
            return null;
        }

        public PortInstance GetInput(string name)
        {
            foreach (var p in _inputPorts)
                if (p.Name == name) return p;
            return null;
        }

        public PortInstance GetOutput(string name)
        {
            foreach (var p in _outputPorts)
                if (p.Name == name) return p;
            return null;
        }
    }
}
