using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace OverclockedClash.Data
{
    /// <summary>
    /// Chargeur singleton pour tous les fichiers JSON de définition
    /// </summary>
    public class DataLoader : MonoBehaviour
    {
        private static DataLoader _instance;
        public static DataLoader Instance
        {
            get
            {
                if (_instance == null)
                {
                    GameObject go = new GameObject("DataLoader");
                    _instance = go.AddComponent<DataLoader>();
                    DontDestroyOnLoad(go);
                }
                return _instance;
            }
        }

        private Dictionary<string, PieceDefinition> _pieces;
        private Dictionary<string, ModuleDefinition> _modules;
        private Dictionary<string, LogicNodeDefinition> _logicNodes;

        public Dictionary<string, PieceDefinition> Pieces => _pieces;
        public Dictionary<string, ModuleDefinition> Modules => _modules;
        public Dictionary<string, LogicNodeDefinition> LogicNodes => _logicNodes;

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }
            _instance = this;
            DontDestroyOnLoad(gameObject);

            LoadAllData();
        }

        private void LoadAllData()
        {
            _pieces = LoadPieces();
            _modules = LoadModules();
            _logicNodes = LoadLogicNodes();
        }

        private Dictionary<string, PieceDefinition> LoadPieces()
        {
            string path = Path.Combine(Application.dataPath, "Data", "pieces.json");
            if (!File.Exists(path))
            {
                Debug.LogError($"Pieces file not found at: {path}");
                return new Dictionary<string, PieceDefinition>();
            }

            string json = File.ReadAllText(path);
            var wrapper = JsonUtility.FromJson<PiecesWrapper>(json);
            
            var dict = new Dictionary<string, PieceDefinition>();
            foreach (var piece in wrapper.pieces)
            {
                dict[piece.id] = piece;
            }
            
            Debug.Log($"Loaded {dict.Count} pieces");
            return dict;
        }

        private Dictionary<string, ModuleDefinition> LoadModules()
        {
            string path = Path.Combine(Application.dataPath, "Data", "modules.json");
            if (!File.Exists(path))
            {
                Debug.LogError($"Modules file not found at: {path}");
                return new Dictionary<string, ModuleDefinition>();
            }

            string json = File.ReadAllText(path);
            var wrapper = JsonUtility.FromJson<ModulesWrapper>(json);
            
            var dict = new Dictionary<string, ModuleDefinition>();
            foreach (var module in wrapper.modules)
            {
                dict[module.id] = module;
            }
            
            Debug.Log($"Loaded {dict.Count} modules");
            return dict;
        }

        private Dictionary<string, LogicNodeDefinition> LoadLogicNodes()
        {
            string path = Path.Combine(Application.dataPath, "Data", "logicNodes.json");
            if (!File.Exists(path))
            {
                Debug.LogError($"Logic nodes file not found at: {path}");
                return new Dictionary<string, LogicNodeDefinition>();
            }

            string json = File.ReadAllText(path);
            var wrapper = JsonUtility.FromJson<LogicNodesWrapper>(json);
            
            var dict = new Dictionary<string, LogicNodeDefinition>();
            foreach (var node in wrapper.logicNodes)
            {
                dict[node.id] = node;
            }
            
            Debug.Log($"Loaded {dict.Count} logic nodes");
            return dict;
        }
    }

    // Wrappers pour la désérialisation JSON
    [System.Serializable]
    public class PiecesWrapper
    {
        public PieceDefinition[] pieces;
    }

    [System.Serializable]
    public class ModulesWrapper
    {
        public ModuleDefinition[] modules;
    }

    [System.Serializable]
    public class LogicNodesWrapper
    {
        public LogicNodeDefinition[] logicNodes;
    }
}

