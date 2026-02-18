using System;
using UnityEngine;

namespace OverclockedClash.Data
{
    /// <summary>
    /// Définition d'une pièce chargée depuis JSON
    /// </summary>
    [Serializable]
    public class PieceDefinition
    {
        public string id;
        public string name;
        public string category;
        public int maxHP;
        public float energyCost;
        public float energyCapacity; // Pour l'unité centrale
        public float energyRegen; // Pour l'unité centrale
        
        // Stats spécifiques selon le type de pièce
        public float baseDamage;
        public float baseCooldown;
        public float detectionRange;
        public float baseSpeed;
        
        public PortDefinition[] inputs;
        public PortDefinition[] outputs;
        public int anchorPoints;
        public string behavior; // "weapon", "radar", "movement", "core", etc.
    }
}

