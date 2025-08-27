using UnityEngine;
using UnityEditor;

namespace ScrapArchitect.UI
{
    /// <summary>
    /// Быстрое создание недостающих UI префабов
    /// </summary>
    public class QuickPrefabCreator : MonoBehaviour
    {
        [MenuItem("Scrap Architect/Quick Create Missing UI Prefabs")]
        public static void QuickCreateMissingPrefabs()
        {
            Debug.Log("Quick creating missing UI prefabs...");
            
            // Создаем недостающие префабы
            CreateWorldMapPanel();
            CreateDefeatPanel();
            CreateSettingsPanel();
            CreatePausePanel();
            CreateLoadingPanel();
            
            Debug.Log("Quick creation completed! Check Assets/Prefabs/UI/Panels/ folder.");
        }
        
        private static void CreateWorldMapPanel()
        {
            // Создаем GameObject в сцене
            GameObject worldMapPanel = new GameObject("WorldMapPanel");
            
            // Добавляем компоненты
            RectTransform rectTransform = worldMapPanel.AddComponent<RectTransform>();
            worldMapPanel.AddComponent<CanvasRenderer>();
            worldMapPanel.AddComponent<WorldMapUI>();
            
            // Настраиваем RectTransform
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.anchoredPosition = Vector2.zero;
            rectTransform.sizeDelta = Vector2.zero;
            
            // Добавляем фон
            UnityEngine.UI.Image image = worldMapPanel.AddComponent<UnityEngine.UI.Image>();
            image.color = new Color(0.1f, 0.1f, 0.3f, 0.95f);
            
            // Добавляем текст
            GameObject textGO = new GameObject("Title");
            textGO.transform.SetParent(worldMapPanel.transform);
            
            RectTransform textRect = textGO.AddComponent<RectTransform>();
            textRect.anchorMin = new Vector2(0.1f, 0.8f);
            textRect.anchorMax = new Vector2(0.9f, 0.95f);
            textRect.anchoredPosition = Vector2.zero;
            textRect.sizeDelta = Vector2.zero;
            
            UnityEngine.UI.Text text = textGO.AddComponent<UnityEngine.UI.Text>();
            text.text = "World Map";
            text.color = Color.white;
            text.fontSize = 32;
            text.fontStyle = FontStyle.Bold;
            text.alignment = TextAnchor.MiddleCenter;
            
            // Создаем префаб
            string prefabPath = "Assets/Prefabs/UI/Panels/WorldMapPanel.prefab";
            CreatePrefabAsset(worldMapPanel, prefabPath);
        }
        
        private static void CreateDefeatPanel()
        {
            GameObject defeatPanel = new GameObject("DefeatPanel");
            
            RectTransform rectTransform = defeatPanel.AddComponent<RectTransform>();
            defeatPanel.AddComponent<CanvasRenderer>();
            defeatPanel.AddComponent<DefeatScreen>();
            
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.anchoredPosition = Vector2.zero;
            rectTransform.sizeDelta = Vector2.zero;
            
            UnityEngine.UI.Image image = defeatPanel.AddComponent<UnityEngine.UI.Image>();
            image.color = new Color(0.3f, 0.1f, 0.1f, 0.95f);
            
            GameObject textGO = new GameObject("Title");
            textGO.transform.SetParent(defeatPanel.transform);
            
            RectTransform textRect = textGO.AddComponent<RectTransform>();
            textRect.anchorMin = new Vector2(0.1f, 0.8f);
            textRect.anchorMax = new Vector2(0.9f, 0.95f);
            textRect.anchoredPosition = Vector2.zero;
            textRect.sizeDelta = Vector2.zero;
            
            UnityEngine.UI.Text text = textGO.AddComponent<UnityEngine.UI.Text>();
            text.text = "Defeat";
            text.color = Color.white;
            text.fontSize = 32;
            text.fontStyle = FontStyle.Bold;
            text.alignment = TextAnchor.MiddleCenter;
            
            string prefabPath = "Assets/Prefabs/UI/Panels/DefeatPanel.prefab";
            CreatePrefabAsset(defeatPanel, prefabPath);
        }
        
