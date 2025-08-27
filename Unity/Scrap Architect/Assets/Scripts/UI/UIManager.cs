using UnityEngine;
using System;
using System.Collections.Generic;
using ScrapArchitect.Gameplay;
using ScrapArchitect.UI;

namespace ScrapArchitect.UI
{
    /// <summary>
    /// Центральный менеджер интерфейса - управляет всеми UI экранами
    /// </summary>
    public class UIManager : MonoBehaviour
    {
        // Singleton pattern
        public static UIManager Instance { get; private set; }
        
        [Header("UI Panels")]
        public MainMenuUI mainMenuPanel;
        public ContractSelectionUI contractSelectionPanel;
        public WorldMapUI worldMapPanel;
        public GameplayUI gameplayPanel;
        public VictoryScreen victoryPanel;
        public DefeatScreen defeatPanel;
        public SettingsUI settingsPanel;
        public PauseMenu pausePanel;
        public LoadingScreen loadingPanel;
        
        [Header("Game Modes")]
        public BuildingMode buildingMode;
        
        [Header("Audio")]
        public AudioClip buttonClickSound;
        public AudioClip panelOpenSound;
        public AudioClip panelCloseSound;
        
        [Header("Animation Settings")]
        public float panelAnimationDuration = 0.3f;
        public AnimationCurve panelAnimationCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        
        private UIBase currentActivePanel;
        private Stack<UIBase> panelHistory = new Stack<UIBase>();
        
        // Events
        public Action<UIBase> OnPanelOpened;
        public Action<UIBase> OnPanelClosed;
        public Action OnGameStarted;
        public Action OnGamePaused;
        public Action OnGameResumed;
        public Action OnGameQuit;
        
