using UnityEngine;
using UnityEngine.UI;
using TMPro;
using ScrapArchitect.Core;

namespace ScrapArchitect.UI
{
    /// <summary>
    /// Основной менеджер UI - управляет всеми интерфейсами в игре
    /// </summary>
    public class UIManager : MonoBehaviour
    {
        [Header("UI Panels")]
        public GameObject mainMenuPanel;
        public GameObject buildModePanel;
        public GameObject testModePanel;
        public GameObject pausePanel;
        public GameObject settingsPanel;
        public GameObject infoPanel;
        
        [Header("Build Mode UI")]
        public BuildModeUI buildModeUI;
        
        [Header("Test Mode UI")]
        public TestModeUI testModeUI;
        
        [Header("Common UI")]
        public TextMeshProUGUI gameTimeText;
        public TextMeshProUGUI playerLevelText;
        public TextMeshProUGUI moneyText;
        public Button pauseButton;
        public Button settingsButton;
        
        [Header("Info Panel")]
        public GameObject partInfoPanel;
        public TextMeshProUGUI partNameText;
        public TextMeshProUGUI partInfoText;
        public Button closeInfoButton;
        
        // Singleton pattern
        public static UIManager Instance { get; private set; }
        
        // Events
        public System.Action OnPauseButtonClicked;
        public System.Action OnSettingsButtonClicked;
        public System.Action OnResumeButtonClicked;
        
