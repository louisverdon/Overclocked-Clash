using UnityEngine;
using OverclockedClash.Data;

namespace OverclockedClash
{
    /// <summary>
    /// Script de démarrage pour vérifier le chargement des données JSON (DataLoader).
    /// Attacher à un GameObject dans une scène et lancer Play.
    /// </summary>
    public class BootLoader : MonoBehaviour
    {
        private void Start()
        {
            var dl = DataLoader.Instance;

            Debug.Log($"Pieces: {dl.Pieces.Count}, Modules: {dl.Modules.Count}, LogicNodes: {dl.LogicNodes.Count}, Stats: {dl.Stats?.Count ?? 0}");
            if (dl.UnitCore != null)
                Debug.Log($"UnitCore: {dl.UnitCore.name} (HP={dl.UnitCore.maxHP}, Energy={dl.UnitCore.energyCapacity})");

            if (dl.Pieces.TryGetValue("canon_basic", out var canon))
                Debug.Log($"Canon chargé: {canon.name} (HP={canon.maxHP})");
            else
                Debug.LogError("canon_basic introuvable (vérifie pieces.json)");
        }
    }
}
