using System.Collections.Generic;
using UnityEngine;

namespace OverclockedClash.UI
{
    /// <summary>
    /// Dessine des lignes entre les positions des ports connectés (à attacher au Canvas ou à un GameObject avec RectTransform).
    /// Appeler SetConnections() avec la liste des paires (positionA, positionB) pour afficher les connexions.
    /// </summary>
    [RequireComponent(typeof(CanvasRenderer))]
    public class ConnectionDrawer : Graphic
    {
        private readonly List<(Vector2 a, Vector2 b)> _connections = new List<(Vector2, Vector2)>();

        protected override void Awake()
        {
            base.Awake();
            raycastTarget = false;
        }

        public void SetConnections(List<(Vector2 a, Vector2 b)> connections)
        {
            _connections.Clear();
            if (connections != null)
                _connections.AddRange(connections);
            SetVerticesDirty();
        }

        public void AddConnection(Vector2 a, Vector2 b)
        {
            _connections.Add((a, b));
            SetVerticesDirty();
        }

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();
            if (_connections.Count == 0) return;

            var color = new Color(0.6f, 0.8f, 1f, 0.8f);
            float halfWidth = 2f;

            foreach (var (a, b) in _connections)
            {
                var dir = (b - a).normalized;
                var perp = new Vector2(-dir.y, dir.x) * halfWidth;
                var v0 = a - perp;
                var v1 = a + perp;
                var v2 = b + perp;
                var v3 = b - perp;

                int idx = vh.currentVertCount;
                vh.AddVert(v0, color, Vector2.zero);
                vh.AddVert(v1, color, Vector2.zero);
                vh.AddVert(v2, color, Vector2.zero);
                vh.AddVert(v3, color, Vector2.zero);
                vh.AddTriangle(idx, idx + 1, idx + 2);
                vh.AddTriangle(idx, idx + 2, idx + 3);
            }
        }
    }
}
