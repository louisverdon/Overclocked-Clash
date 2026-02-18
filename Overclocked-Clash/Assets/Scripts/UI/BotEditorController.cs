using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using OverclockedClash.Builder;
using OverclockedClash.Core;
using OverclockedClash.Data;
using OverclockedClash.Testing;

namespace OverclockedClash.UI
{
    /// <summary>
    /// Contrôleur de l'éditeur de bot : UI avec liste des pièces, zone de construction, boutons Valider / Tester.
    /// Crée l'UI à partir d'un Canvas existant ou en crée un au démarrage.
    /// </summary>
    public class BotEditorController : MonoBehaviour
    {
        [Header("Références UI (optionnel)")]
        [SerializeField] private Transform _pieceListContainer;
        [SerializeField] private Transform _buildAreaContainer;
        [SerializeField] private Button _validateButton;
        [SerializeField] private Button _testCombatButton;
        [SerializeField] private Text _statusText;

        private BotBuilder _builder;
        private readonly List<GameObject> _pieceButtons = new List<GameObject>();
        private readonly Dictionary<PieceInstance, GameObject> _pieceDisplay = new Dictionary<PieceInstance, GameObject>();
        private bool _wiringMode;
        private PieceInstance _wiringSource;

        private const float CellSize = 60f;
        private const int GridSize = 6;

        private void Awake()
        {
            _builder = new BotBuilder();
            if (_pieceListContainer == null || _buildAreaContainer == null)
                CreateMinimalUI();
            SetupUI();
        }

        private void CreateMinimalUI()
        {
            var canvasObj = new GameObject("BotEditorCanvas");
            var canvas = canvasObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasObj.AddComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            canvasObj.AddComponent<GraphicRaycaster>();

            var root = new GameObject("Root");
            root.transform.SetParent(canvas.transform, false);
            var rootRect = root.AddComponent<RectTransform>();
            rootRect.anchorMin = Vector2.zero;
            rootRect.anchorMax = Vector2.one;
            rootRect.offsetMin = rootRect.offsetMax = Vector2.zero;

            var leftPanel = CreatePanel(root.transform, "LeftPanel", new Vector2(0, 0), new Vector2(200, 1));
            var rightPanel = CreatePanel(root.transform, "RightPanel", new Vector2(200, 0), new Vector2(1, 1));

            _pieceListContainer = CreatePanel(leftPanel.transform, "PieceList", Vector2.zero, Vector2.one).transform;
            _buildAreaContainer = CreatePanel(rightPanel.transform, "BuildArea", Vector2.zero, new Vector2(1, 0.8f)).transform;

            var buttonPanel = CreatePanel(rightPanel.transform, "Buttons", new Vector2(0, 0.8f), new Vector2(1, 1));
            _validateButton = CreateButton(buttonPanel.transform, "Valider le bot", OnValidateClicked);
            _validateButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(-120, -20);
            _testCombatButton = CreateButton(buttonPanel.transform, "Tester en combat", OnTestCombatClicked);
            _testCombatButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(40, -20);
            CreateButton(buttonPanel.transform, "Mode câblage", OnWiringToggle).GetComponent<RectTransform>().anchoredPosition = new Vector2(120, -20);

            var statusObj = new GameObject("Status");
            statusObj.transform.SetParent(rightPanel.transform);
            var statusRect = statusObj.AddComponent<RectTransform>();
            statusRect.anchorMin = new Vector2(0, 0.9f);
            statusRect.anchorMax = new Vector2(1, 0.95f);
            statusRect.offsetMin = statusRect.offsetMax = Vector2.zero;
            _statusText = statusObj.AddComponent<Text>();
            _statusText.text = "Éditeur de bot - Cliquez sur une pièce pour l'ajouter";
            _statusText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            _statusText.fontSize = 14;
        }

        private static GameObject CreatePanel(Transform parent, string name, Vector2 anchorMin, Vector2 anchorMax)
        {
            var go = new GameObject(name);
            go.transform.SetParent(parent);
            var rect = go.AddComponent<RectTransform>();
            rect.anchorMin = anchorMin;
            rect.anchorMax = anchorMax;
            rect.offsetMin = rect.offsetMax = Vector2.zero;
            go.AddComponent<Image>().color = new Color(0.2f, 0.2f, 0.25f);
            return go;
        }

        private static Button CreateButton(Transform parent, string label, UnityEngine.Events.UnityAction onClick)
        {
            var go = new GameObject("Button_" + label);
            go.transform.SetParent(parent);
            var rect = go.AddComponent<RectTransform>();
            rect.sizeDelta = new Vector2(150, 30);
            var img = go.AddComponent<Image>();
            img.color = new Color(0.3f, 0.5f, 0.8f);
            var btn = go.AddComponent<Button>();
            btn.onClick.AddListener(onClick);
            var textObj = new GameObject("Text");
            textObj.transform.SetParent(go.transform);
            var textRect = textObj.AddComponent<RectTransform>();
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.offsetMin = textRect.offsetMax = Vector2.zero;
            var text = textObj.AddComponent<Text>();
            text.text = label;
            text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            text.fontSize = 12;
            text.alignment = TextAnchor.MiddleCenter;
            return btn;
        }

