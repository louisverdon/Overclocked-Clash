using System.Collections.Generic;
using UnityEngine;
using OverclockedClash.Data;

namespace OverclockedClash.Core
{
    /// <summary>
    /// Représente un bot complet : pièces, unité centrale, et graphe de connexions (connexions entre ports).
    /// </summary>
    public class BotInstance
    {
        private List<PieceInstance> _pieces;
        private UnitCore _core;

        public IReadOnlyList<PieceInstance> Pieces => _pieces;
        public UnitCore Core => _core;

        public BotInstance()
        {
            _pieces = new List<PieceInstance>();
        }

        /// <summary>
        /// Ajoute une pièce au bot. Si c'est l'unité centrale (behavior "core"), elle est enregistrée comme Core.
        /// </summary>
        public void AddPiece(PieceInstance piece)
        {
            if (piece == null) return;
            _pieces.Add(piece);
            if (piece is UnitCore uc)
                _core = uc;
            else if (piece.Definition?.behavior == "core" && _core == null)
            {
                // Définition core mais pas encore d'UnitCore : on ne convertit pas après coup, le caller doit ajouter un UnitCore
                Debug.LogWarning("Piece with behavior 'core' added as PieceInstance; use UnitCore for the central unit.");
            }
        }

        /// <summary>
        /// Connecte deux ports (un input, un output). Vérifie les types et la direction.
        /// </summary>
        public bool ConnectPorts(PortInstance port1, PortInstance port2)
        {
            if (port1 == null || port2 == null) return false;
            return port1.Connect(port2);
        }

        /// <summary>
        /// Le bot est détruit si l'unité centrale est détruite.
        /// </summary>
        public bool IsDestroyed()
        {
            return _core != null && _core.IsDestroyed();
        }

        /// <summary>
        /// Clone le bot pour les tests (copie des pièces et des connexions).
        /// </summary>
        public BotInstance Clone()
        {
            var clone = new BotInstance();
            var pieceMap = new Dictionary<PieceInstance, PieceInstance>();

            foreach (var p in _pieces)
            {
                PieceInstance newPiece;
                if (p is UnitCore)
                    newPiece = new UnitCore(p.Definition);
                else
                    newPiece = new PieceInstance(p.Definition);

                int damageToApply = p.MaxHP - p.CurrentHP;
                if (damageToApply > 0)
                    newPiece.TakeDamage(damageToApply);

                pieceMap[p] = newPiece;
                clone.AddPiece(newPiece);
            }

            // Reconnecter les ports : pour chaque input, retrouver l'output connecté et connecter les clones
            foreach (var p in _pieces)
            {
                var newPiece = pieceMap[p];
                if (newPiece == null) continue;

                for (int i = 0; i < p.InputPorts.Count; i++)
                {
                    var oldInput = p.InputPorts[i];
                    var newInput = newPiece.InputPorts[i];
                    foreach (var oldOutputPort in oldInput.GetConnections())
                    {
                        bool found = false;
                        foreach (var src in _pieces)
                        {
                            for (int j = 0; j < src.OutputPorts.Count; j++)
                            {
                                if (src.OutputPorts[j] != oldOutputPort) continue;
                                var newOutputPort = pieceMap[src].OutputPorts[j];
                                newOutputPort.Connect(newInput);
                                found = true;
                                break;
                            }
                            if (found) break;
                        }
                    }
                }
            }

            return clone;
        }
    }
}
