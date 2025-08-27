using UnityEngine;
using UnityEditor;
using System;
using System.IO;

namespace ScrapArchitect.UI
{
    /// <summary>
    /// Скрипт для создания недостающих UI префабов
    /// </summary>
    public class CreateMissingPrefabs : MonoBehaviour
    {
        [MenuItem("Scrap Architect/Create Missing UI Prefabs")]
        public static void CreateMissingUIPrefabs()
        {
            Debug.Log("Creating missing UI prefabs...");
            
            // Создаем недостающие префабы
            CreateWorldMapPanel();
            CreateDefeatPanel();
            CreateSettingsPanel();
            CreatePausePanel();
            CreateLoadingPanel();
            
            Debug.Log("Missing UI prefabs created successfully!");
        }
        
        private static void CreateWorldMapPanel()
        {
            // Создаем GameObject
            GameObject worldMapPanel = new GameObject("WorldMapPanel");
            
            // Добавляем компоненты
            RectTransform rectTransform = worldMapPanel.AddComponent<RectTransform>();
            CanvasRenderer canvasRenderer = worldMapPanel.AddComponent<CanvasRenderer>();
            WorldMapUI worldMapUI = worldMapPanel.AddComponent<WorldMapUI>();
            
            // Настраиваем RectTransform
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.anchoredPosition = Vector2.zero;
            rectTransform.sizeDelta = Vector2.zero;
            
            // Создаем префаб
            string prefabPath = "Assets/Prefabs/UI/Panels/WorldMapPanel.prefab";
            CreatePrefab(worldMapPanel, prefabPath);
        }
        
        private static void CreateDefeatPanel()
        {
            GameObject defeatPanel = new GameObject("DefeatPanel");
            
            RectTransform rectTransform = defeatPanel.AddComponent<RectTransform>();
            CanvasRenderer canvasRenderer = defeatPanel.AddComponent<CanvasRenderer>();
            DefeatScreen defeatScreen = defeatPanel.AddComponent<DefeatScreen>();
            
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.anchoredPosition = Vector2.zero;
            rectTransform.sizeDelta = Vector2.zero;
            
            string prefabPath = "Assets/Prefabs/UI/Panels/DefeatPanel.prefab";
            CreatePrefab(defeatPanel, prefabPath);
        }
        
        private static void CreateSettingsPanel()
        {
            GameObject settingsPanel = new GameObject("SettingsPanel");
            
            RectTransform rectTransform = settingsPanel.AddComponent<RectTransform>();
            CanvasRenderer canvasRenderer = settingsPanel.AddComponent<CanvasRenderer>();
            SettingsUI settingsUI = settingsPanel.AddComponent<SettingsUI>();
            
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.anchoredPosition = Vector2.zero;
            rectTransform.sizeDelta = Vector2.zero;
            
            string prefabPath = "Assets/Prefabs/UI/Panels/SettingsPanel.prefab";
            CreatePrefab(settingsPanel, prefabPath);
        }
        
        private static void CreatePausePanel()
        {
            GameObject pausePanel = new GameObject("PausePanel");
            
            RectTransform rectTransform = pausePanel.AddComponent<RectTransform>();
            CanvasRenderer canvasRenderer = pausePanel.AddComponent<CanvasRenderer>();
            PauseMenu pauseMenu = pausePanel.AddComponent<PauseMenu>();
            
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.anchoredPosition = Vector2.zero;
            rectTransform.sizeDelta = Vector2.zero;
            
            string prefabPath = "Assets/Prefabs/UI/Panels/PausePanel.prefab";
            CreatePrefab(pausePanel, prefabPath);
        }
        
        private static void CreateLoadingPanel()
        {
            GameObject loadingPanel = new GameObject("LoadingPanel");
            
            RectTransform rectTransform = loadingPanel.AddComponent<RectTransform>();
            CanvasRenderer canvasRenderer = loadingPanel.AddComponent<CanvasRenderer>();
            LoadingScreen loadingScreen = loadingPanel.AddComponent<LoadingScreen>();
            
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.anchoredPosition = Vector2.zero;
            rectTransform.sizeDelta = Vector2.zero;
            
            string prefabPath = "Assets/Prefabs/UI/Panels/LoadingPanel.prefab";
            CreatePrefab(loadingPanel, prefabPath);
        }
        
        private static void CreatePrefab(GameObject gameObject, string prefabPath)
        {
            // Убеждаемся, что папка существует
            string directory = Path.GetDirectoryName(prefabPath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            
            // Создаем префаб
            GameObject prefab = PrefabUtility.SaveAsPrefabAsset(gameObject, prefabPath);
            
            // Удаляем временный GameObject
            DestroyImmediate(gameObject);
            
            Debug.Log($"Created prefab: {prefabPath}");
        }
    }
}
