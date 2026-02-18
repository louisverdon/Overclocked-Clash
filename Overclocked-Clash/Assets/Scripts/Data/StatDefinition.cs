using System;
using UnityEngine;

namespace OverclockedClash.Data
{
    /// <summary>
    /// Définition d'une statistique modifiable (chargée depuis stats.json)
    /// </summary>
    [Serializable]
    public class StatDefinition
    {
        public string id;
        public string name;
        public string type; // "number", etc.
        public float defaultValue;
    }

    /// <summary>
    /// Identifiants des stats pour référence en code
    /// </summary>
    public enum StatId
    {
        Damage,
        Cooldown,
        Speed,
        Range,
        EnergyCost
    }
}
