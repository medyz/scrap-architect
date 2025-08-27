using UnityEngine;
using ScrapArchitect.UI;

/// <summary>
/// Тестовый скрипт для проверки MainMenuPanel
/// </summary>
public class TestMainMenuUI : MonoBehaviour
{
    [Header("Test Settings")]
    public MainMenuUI mainMenuPanel;
    public bool autoShow = true;
    public bool testAnimations = true;
    
    void Start()
    {
        if (autoShow && mainMenuPanel != null)
        {
            // Инициализируем и показываем
            mainMenuPanel.Initialize(null); // Без UIManager для тестирования
            mainMenuPanel.Show();
        }
    }
    
    [ContextMenu("Show Main Menu")]
    public void ShowMainMenu()
    {
        if (mainMenuPanel != null)
        {
            mainMenuPanel.gameObject.SetActive(true);
            mainMenuPanel.Show();
        }
    }
    
    [ContextMenu("Hide Main Menu")]
    public void HideMainMenu()
    {
        if (mainMenuPanel != null)
        {
            mainMenuPanel.Hide();
        }
    }
    
    [ContextMenu("Toggle Main Menu")]
    public void ToggleMainMenu()
    {
        if (mainMenuPanel != null)
        {
            if (mainMenuPanel.IsVisible())
            {
                HideMainMenu();
            }
            else
            {
                ShowMainMenu();
            }
        }
    }
    
    void Update()
    {
        // Горячие клавиши для тестирования
        if (Input.GetKeyDown(KeyCode.F1))
        {
            ShowMainMenu();
        }
        else if (Input.GetKeyDown(KeyCode.F2))
        {
            HideMainMenu();
        }
        else if (Input.GetKeyDown(KeyCode.F3))
        {
            ToggleMainMenu();
        }
    }
}
