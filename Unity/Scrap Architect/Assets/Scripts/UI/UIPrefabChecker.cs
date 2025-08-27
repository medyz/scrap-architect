using UnityEngine;
using System;
using System.Collections.Generic;

namespace ScrapArchitect.UI
{
    /// <summary>
    /// Скрипт для проверки и создания недостающих UI префабов
    /// </summary>
    public class UIPrefabChecker : MonoBehaviour
    {
        [Header("Missing Prefabs Status")]
        [SerializeField] private bool worldMapPanelMissing = true;
        [SerializeField] private bool defeatPanelMissing = true;
        [SerializeField] private bool settingsPanelMissing = true;
        [SerializeField] private bool pausePanelMissing = true;
        [SerializeField] private bool loadingPanelMissing = true;
        
        [Header("Instructions")]
        [TextArea(3, 10)]
        [SerializeField] private string instructions = 
            "Для создания недостающих префабов:\n" +
            "1. Создайте пустые GameObject в сцене\n" +
            "2. Добавьте соответствующие UI скрипты\n" +
            "3. Настройте RectTransform (anchor: stretch-stretch)\n" +
            "4. Создайте префабы в папке Assets/Prefabs/UI/Panels/\n" +
            "5. Назначьте префабы в UIManager";
        
        private void Start()
        {
            CheckMissingPrefabs();
        }
        
        [ContextMenu("Check Missing Prefabs")]
        public void CheckMissingPrefabs()
        {
            Debug.Log("=== UI Prefab Checker ===");
            
            // Проверяем существующие префабы
            CheckPrefab("MainMenuPanel");
            CheckPrefab("ContractSelectionPanel");
            CheckPrefab("WorldMapPanel");
            CheckPrefab("GameplayPanel");
            CheckPrefab("VictoryPanel");
            CheckPrefab("DefeatPanel");
            CheckPrefab("SettingsPanel");
            CheckPrefab("PausePanel");
            CheckPrefab("LoadingPanel");
            
            Debug.Log("=== End UI Prefab Checker ===");
        }
        
        private void CheckPrefab(string prefabName)
        {
            string prefabPath = $"Assets/Prefabs/UI/Panels/{prefabName}.prefab";
            GameObject prefab = Resources.Load<GameObject>(prefabPath);
            
            if (prefab == null)
            {
                Debug.LogWarning($"Missing prefab: {prefabPath}");
            }
            else
            {
                Debug.Log($"Found prefab: {prefabPath}");
            }
        }
        
        [ContextMenu("Create Missing Prefabs")]
        public void CreateMissingPrefabs()
        {
            Debug.Log("Creating missing UI prefabs...");
            
            // Создаем простые префабы
            CreateSimplePrefab("WorldMapPanel", typeof(WorldMapUI));
            CreateSimplePrefab("DefeatPanel", typeof(DefeatScreen));
            CreateSimplePrefab("SettingsPanel", typeof(SettingsUI));
            CreateSimplePrefab("PausePanel", typeof(PauseMenu));
            CreateSimplePrefab("LoadingPanel", typeof(LoadingScreen));
            
            Debug.Log("Missing UI prefabs created!");
        }
        
        private void CreateSimplePrefab(string prefabName, System.Type componentType)
        {
            // Создаем GameObject
            GameObject go = new GameObject(prefabName);
            
            // Добавляем компоненты
            RectTransform rectTransform = go.AddComponent<RectTransform>();
            go.AddComponent<CanvasRenderer>();
            go.AddComponent(componentType);
            
            // Настраиваем RectTransform
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.anchoredPosition = Vector2.zero;
            rectTransform.sizeDelta = Vector2.zero;
            
            // Добавляем Image компонент для фона
            UnityEngine.UI.Image image = go.AddComponent<UnityEngine.UI.Image>();
            image.color = new Color(0.2f, 0.2f, 0.2f, 0.9f);
            
            // Добавляем текст
            GameObject textGO = new GameObject("Text");
            textGO.transform.SetParent(go.transform);
            
            RectTransform textRect = textGO.AddComponent<RectTransform>();
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.anchoredPosition = Vector2.zero;
            textRect.sizeDelta = Vector2.zero;
            
            UnityEngine.UI.Text text = textGO.AddComponent<UnityEngine.UI.Text>();
            text.text = prefabName;
            text.color = Color.white;
            text.fontSize = 24;
            text.alignment = TextAnchor.MiddleCenter;
            
            Debug.Log($"Created {prefabName} prefab structure");
        }
    }
}
