using UnityEngine;
using UnityEditor;
using System;

namespace ScrapArchitect.UI
{
    /// <summary>
    /// Простой тестовый скрипт для создания префабов
    /// </summary>
    public class TestPrefabCreation : MonoBehaviour
    {
        [MenuItem("Scrap Architect/Test Create Single Prefab")]
        public static void TestCreateSinglePrefab()
        {
            Debug.Log("Testing single prefab creation...");
            
            // Создаем простой префаб для тестирования
            GameObject testPanel = new GameObject("TestPanel");
            
            // Добавляем компоненты
            RectTransform rectTransform = testPanel.AddComponent<RectTransform>();
            testPanel.AddComponent<CanvasRenderer>();
            
            // Настраиваем RectTransform
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.anchoredPosition = Vector2.zero;
            rectTransform.sizeDelta = Vector2.zero;
            
            // Добавляем фон
            UnityEngine.UI.Image image = testPanel.AddComponent<UnityEngine.UI.Image>();
            image.color = new Color(0.2f, 0.2f, 0.2f, 0.9f);
            
            // Добавляем текст
            GameObject textGO = new GameObject("Text");
            textGO.transform.SetParent(testPanel.transform);
            
            RectTransform textRect = textGO.AddComponent<RectTransform>();
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.anchoredPosition = Vector2.zero;
            textRect.sizeDelta = Vector2.zero;
            
            UnityEngine.UI.Text text = textGO.AddComponent<UnityEngine.UI.Text>();
            text.text = "Test Panel";
            text.color = Color.white;
            text.fontSize = 24;
            text.alignment = TextAnchor.MiddleCenter;
            
            // Создаем префаб
            string prefabPath = "Assets/Prefabs/UI/Panels/TestPanel.prefab";
            
            // Убеждаемся, что папка существует
            string directory = System.IO.Path.GetDirectoryName(prefabPath);
            if (!System.IO.Directory.Exists(directory))
            {
                System.IO.Directory.CreateDirectory(directory);
            }
            
            // Создаем префаб
            GameObject prefab = PrefabUtility.SaveAsPrefabAsset(testPanel, prefabPath);
            
            // Удаляем временный GameObject из сцены
            DestroyImmediate(testPanel);
            
            Debug.Log($"Test prefab created: {prefabPath}");
        }
        
        [MenuItem("Scrap Architect/Test Check All Scripts")]
        public static void TestCheckAllScripts()
        {
            Debug.Log("Testing all UI scripts...");
            
            // Проверяем, что все необходимые скрипты существуют
            Type[] requiredTypes = {
                typeof(WorldMapUI),
                typeof(DefeatScreen),
                typeof(SettingsUI),
                typeof(PauseMenu),
                typeof(LoadingScreen)
            };
            
            foreach (Type type in requiredTypes)
            {
                if (type != null)
                {
                    Debug.Log($"✓ Found script: {type.Name}");
                }
                else
                {
                    Debug.LogError($"✗ Missing script: {type.Name}");
                }
            }
            
            Debug.Log("Script check completed!");
        }
    }
}
