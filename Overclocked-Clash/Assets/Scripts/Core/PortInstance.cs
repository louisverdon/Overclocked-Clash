using System.Collections.Generic;
using UnityEngine;

namespace OverclockedClash.Core
{
    /// <summary>
    /// Représente une instance d'un port (entrée ou sortie) sur une pièce
    /// </summary>
    public class PortInstance
    {
        public string Name { get; private set; }
        public PortType Type { get; private set; }
        public bool IsInput { get; private set; }
        
        private object _value;
        private List<PortInstance> _connections;

        public PortInstance(string name, PortType type, bool isInput)
        {
            Name = name;
            Type = type;
            IsInput = isInput;
            _connections = new List<PortInstance>();
            
            // Valeur par défaut selon le type
            _value = type switch
            {
                PortType.Bool => false,
                PortType.Number => 0f,
                PortType.Event => false,
                PortType.HUD_Number => 0f,
                _ => null
            };
        }

        /// <summary>
        /// Connecte ce port à un autre port (vérifie la compatibilité des types)
        /// </summary>
        public bool Connect(PortInstance otherPort)
        {
            if (Type != otherPort.Type)
            {
                Debug.LogWarning($"Cannot connect ports of different types: {Type} != {otherPort.Type}");
                return false;
            }

            if (IsInput == otherPort.IsInput)
            {
                Debug.LogWarning("Cannot connect two inputs or two outputs together");
                return false;
            }

            if (!_connections.Contains(otherPort))
            {
                _connections.Add(otherPort);
            }

            if (!otherPort._connections.Contains(this))
            {
                otherPort._connections.Add(this);
            }

            return true;
        }

        /// <summary>
        /// Déconnecte ce port d'un autre port
        /// </summary>
        public void Disconnect(PortInstance otherPort)
        {
            _connections.Remove(otherPort);
            otherPort._connections.Remove(this);
        }

        /// <summary>
        /// Déconnecte toutes les connexions
        /// </summary>
        public void DisconnectAll()
        {
            foreach (var connection in _connections.ToArray())
            {
                Disconnect(connection);
            }
        }

        /// <summary>
        /// Obtient la valeur actuelle du port selon son type
        /// </summary>
        public T GetValue<T>()
        {
            if (_value is T value)
                return value;
            return default(T);
        }

        /// <summary>
        /// Définit la valeur du port
        /// </summary>
        public void SetValue(object value)
        {
            _value = value;
        }

        /// <summary>
        /// Propage la valeur à tous les ports connectés (pour les sorties)
        /// </summary>
        public void Propagate()
        {
            if (IsInput) return; // Seules les sorties propagent

            foreach (var connection in _connections)
            {
                connection.SetValue(_value);
            }
        }

        public List<PortInstance> GetConnections()
        {
            return new List<PortInstance>(_connections);
        }
    }
}

