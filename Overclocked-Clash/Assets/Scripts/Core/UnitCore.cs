using UnityEngine;
using OverclockedClash.Data;

namespace OverclockedClash.Core
{
    /// <summary>
    /// Unité centrale d'un bot : hérite de PieceInstance, gère l'énergie et les inputs joueur (A/B/C).
    /// Implémente IEnergyConsumer pour que les pièces puissent consommer l'énergie du bot.
    /// </summary>
    public class UnitCore : PieceInstance, IEnergyConsumer
    {
        private float _baseMaxEnergy;
        private float _capacityBonus;

        public float CurrentEnergy { get; private set; }
        public float BaseMaxEnergy => _baseMaxEnergy;
        public float CapacityBonus => _capacityBonus;
        public float MaxEnergy => _baseMaxEnergy + _capacityBonus;
        public float EnergyRegen => Definition?.energyRegen ?? 0f;

        public UnitCore(PieceDefinition definition) : base(definition)
        {
            _baseMaxEnergy = definition?.energyCapacity ?? 100f;
            _capacityBonus = 0f;
            CurrentEnergy = MaxEnergy;
        }

        /// <summary>
        /// Tente de consommer de l'énergie. Retourne false si pas assez.
        /// </summary>
        public bool TryConsumeEnergy(float amount)
        {
            if (amount <= 0) return true;
            if (CurrentEnergy < amount) return false;
            CurrentEnergy -= amount;
            return true;
        }

        /// <summary>
        /// Recharge l'énergie (générateurs, panneaux solaires, etc.)
        /// </summary>
        public void RechargeEnergy(float amount)
        {
            AddEnergy(amount);
        }

        /// <summary>
        /// Ajoute de l'énergie (plafonnée à MaxEnergy).
        /// </summary>
        public void AddEnergy(float amount)
        {
            CurrentEnergy = Mathf.Min(MaxEnergy, CurrentEnergy + amount);
        }

        /// <summary>
        /// Ajoute un bonus de capacité (ex. batteries). À appeler après construction du bot.
        /// </summary>
        public void AddCapacityBonus(float bonus)
        {
            if (bonus > 0) _capacityBonus += bonus;
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
