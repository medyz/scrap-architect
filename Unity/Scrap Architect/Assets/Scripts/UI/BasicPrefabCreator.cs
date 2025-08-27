using UnityEngine;
using UnityEditor;
using System.IO;

namespace ScrapArchitect.UI
{
    /// <summary>
    /// Максимально простой скрипт для создания UI префабов
    /// </summary>
    public class BasicPrefabCreator : MonoBehaviour
    {
        [MenuItem("Scrap Architect/Basic Create All Panels")]
        public static void CreateAllPanels()
        {
            Debug.Log("=== Basic Prefab Creator ===");
            
            CreatePanel("WorldMapPanel", "World Map", Color.blue);
            CreatePanel("DefeatPanel", "Defeat", Color.red);
            CreatePanel("SettingsPanel", "Settings", Color.gray);
            CreatePanel("PausePanel", "Paused", Color.black);
            CreatePanel("LoadingPanel", "Loading...", Color.black);
            
            Debug.Log("=== All panels created! ===");
        }
        
        private static void CreatePanel(string name, string title, Color color)
        {
            try
            {
                // Создаем GameObject
                GameObject panel = new GameObject(name);
                
                // Добавляем RectTransform
                RectTransform rect = panel.AddComponent<RectTransform>();
                rect.anchorMin = Vector2.zero;
                rect.anchorMax = Vector2.one;
                rect.anchoredPosition = Vector2.zero;
                rect.sizeDelta = Vector2.zero;
                
                // Добавляем CanvasRenderer
                panel.AddComponent<CanvasRenderer>();
                
                // Добавляем Image для фона
                UnityEngine.UI.Image image = panel.AddComponent<UnityEngine.UI.Image>();
                image.color = new Color(color.r, color.g, color.b, 0.9f);
                
                // Добавляем текст
                GameObject textObj = new GameObject("Text");
                textObj.transform.SetParent(panel.transform);
                
                RectTransform textRect = textObj.AddComponent<RectTransform>();
                textRect.anchorMin = Vector2.zero;
                textRect.anchorMax = Vector2.one;
                textRect.anchoredPosition = Vector2.zero;
                textRect.sizeDelta = Vector2.zero;
                
                UnityEngine.UI.Text text = textObj.AddComponent<UnityEngine.UI.Text>();
                text.text = title;
                text.color = Color.white;
                text.fontSize = 24;
                text.alignment = TextAnchor.MiddleCenter;
                
                // Создаем папку если не существует
                string folderPath = "Assets/Prefabs/UI/Panels";
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                
                // Создаем префаб
                string prefabPath = $"{folderPath}/{name}.prefab";
                GameObject prefab = PrefabUtility.SaveAsPrefabAsset(panel, prefabPath);
                
                // Удаляем временный объект
                DestroyImmediate(panel);
                
                Debug.Log($"✓ Created: {prefabPath}");
            }
            catch (System.Exception e)
            {
                Debug.LogError($"✗ Error creating {name}: {e.Message}");
            }
        }
    }
}
