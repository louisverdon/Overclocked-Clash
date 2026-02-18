using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using OverclockedClash.Data;
using OverclockedClash.Modules;

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
        private List<StatModule> _attachedModules;

        public IReadOnlyList<PortInstance> InputPorts => _inputPorts;
        public IReadOnlyList<PortInstance> OutputPorts => _outputPorts;
        public IReadOnlyList<StatModule> Modules => _attachedModules;

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
            _attachedModules = new List<StatModule>();
        }

        /// <summary>
        /// Attache un module modificateur de stat à cette pièce pour la stat donnée.
        /// </summary>
        public void AddModule(StatModule module, string statName)
        {
            if (module == null) return;
            module.StatName = statName;
            _attachedModules.Add(module);
        }

        /// <summary>
        /// Retourne les modules attachés qui modifient la stat donnée.
        /// </summary>
        public IEnumerable<StatModule> GetModulesForStat(string statName)
        {
            return _attachedModules.Where(m => m.StatName == statName);
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
