using System;
using UnityEngine;

namespace OverclockedClash.Data
{
    /// <summary>
    /// Définition d'un module modificateur chargé depuis JSON
    /// </summary>
    [Serializable]
    public class ModuleDefinition
    {
        public string id;
        public string name;
        public float energyCost;
        public PortDefinition[] inputs;
        public PortDefinition[] outputs;
        public string type; // "multiplier", "adder", "divider", "clamp", etc.
    }
}