        private void SetupUI()
        {
            PopulatePieceList();
        }

        private void PopulatePieceList()
        {
            foreach (var btn in _pieceButtons)
                Destroy(btn);
            _pieceButtons.Clear();

            var loader = DataLoader.Instance;
            if (loader?.Pieces == null) return;

            int i = 0;
            foreach (var kv in loader.Pieces)
            {
                var pieceId = kv.Key;
                var def = kv.Value;
                var btn = CreateButton(_pieceListContainer, $"{def.name} ({pieceId})", () => OnPieceSelected(pieceId));
                btn.transform.SetParent(_pieceListContainer);
                var rect = btn.GetComponent<RectTransform>();
                rect.anchorMin = new Vector2(0, 1);
                rect.anchorMax = new Vector2(1, 1);
                rect.pivot = new Vector2(0.5f, 1);
                rect.anchoredPosition = new Vector2(0, -i * 35);
                rect.sizeDelta = new Vector2(-10, 30);
                _pieceButtons.Add(btn.gameObject);
                i++;
            }
        }

        private void OnPieceSelected(string pieceId)
        {
            var pos = new Vector2(Random.Range(0, GridSize) * CellSize, -Random.Range(0, GridSize) * CellSize);
            var piece = _builder.AddPiece(pieceId, pos);
            if (piece != null)
            {
                CreatePieceDisplay(piece);
                SetStatus($"Ajouté: {piece.Definition.name}");
            }
        }

        private void CreatePieceDisplay(PieceInstance piece)
        {
            var go = new GameObject($"Piece_{piece.Definition.id}");
            go.transform.SetParent(_buildAreaContainer);
            var rect = go.AddComponent<RectTransform>();
            rect.anchorMin = new Vector2(0, 1);
            rect.anchorMax = new Vector2(0, 1);
            rect.pivot = new Vector2(0.5f, 0.5f);
            rect.anchoredPosition = _builder.PiecePositions[piece];
            rect.sizeDelta = new Vector2(50, 50);
            var img = go.AddComponent<Image>();
            img.color = new Color(0.4f, 0.6f, 0.9f);
            var btn = go.AddComponent<Button>();
            btn.onClick.AddListener(() => OnPieceClicked(piece));
            var textObj = new GameObject("Label");
            textObj.transform.SetParent(go.transform);
            var textRect = textObj.AddComponent<RectTransform>();
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.offsetMin = textRect.offsetMax = Vector2.zero;
            var text = textObj.AddComponent<Text>();
            text.text = piece.Definition.name.Length > 6 ? piece.Definition.name.Substring(0, 6) : piece.Definition.name;
            text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            text.fontSize = 10;
            text.alignment = TextAnchor.MiddleCenter;
            _pieceDisplay[piece] = go;
        }

        private void OnWiringToggle()
        {
            _wiringMode = !_wiringMode;
            _wiringSource = null;
            SetStatus(_wiringMode ? "Mode câblage: clic sur pièce source puis pièce cible" : "Mode câblage désactivé");
        }

        private void OnPieceClicked(PieceInstance piece)
        {
            if (_wiringMode)
            {
                if (_wiringSource == null)
                {
                    _wiringSource = piece;
                    SetStatus($"Source: {piece.Definition.name}. Cliquez sur la pièce cible.");
                }
                else if (_wiringSource != piece)
                {
                    bool connected = false;
                    foreach (var outPort in _wiringSource.OutputPorts)
                    {
                        foreach (var inPort in piece.InputPorts)
                        {
                            if (_builder.ConnectPorts(outPort, inPort))
                            {
                                connected = true;
                                break;
                            }
                        }
                        if (connected) break;
                    }
                    SetStatus(connected ? "Connexion établie." : "Échec (types incompatibles ou déjà connecté).");
                    _wiringSource = null;
                }
            }
            else
            {
                SetStatus($"Pièce: {piece.Definition.name}");
            }
        }

        private void OnValidateClicked()
        {
            var bot = _builder.BuildBot();
            if (bot.Core == null)
            {
                SetStatus("Erreur: le bot doit avoir une unité centrale.");
                return;
            }
            bot.Validate();
            var result = TestEngine.ValidateBot(bot);
            SetStatus(bot.IsValidated ? "Bot validé !" : $"Échec: {result.ErrorMessage}");
        }

        private void OnTestCombatClicked()
        {
            var bot = _builder.BuildBot();
            if (bot.Core == null)
            {
                SetStatus("Erreur: le bot doit avoir une unité centrale.");
                return;
            }
            var result = CombatTestRunner.RunCombat(bot);
            var msg = result.IsValid
                ? $"Combat: Victoire en {result.TicksExecuted} ticks !"
                : $"Combat: Défaite après {result.TicksExecuted} ticks. Dégâts: {result.DamageDealt}";
            SetStatus(msg);
        }

        private void SetStatus(string msg)
        {
            if (_statusText != null) _statusText.text = msg;
            Debug.Log($"[BotEditor] {msg}");
        }
    }
}
