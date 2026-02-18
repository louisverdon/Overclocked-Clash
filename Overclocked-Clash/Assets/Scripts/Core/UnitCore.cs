using UnityEngine;
using OverclockedClash.Data;

namespace OverclockedClash.Core
{
    /// <summary>
    /// Unité centrale d'un bot : hérite de PieceInstance, gère l'énergie et les inputs joueur (A/B/C).
    /// </summary>
    public class UnitCore : PieceInstance
    {
        public float CurrentEnergy { get; private set; }
        public float MaxEnergy { get; private set; }
        public float EnergyRegen => Definition?.energyRegen ?? 0f;

        public UnitCore(PieceDefinition definition) : base(definition)
        {
            MaxEnergy = definition?.energyCapacity ?? 100f;
            CurrentEnergy = MaxEnergy;
        }

        /// <summary>
        /// Tente de consommer de l'énergie. Retourne true si possible.
        /// </summary>
        public bool TryConsumeEnergy(float amount)
        {
            if (amount <= 0) return true;
            if (CurrentEnergy < amount) return false;
            CurrentEnergy -= amount;
            return true;
        }

        /// <summary>
        /// Recharge l'énergie (générateurs, etc.)
        /// </summary>
        public void AddEnergy(float amount)
        {
            CurrentEnergy = Mathf.Min(MaxEnergy, CurrentEnergy + amount);
        }

        /// <summary>
        /// Régénération passive (à appeler chaque tick si besoin).
        /// </summary>
        public void RegenEnergy()
        {
            AddEnergy(EnergyRegen);
        }

        /// <summary>
        /// Récupère l'état de l'input joueur (0=A, 1=B, 2=C).
        /// </summary>
        public bool GetPlayerInput(int index)
        {
            var names = new[] { "playerInputA", "playerInputB", "playerInputC" };
            if (index < 0 || index >= names.Length) return false;
            var port = GetInput(names[index]);
            return port != null && port.GetValue<bool>();
        }
    }
}