        private void Awake()
        {
            // Singleton pattern
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                InitializeUI();
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
        private void Start()
        {
            // Подписываемся на события GameManager
            if (Core.GameManager.Instance != null)
            {
                Core.GameManager.Instance.OnGameStateChanged += OnGameStateChanged;
                Core.GameManager.Instance.OnGameModeChanged += OnGameModeChanged;
                Core.GameManager.Instance.OnPauseChanged += OnPauseChanged;
            }
            
            // Настраиваем кнопки
            SetupButtons();
            
            // Показываем главное меню
            ShowMainMenu();
        }
        
        private void OnDestroy()
        {
            // Отписываемся от событий
            if (Core.GameManager.Instance != null)
            {
                Core.GameManager.Instance.OnGameStateChanged -= OnGameStateChanged;
                Core.GameManager.Instance.OnGameModeChanged -= OnGameModeChanged;
                Core.GameManager.Instance.OnPauseChanged -= OnPauseChanged;
            }
        }
        
        /// <summary>
        /// Инициализация UI
        /// </summary>
        private void InitializeUI()
        {
            // Скрываем все панели
            HideAllPanels();
            
            Debug.Log("UIManager initialized");
        }
        
        /// <summary>
        /// Настройка кнопок
        /// </summary>
        private void SetupButtons()
        {
            if (pauseButton != null)
            {
                pauseButton.onClick.AddListener(OnPauseButtonClick);
            }
            
            if (settingsButton != null)
            {
                settingsButton.onClick.AddListener(OnSettingsButtonClick);
            }
            
            if (closeInfoButton != null)
            {
                closeInfoButton.onClick.AddListener(HidePartInfo);
            }
        }
        
        /// <summary>
        /// Обработка изменения состояния игры
        /// </summary>
        private void OnGameStateChanged(GameState newState)
        {
            switch (newState)
            {
                case GameState.MainMenu:
                    ShowMainMenu();
                    break;
                    
                case GameState.Building:
                    ShowBuildMode();
                    break;
                    
                case GameState.Testing:
                    ShowTestMode();
                    break;
                    
                case GameState.Playing:
                    ShowGameUI();
                    break;
                    
                case GameState.Paused:
                    ShowPauseMenu();
                    break;
            }
        }
        
        /// <summary>
        /// Обработка изменения режима игры
        /// </summary>
        private void OnGameModeChanged(GameMode newMode)
        {
            switch (newMode)
            {
                case GameMode.Build:
                    if (buildModeUI != null)
                    {
                        buildModeUI.Show();
                    }
                    break;
                    
                case GameMode.Test:
                    if (testModeUI != null)
                    {
                        testModeUI.Show();
                    }
                    break;
            }
        }
        
        /// <summary>
        /// Обработка изменения паузы
        /// </summary>
        private void OnPauseChanged(bool isPaused)
        {
            if (isPaused)
            {
                ShowPauseMenu();
            }
            else
            {
                HidePauseMenu();
            }
        }
        
        /// <summary>
        /// Скрытие всех панелей
        /// </summary>
        private void HideAllPanels()
        {
            if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
            if (buildModePanel != null) buildModePanel.SetActive(false);
            if (testModePanel != null) testModePanel.SetActive(false);
            if (pausePanel != null) pausePanel.SetActive(false);
            if (settingsPanel != null) settingsPanel.SetActive(false);
            if (infoPanel != null) infoPanel.SetActive(false);
            
            if (buildModeUI != null) buildModeUI.Hide();
            if (testModeUI != null) testModeUI.Hide();
        }
        
        /// <summary>
        /// Показать главное меню
        /// </summary>
        public void ShowMainMenu()
        {
            HideAllPanels();
            if (mainMenuPanel != null)
            {
                mainMenuPanel.SetActive(true);
            }
        }
        
        /// <summary>
        /// Показать режим строительства
        /// </summary>
        public void ShowBuildMode()
        {
            HideAllPanels();
            if (buildModePanel != null)
            {
                buildModePanel.SetActive(true);
            }
            if (buildModeUI != null)
            {
                buildModeUI.Show();
            }
        }
        
        /// <summary>
        /// Показать режим тестирования
        /// </summary>
        public void ShowTestMode()
        {
            HideAllPanels();
            if (testModePanel != null)
            {
                testModePanel.SetActive(true);
            }
            if (testModeUI != null)
            {
                testModeUI.Show();
            }
        }
        
        /// <summary>
        /// Показать игровой UI
        /// </summary>
        public void ShowGameUI()
        {
            // Показываем общий UI для игры
            if (gameTimeText != null) gameTimeText.gameObject.SetActive(true);
            if (playerLevelText != null) playerLevelText.gameObject.SetActive(true);
            if (moneyText != null) moneyText.gameObject.SetActive(true);
            if (pauseButton != null) pauseButton.gameObject.SetActive(true);
        }
        
        /// <summary>
        /// Показать меню паузы
        /// </summary>
        public void ShowPauseMenu()
        {
            if (pausePanel != null)
            {
                pausePanel.SetActive(true);
            }
        }
        
        /// <summary>
        /// Скрыть меню паузы
        /// </summary>
        public void HidePauseMenu()
        {
            if (pausePanel != null)
            {
                pausePanel.SetActive(false);
            }
        }
        
        /// <summary>
        /// Показать настройки
        /// </summary>
        public void ShowSettings()
        {
            if (settingsPanel != null)
            {
                settingsPanel.SetActive(true);
            }
        }
        
        /// <summary>
        /// Скрыть настройки
        /// </summary>
        public void HideSettings()
        {
            if (settingsPanel != null)
            {
                settingsPanel.SetActive(false);
            }
        }
        
        /// <summary>
        /// Показать информацию о детали
        /// </summary>
        public void ShowPartInfo(string partName, string partInfo)
        {
            if (partInfoPanel != null)
            {
                partInfoPanel.SetActive(true);
                
                if (partNameText != null)
                {
                    partNameText.text = partName;
                }
                
                if (partInfoText != null)
                {
                    partInfoText.text = partInfo;
                }
            }
        }
        
        /// <summary>
        /// Скрыть информацию о детали
        /// </summary>
        public void HidePartInfo()
        {
            if (partInfoPanel != null)
            {
                partInfoPanel.SetActive(false);
            }
        }
        
        /// <summary>
        /// Обновить игровое время
        /// </summary>
        public void UpdateGameTime(float time)
        {
            if (gameTimeText != null)
            {
                int minutes = Mathf.FloorToInt(time / 60f);
                int seconds = Mathf.FloorToInt(time % 60f);
                gameTimeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
            }
        }
        
        /// <summary>
        /// Обновить уровень игрока
        /// </summary>
        public void UpdatePlayerLevel(int level)
        {
            if (playerLevelText != null)
            {
                playerLevelText.text = $"Level: {level}";
            }
        }
        
        /// <summary>
        /// Обновить деньги игрока
        /// </summary>
        public void UpdateMoney(float money)
        {
            if (moneyText != null)
            {
                moneyText.text = $"Money: ${money:F0}";
            }
        }
        
        /// <summary>
        /// Обработчик нажатия кнопки паузы
        /// </summary>
        private void OnPauseButtonClick()
        {
            OnPauseButtonClicked?.Invoke();
            
            if (Core.GameManager.Instance != null)
            {
                Core.GameManager.Instance.TogglePause();
            }
        }
        
        /// <summary>
        /// Обработчик нажатия кнопки настроек
        /// </summary>
        private void OnSettingsButtonClick()
        {
            OnSettingsButtonClicked?.Invoke();
            ShowSettings();
        }
        
        /// <summary>
        /// Обработчик нажатия кнопки возобновления
        /// </summary>
        public void OnResumeButtonClick()
        {
            OnResumeButtonClicked?.Invoke();
            
            if (Core.GameManager.Instance != null)
            {
                Core.GameManager.Instance.TogglePause();
            }
        }
        
        /// <summary>
        /// Обработчик нажатия кнопки выхода в главное меню
        /// </summary>
        public void OnMainMenuButtonClick()
        {
            if (Core.GameManager.Instance != null)
            {
                Core.GameManager.Instance.ReturnToMainMenu();
            }
        }
        
        /// <summary>
        /// Обработчик нажатия кнопки выхода из игры
        /// </summary>
        public void OnQuitButtonClick()
        {
            if (Core.GameManager.Instance != null)
            {
                Core.GameManager.Instance.QuitGame();
            }
        }
    }
}
