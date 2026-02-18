using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace OverclockedClash.Editor
{
    public static class SceneSetupMenu
    {
        [MenuItem("Overclocked Clash/Créer scène BotEditor")]
        public static void CreateBotEditorScene()
        {
            var scene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects);
            var go = new GameObject("BotEditor");
            go.AddComponent<OverclockedClash.UI.BotEditorController>();
            EditorSceneManager.SaveScene(scene, "Assets/Scenes/BotEditor.unity");
            Debug.Log("Scène BotEditor créée. Pensez à l'ajouter dans File > Build Settings.");
        }

        [MenuItem("Overclocked Clash/Créer scène CombatTest")]
        public static void CreateCombatTestScene()
        {
            var scene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects);
            var go = new GameObject("CombatTestRunner");
            go.AddComponent<OverclockedClash.UI.CombatTestRunner>();
            EditorSceneManager.SaveScene(scene, "Assets/Scenes/CombatTest.unity");
            Debug.Log("Scène CombatTest créée.");
        }
    }
}
