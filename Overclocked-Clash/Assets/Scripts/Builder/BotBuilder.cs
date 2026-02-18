using System.Collections.Generic;
using UnityEngine;
using OverclockedClash.Core;
using OverclockedClash.Data;

namespace OverclockedClash.Builder
{
    /// <summary>
    /// Gère la construction d'un bot : pièces, positions sur la grille, connexions entre ports.
    /// </summary>
    public class BotBuilder
    {
        private BotInstance _bot = new BotInstance();
        private readonly Dictionary<PieceInstance, Vector2> _piecePositions = new Dictionary<PieceInstance, Vector2>();

        public IReadOnlyDictionary<PieceInstance, Vector2> PiecePositions => _piecePositions;
        public IReadOnlyList<PieceInstance> Pieces => _bot.Pieces;

        /// <summary>
        /// Ajoute une pièce au bot à la position donnée (pour l'affichage sur la grille).
        /// </summary>
        public PieceInstance AddPiece(string pieceId, Vector2 position)
        {
            var loader = DataLoader.Instance;
            if (loader?.Pieces == null || !loader.Pieces.TryGetValue(pieceId, out var def))
            {
                Debug.LogError($"Piece '{pieceId}' introuvable.");
                return null;
            }

            var piece = PieceFactory.Create(def);
            if (piece == null) return null;

            _bot.AddPiece(piece);
            _piecePositions[piece] = position;
            return piece;
        }

        /// <summary>
        /// Connecte deux ports (un input, un output).
        /// </summary>
        public bool ConnectPorts(PortInstance port1, PortInstance port2)
        {
            return _bot.ConnectPorts(port1, port2);
        }

        /// <summary>
        /// Construit le BotInstance final (avec ApplyEnergyBonuses).
        /// </summary>
        public BotInstance BuildBot()
        {
            _bot.ApplyEnergyBonuses();
            return _bot;
        }

        /// <summary>
        /// Réinitialise le builder pour construire un nouveau bot.
        /// </summary>
        public void Reset()
        {
            _bot = new BotInstance();
            _piecePositions.Clear();
        }
    }
}
