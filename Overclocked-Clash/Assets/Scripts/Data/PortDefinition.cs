using System;
using UnityEngine;

namespace OverclockedClash.Data
{
    /// <summary>
    /// Structure pour un port (type, nom, direction implicite via inputs/outputs dans le JSON)
    /// </summary>
    [Serializable]
    public class PortDefinition
    {
        public string name;
        public string type; // "bool", "number", "event", "hud_number"
    }
}