        private static void CreateSettingsPanel()
        {
            GameObject settingsPanel = new GameObject("SettingsPanel");
            
            RectTransform rectTransform = settingsPanel.AddComponent<RectTransform>();
            settingsPanel.AddComponent<CanvasRenderer>();
            settingsPanel.AddComponent<SettingsUI>();
            
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.anchoredPosition = Vector2.zero;
            rectTransform.sizeDelta = Vector2.zero;
            
            UnityEngine.UI.Image image = settingsPanel.AddComponent<UnityEngine.UI.Image>();
            image.color = new Color(0.1f, 0.1f, 0.1f, 0.95f);
            
            GameObject textGO = new GameObject("Title");
            textGO.transform.SetParent(settingsPanel.transform);
            
            RectTransform textRect = textGO.AddComponent<RectTransform>();
            textRect.anchorMin = new Vector2(0.1f, 0.8f);
            textRect.anchorMax = new Vector2(0.9f, 0.95f);
            textRect.anchoredPosition = Vector2.zero;
            textRect.sizeDelta = Vector2.zero;
            
            UnityEngine.UI.Text text = textGO.AddComponent<UnityEngine.UI.Text>();
            text.text = "Settings";
            text.color = Color.white;
            text.fontSize = 32;
            text.fontStyle = FontStyle.Bold;
            text.alignment = TextAnchor.MiddleCenter;
            
            string prefabPath = "Assets/Prefabs/UI/Panels/SettingsPanel.prefab";
            CreatePrefabAsset(settingsPanel, prefabPath);
        }
        
        private static void CreatePausePanel()
        {
            GameObject pausePanel = new GameObject("PausePanel");
            
            RectTransform rectTransform = pausePanel.AddComponent<RectTransform>();
            pausePanel.AddComponent<CanvasRenderer>();
            pausePanel.AddComponent<PauseMenu>();
            
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.anchoredPosition = Vector2.zero;
            rectTransform.sizeDelta = Vector2.zero;
            
            UnityEngine.UI.Image image = pausePanel.AddComponent<UnityEngine.UI.Image>();
            image.color = new Color(0.1f, 0.1f, 0.1f, 0.8f);
            
            GameObject textGO = new GameObject("Title");
            textGO.transform.SetParent(pausePanel.transform);
            
            RectTransform textRect = textGO.AddComponent<RectTransform>();
            textRect.anchorMin = new Vector2(0.1f, 0.8f);
            textRect.anchorMax = new Vector2(0.9f, 0.95f);
            textRect.anchoredPosition = Vector2.zero;
            textRect.sizeDelta = Vector2.zero;
            
            UnityEngine.UI.Text text = textGO.AddComponent<UnityEngine.UI.Text>();
            text.text = "Paused";
            text.color = Color.white;
            text.fontSize = 32;
            text.fontStyle = FontStyle.Bold;
            text.alignment = TextAnchor.MiddleCenter;
            
            string prefabPath = "Assets/Prefabs/UI/Panels/PausePanel.prefab";
            CreatePrefabAsset(pausePanel, prefabPath);
        }
        
        private static void CreateLoadingPanel()
        {
            GameObject loadingPanel = new GameObject("LoadingPanel");
            
            RectTransform rectTransform = loadingPanel.AddComponent<RectTransform>();
            loadingPanel.AddComponent<CanvasRenderer>();
            loadingPanel.AddComponent<LoadingScreen>();
            
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.anchoredPosition = Vector2.zero;
            rectTransform.sizeDelta = Vector2.zero;
            
            UnityEngine.UI.Image image = loadingPanel.AddComponent<UnityEngine.UI.Image>();
            image.color = new Color(0.1f, 0.1f, 0.1f, 0.95f);
            
            GameObject textGO = new GameObject("Title");
            textGO.transform.SetParent(loadingPanel.transform);
            
            RectTransform textRect = textGO.AddComponent<RectTransform>();
            textRect.anchorMin = new Vector2(0.1f, 0.8f);
            textRect.anchorMax = new Vector2(0.9f, 0.95f);
            textRect.anchoredPosition = Vector2.zero;
            textRect.sizeDelta = Vector2.zero;
            
            UnityEngine.UI.Text text = textGO.AddComponent<UnityEngine.UI.Text>();
            text.text = "Loading...";
            text.color = Color.white;
            text.fontSize = 32;
            text.fontStyle = FontStyle.Bold;
            text.alignment = TextAnchor.MiddleCenter;
            
            string prefabPath = "Assets/Prefabs/UI/Panels/LoadingPanel.prefab";
            CreatePrefabAsset(loadingPanel, prefabPath);
        }
        
        private static void CreatePrefabAsset(GameObject gameObject, string prefabPath)
        {
            // Убеждаемся, что папка существует
            string directory = System.IO.Path.GetDirectoryName(prefabPath);
            if (!System.IO.Directory.Exists(directory))
            {
                System.IO.Directory.CreateDirectory(directory);
            }
            
            // Создаем префаб
            GameObject prefab = PrefabUtility.SaveAsPrefabAsset(gameObject, prefabPath);
            
            // Удаляем временный GameObject из сцены
            DestroyImmediate(gameObject);
            
            Debug.Log($"Created prefab: {prefabPath}");
        }
    }
}
