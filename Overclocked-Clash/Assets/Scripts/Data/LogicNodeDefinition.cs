using System;
using UnityEngine;

namespace OverclockedClash.Data
{
    /// <summary>
    /// Définition d'un nœud logique chargé depuis JSON
    /// </summary>
    [Serializable]
    public class LogicNodeDefinition
    {
        public string id;
        public string name;
        public PortDefinition[] inputs;
        public PortDefinition[] outputs;
        public string type; // "and", "or", "not", "compare", "delay", "toggle"
        public string[] comparisonTypes; // Pour les comparateurs: [">", "<", "=", "!="]
    }
}