        private void Awake()
        {
            // Singleton pattern
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                InitializeUIManager();
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
        private void Start()
        {
            // Показать главное меню при запуске
            ShowMainMenu();
        }
        
        /// <summary>
        /// Инициализация UI менеджера
        /// </summary>
        private void InitializeUIManager()
        {
            // Инициализируем все панели
            InitializePanel(mainMenuPanel);
            InitializePanel(contractSelectionPanel);
            InitializePanel(worldMapPanel);
            InitializePanel(gameplayPanel);
            InitializePanel(victoryPanel);
            InitializePanel(defeatPanel);
            InitializePanel(settingsPanel);
            InitializePanel(pausePanel);
            InitializePanel(loadingPanel);
            
            Debug.Log("UIManager initialized");
        }
        
        /// <summary>
        /// Инициализация панели
        /// </summary>
        private void InitializePanel(UIBase panel)
        {
            if (panel != null)
            {
                panel.Initialize(this);
                panel.gameObject.SetActive(false);
            }
        }
        
        /// <summary>
        /// Показать главное меню
        /// </summary>
        public void ShowMainMenu()
        {
            ShowPanel(mainMenuPanel);
        }
        
        /// <summary>
        /// Показать выбор контрактов
        /// </summary>
        public void ShowContractSelection()
        {
            ShowPanel(contractSelectionPanel);
        }
        
        /// <summary>
        /// Показать игровой интерфейс
        /// </summary>
        public void ShowGameplayUI()
        {
            ShowPanel(gameplayPanel);
            OnGameStarted?.Invoke();
        }
        
        /// <summary>
        /// Показать экран победы
        /// </summary>
        public void ShowVictoryScreen()
        {
            ShowPanel(victoryPanel);
        }
        
        /// <summary>
        /// Показать экран поражения
        /// </summary>
        public void ShowDefeatScreen()
        {
            ShowPanel(defeatPanel);
        }
        
        /// <summary>
        /// Показать настройки
        /// </summary>
        public void ShowSettings()
        {
            ShowPanel(settingsPanel);
        }
        
        /// <summary>
        /// Показать меню паузы
        /// </summary>
        public void ShowPauseMenu()
        {
            ShowPanel(pausePanel);
            OnGamePaused?.Invoke();
        }
        
        /// <summary>
        /// Скрыть меню паузы
        /// </summary>
        public void HidePauseMenu()
        {
            HideCurrentPanel();
            OnGameResumed?.Invoke();
        }
        
        /// <summary>
        /// Показать экран загрузки
        /// </summary>
        public void ShowLoadingScreen()
        {
            ShowPanel(loadingPanel);
        }
        
        /// <summary>
        /// Скрыть экран загрузки
        /// </summary>
        public void HideLoadingScreen()
        {
            HideCurrentPanel();
        }
        
        /// <summary>
        /// Показать карту мира
        /// </summary>
        public void ShowWorldMap()
        {
            ShowPanel(worldMapPanel);
        }
        
        /// <summary>
        /// Показать режим строительства
        /// </summary>
        public void ShowBuildingMode()
        {
            // Скрываем все UI панели
            HideAllPanels();
            
            // Активируем режим строительства
            if (buildingMode != null)
            {
                buildingMode.gameObject.SetActive(true);
                currentActivePanel = null; // Режим строительства не является UI панелью
            }
            else
            {
                Debug.LogWarning("BuildingMode не найден! Переходим к выбору контрактов.");
                ShowContractSelection();
            }
        }
        
        /// <summary>
        /// Показать панель
        /// </summary>
        public void ShowPanel(UIBase panel)
        {
            if (panel == null) return;
            
            // Скрыть текущую панель
            if (currentActivePanel != null)
            {
                currentActivePanel.Hide();
                panelHistory.Push(currentActivePanel);
            }
            
            // Показать новую панель
            currentActivePanel = panel;
            panel.Show();
            
            OnPanelOpened?.Invoke(panel);
            PlayPanelOpenSound();
        }
        
        /// <summary>
        /// Скрыть текущую панель
        /// </summary>
        public void HideCurrentPanel()
        {
            if (currentActivePanel != null)
            {
                currentActivePanel.Hide();
                OnPanelClosed?.Invoke(currentActivePanel);
                PlayPanelCloseSound();
                
                // Восстановить предыдущую панель
                if (panelHistory.Count > 0)
                {
                    currentActivePanel = panelHistory.Pop();
                    currentActivePanel.Show();
                }
                else
                {
                    currentActivePanel = null;
                }
            }
        }
        
        /// <summary>
        /// Вернуться к предыдущей панели
        /// </summary>
        public void GoBack()
        {
            if (panelHistory.Count > 0)
            {
                HideCurrentPanel();
            }
        }
        
        /// <summary>
        /// Очистить историю панелей
        /// </summary>
        public void ClearPanelHistory()
        {
            panelHistory.Clear();
        }
        
        /// <summary>
        /// Получить текущую активную панель
        /// </summary>
        public UIBase GetCurrentPanel()
        {
            return currentActivePanel;
        }
        
        /// <summary>
        /// Проверить, активна ли панель
        /// </summary>
        public bool IsPanelActive<T>() where T : UIBase
        {
            return currentActivePanel is T;
        }
        
        /// <summary>
        /// Скрыть все панели
        /// </summary>
        public void HideAllPanels()
        {
            // Скрываем все UI панели
            if (mainMenuPanel != null) mainMenuPanel.gameObject.SetActive(false);
            if (contractSelectionPanel != null) contractSelectionPanel.gameObject.SetActive(false);
            if (worldMapPanel != null) worldMapPanel.gameObject.SetActive(false);
            if (gameplayPanel != null) gameplayPanel.gameObject.SetActive(false);
            if (victoryPanel != null) victoryPanel.gameObject.SetActive(false);
            if (defeatPanel != null) defeatPanel.gameObject.SetActive(false);
            if (settingsPanel != null) settingsPanel.gameObject.SetActive(false);
            if (pausePanel != null) pausePanel.gameObject.SetActive(false);
            if (loadingPanel != null) loadingPanel.gameObject.SetActive(false);
            
            // Скрываем режим строительства
            if (buildingMode != null) buildingMode.gameObject.SetActive(false);
            
            currentActivePanel = null;
            panelHistory.Clear();
        }
        
        #region Audio Methods
        
        /// <summary>
        /// Воспроизвести звук клика кнопки
        /// </summary>
        public void PlayButtonClickSound()
        {
            if (buttonClickSound != null)
            {
                AudioSource.PlayClipAtPoint(buttonClickSound, Camera.main.transform.position);
            }
        }
        
        /// <summary>
        /// Воспроизвести звук открытия панели
        /// </summary>
        private void PlayPanelOpenSound()
        {
            if (panelOpenSound != null)
            {
                AudioSource.PlayClipAtPoint(panelOpenSound, Camera.main.transform.position);
            }
        }
        
        /// <summary>
        /// Воспроизвести звук закрытия панели
        /// </summary>
        private void PlayPanelCloseSound()
        {
            if (panelCloseSound != null)
            {
                AudioSource.PlayClipAtPoint(panelCloseSound, Camera.main.transform.position);
            }
        }
        
        #endregion
        
        #region Tooltip Methods
        
        /// <summary>
        /// Показать информацию о детали
        /// </summary>
        public void ShowPartInfo(string partName, string tooltipText)
        {
            // TODO: Реализовать систему подсказок
            Debug.Log($"Part Info: {partName} - {tooltipText}");
        }
        
        /// <summary>
        /// Скрыть информацию о детали
        /// </summary>
        public void HidePartInfo()
        {
            // TODO: Скрыть подсказку
            Debug.Log("Hide part info");
        }
        
        #endregion
        
        #region Game Control Methods
        
        /// <summary>
        /// Начать новую игру
        /// </summary>
        public void StartNewGame()
        {
            ShowLoadingScreen();
            // Здесь будет логика загрузки игры
            Invoke(nameof(LoadGame), 1f);
        }
        
        /// <summary>
        /// Загрузить игру
        /// </summary>
        private void LoadGame()
        {
            HideLoadingScreen();
            // Переходим к режиму строительства вместо выбора контрактов
            ShowBuildingMode();
        }
        
        /// <summary>
        /// Выйти из игры
        /// </summary>
        public void QuitGame()
        {
            OnGameQuit?.Invoke();
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #else
                Application.Quit();
            #endif
        }
        
        #endregion
    }
}
