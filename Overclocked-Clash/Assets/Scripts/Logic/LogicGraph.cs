using System.Collections.Generic;
using System.Linq;
using OverclockedClash.Core;

namespace OverclockedClash.Logic
{
    /// <summary>
    /// Gère un graphe de nœuds logiques : ordre d'évaluation et propagation des signaux.
    /// </summary>
    public class LogicGraph
    {
        private readonly List<LogicNode> _nodes = new List<LogicNode>();

        public IReadOnlyList<LogicNode> Nodes => _nodes;

        public void AddNode(LogicNode node)
        {
            if (node != null && !_nodes.Contains(node))
                _nodes.Add(node);
        }

        public void RemoveNode(LogicNode node)
        {
            _nodes.Remove(node);
        }

        /// <summary>
        /// Retourne le nœud qui possède ce port en sortie, ou null si c'est une sortie externe (ex. pièce).
        /// </summary>
        private LogicNode FindOwnerOfOutputPort(PortInstance port)
        {
            if (port == null || port.IsInput) return null;
            foreach (var node in _nodes)
            {
                foreach (var p in node.OutputPorts)
                    if (p == port) return node;
            }
            return null;
        }

        /// <summary>
        /// Construit les dépendances : pour chaque nœud, liste des nœuds dont il dépend (entrées connectées à leurs sorties).
        /// </summary>
        private Dictionary<LogicNode, HashSet<LogicNode>> BuildDependencies()
        {
            var deps = new Dictionary<LogicNode, HashSet<LogicNode>>();
            foreach (var node in _nodes)
                deps[node] = new HashSet<LogicNode>();

            foreach (var node in _nodes)
            {
                foreach (var inputPort in node.InputPorts)
                {
                    foreach (var connected in inputPort.GetConnections())
                    {
                        var owner = FindOwnerOfOutputPort(connected);
                        if (owner != null && owner != node)
                            deps[node].Add(owner);
                    }
                }
            }
            return deps;
        }

        /// <summary>
        /// Tri topologique : les nœuds dont les entrées dépendent d'autres nœuds sont évalués après ceux-ci.
        /// </summary>
        private List<LogicNode> TopologicalSort()
        {
            var deps = BuildDependencies();
            var result = new List<LogicNode>();
            var remaining = new HashSet<LogicNode>(_nodes);

            while (remaining.Count > 0)
            {
                bool progress = false;
                foreach (var n in remaining.ToList())
                {
                    var nodeDeps = deps[n];
                    if (nodeDeps.All(d => !remaining.Contains(d)))
                    {
                        result.Add(n);
                        remaining.Remove(n);
                        progress = true;
                        break;
                    }
                }
                if (!progress)
                    break;
            }

            foreach (var n in remaining)
                result.Add(n);
            return result;
        }

        /// <summary>
        /// Évalue tous les nœuds dans l'ordre des dépendances, puis propage les sorties.
        /// </summary>
        public void EvaluateAll()
        {
            var order = TopologicalSort();
            foreach (var node in order)
            {
                node.Evaluate();
                node.PropagateOutputs();
            }
        }

        /// <summary>
        /// Propage les valeurs des sorties vers les ports connectés (sans recalculer les nœuds).
        /// </summary>
        public void PropagateSignals()
        {
            foreach (var node in _nodes)
                node.PropagateOutputs();
        }
    }
}
