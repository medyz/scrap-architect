using UnityEngine;
using UnityEditor;

namespace ScrapArchitect.UI
{
    /// <summary>
    /// Простой скрипт для создания UI префабов без сложных типов
    /// </summary>
    public class SimplePrefabCreator : MonoBehaviour
    {
        [MenuItem("Scrap Architect/Simple Create WorldMap Panel")]
        public static void CreateWorldMapPanel()
        {
            CreateBasicPanel("WorldMapPanel", "World Map", new Color(0.1f, 0.1f, 0.3f, 0.95f));
        }
        
        [MenuItem("Scrap Architect/Simple Create Defeat Panel")]
        public static void CreateDefeatPanel()
        {
            CreateBasicPanel("DefeatPanel", "Defeat", new Color(0.3f, 0.1f, 0.1f, 0.95f));
        }
        
        [MenuItem("Scrap Architect/Simple Create Settings Panel")]
        public static void CreateSettingsPanel()
        {
            CreateBasicPanel("SettingsPanel", "Settings", new Color(0.1f, 0.1f, 0.1f, 0.95f));
        }
        
        [MenuItem("Scrap Architect/Simple Create Pause Panel")]
        public static void CreatePausePanel()
        {
            CreateBasicPanel("PausePanel", "Paused", new Color(0.1f, 0.1f, 0.1f, 0.8f));
        }
        
        [MenuItem("Scrap Architect/Simple Create Loading Panel")]
        public static void CreateLoadingPanel()
        {
            CreateBasicPanel("LoadingPanel", "Loading...", new Color(0.1f, 0.1f, 0.1f, 0.95f));
        }
        
        [MenuItem("Scrap Architect/Simple Create All Missing Panels")]
        public static void CreateAllMissingPanels()
        {
            Debug.Log("Creating all missing UI panels...");
            
            CreateWorldMapPanel();
            CreateDefeatPanel();
            CreateSettingsPanel();
            CreatePausePanel();
            CreateLoadingPanel();
            
            Debug.Log("All missing UI panels created!");
        }
        
        private static void CreateBasicPanel(string panelName, string titleText, Color backgroundColor)
        {
            // Создаем GameObject
            GameObject panel = new GameObject(panelName);
            
            // Добавляем компоненты
            RectTransform rectTransform = panel.AddComponent<RectTransform>();
            panel.AddComponent<CanvasRenderer>();
            
            // Настраиваем RectTransform
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.anchoredPosition = Vector2.zero;
            rectTransform.sizeDelta = Vector2.zero;
            
            // Добавляем фон
            UnityEngine.UI.Image image = panel.AddComponent<UnityEngine.UI.Image>();
            image.color = backgroundColor;
            
            // Добавляем текст
            GameObject textGO = new GameObject("Title");
            textGO.transform.SetParent(panel.transform);
            
            RectTransform textRect = textGO.AddComponent<RectTransform>();
            textRect.anchorMin = new Vector2(0.1f, 0.8f);
            textRect.anchorMax = new Vector2(0.9f, 0.95f);
            textRect.anchoredPosition = Vector2.zero;
            textRect.sizeDelta = Vector2.zero;
            
            UnityEngine.UI.Text text = textGO.AddComponent<UnityEngine.UI.Text>();
            text.text = titleText;
            text.color = Color.white;
            text.fontSize = 32;
            text.fontStyle = FontStyle.Bold;
            text.alignment = TextAnchor.MiddleCenter;
            
            // Создаем префаб
            string prefabPath = $"Assets/Prefabs/UI/Panels/{panelName}.prefab";
            
            // Убеждаемся, что папка существует
            string directory = System.IO.Path.GetDirectoryName(prefabPath);
            if (!System.IO.Directory.Exists(directory))
            {
                System.IO.Directory.CreateDirectory(directory);
            }
            
            // Создаем префаб
            GameObject prefab = PrefabUtility.SaveAsPrefabAsset(panel, prefabPath);
            
            // Удаляем временный GameObject из сцены
            DestroyImmediate(panel);
            
            Debug.Log($"Created {panelName} prefab: {prefabPath}");
        }
    }
}
