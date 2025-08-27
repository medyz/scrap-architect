using UnityEngine;
using ScrapArchitect.UI;

/// <summary>
/// Вспомогательный скрипт для тестирования UI
/// </summary>
public class UITestHelper : MonoBehaviour
{
    [Header("Test Settings")]
    public bool autoTestOnStart = true;
    public bool showDebugInfo = true;
    
    void Start()
    {
        if (autoTestOnStart)
        {
            Invoke(nameof(TestUI), 0.5f);
        }
    }
    
    [ContextMenu("Test UI")]
    public void TestUI()
    {
        Debug.Log("=== UI Test Started ===");
        
        // Проверяем UIManager
        if (UIManager.Instance != null)
        {
            Debug.Log("✓ UIManager.Instance found");
            
            // Проверяем MainMenuPanel
            if (UIManager.Instance.mainMenuPanel != null)
            {
                Debug.Log("✓ MainMenuPanel assigned");
                Debug.Log($"MainMenuPanel GameObject active: {UIManager.Instance.mainMenuPanel.gameObject.activeInHierarchy}");
                
                // Пытаемся показать главное меню
                UIManager.Instance.ShowMainMenu();
            }
            else
            {
                Debug.LogError("✗ MainMenuPanel is null!");
                
                // Ищем MainMenuPanel в сцене
                MainMenuUI[] mainMenus = FindObjectsOfType<MainMenuUI>();
                Debug.Log($"Found {mainMenus.Length} MainMenuUI objects in scene");
                
                foreach (var menu in mainMenus)
                {
                    Debug.Log($"MainMenuUI: {menu.name}, active: {menu.gameObject.activeInHierarchy}");
                }
            }
        }
        else
        {
            Debug.LogError("✗ UIManager.Instance is null!");
            
            // Ищем UIManager в сцене
            UIManager[] managers = FindObjectsOfType<UIManager>();
            Debug.Log($"Found {managers.Length} UIManager objects in scene");
        }
        
        Debug.Log("=== UI Test Completed ===");
    }
    
    void Update()
    {
        // Горячие клавиши для тестирования
        if (Input.GetKeyDown(KeyCode.F1))
        {
            TestUI();
        }
        else if (Input.GetKeyDown(KeyCode.F2))
        {
            if (UIManager.Instance != null && UIManager.Instance.mainMenuPanel != null)
            {
                UIManager.Instance.ShowMainMenu();
            }
        }
        else if (Input.GetKeyDown(KeyCode.F3))
        {
            // Показать все UI объекты в сцене
            ShowAllUIObjects();
        }
    }
    
    [ContextMenu("Show All UI Objects")]
    public void ShowAllUIObjects()
    {
        Debug.Log("=== All UI Objects in Scene ===");
        
        // Ищем все Canvas
        Canvas[] canvases = FindObjectsOfType<Canvas>();
        Debug.Log($"Found {canvases.Length} Canvas objects:");
        
        foreach (var canvas in canvases)
        {
            Debug.Log($"Canvas: {canvas.name}, active: {canvas.gameObject.activeInHierarchy}, renderMode: {canvas.renderMode}");
        }
        
        // Ищем все MainMenuUI
        MainMenuUI[] mainMenus = FindObjectsOfType<MainMenuUI>();
        Debug.Log($"Found {mainMenus.Length} MainMenuUI objects:");
        
        foreach (var menu in mainMenus)
        {
            Debug.Log($"MainMenuUI: {menu.name}, active: {menu.gameObject.activeInHierarchy}, parent: {menu.transform.parent?.name ?? "none"}");
        }
        
        // Ищем все UIManager
        UIManager[] managers = FindObjectsOfType<UIManager>();
        Debug.Log($"Found {managers.Length} UIManager objects:");
        
        foreach (var manager in managers)
        {
            Debug.Log($"UIManager: {manager.name}, active: {manager.gameObject.activeInHierarchy}");
        }
        
        Debug.Log("=== End UI Objects List ===");
    }
}
