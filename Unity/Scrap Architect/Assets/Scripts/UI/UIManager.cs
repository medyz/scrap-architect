using UnityEngine;
using System;
using System.Collections;
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
                // Перемещаем в корень сцены перед DontDestroyOnLoad
                transform.SetParent(null);
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
            // Используем Invoke для отложенного вызова, чтобы все панели успели инициализироваться
            Invoke(nameof(ShowMainMenu), 0.1f);
        }
        
        /// <summary>
        /// Инициализация UI менеджера
        /// </summary>
        private void InitializeUIManager()
        {
            Debug.Log("=== UIManager InitializeUIManager ===");
            Debug.Log($"mainMenuPanel is null: {mainMenuPanel == null}");
            
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
                Debug.Log($"Initializing panel: {panel.name}");
                panel.Initialize(this);
                // НЕ деактивируем панель здесь - это делает Initialize()
                Debug.Log($"InitializePanel: {panel.name} initialized, keeping current active state");
            }
            else
            {
                Debug.Log("InitializePanel: panel is null");
            }
        }
        
        /// <summary>
        /// Показать главное меню
        /// </summary>
        public void ShowMainMenu()
        {
            Debug.Log($"ShowMainMenu called. mainMenuPanel is null: {mainMenuPanel == null}");
            if (mainMenuPanel == null)
            {
                Debug.LogError("MainMenuPanel is not assigned in UIManager!");
                return;
            }
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
            Debug.Log($"ShowSettings: Called, settingsPanel is {(settingsPanel == null ? "null" : "assigned")}");
            if (settingsPanel != null)
            {
                Debug.Log($"ShowSettings: Showing settings panel: {settingsPanel.name}");
                ShowPanel(settingsPanel);
            }
            else
            {
                Debug.LogError("ShowSettings: settingsPanel is null! Please assign it in the Inspector.");
            }
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
            Debug.Log($"ShowPanel called with panel: {panel?.name ?? "null"}");
            if (panel == null)
            {
                Debug.LogError("ShowPanel: panel is null!");
                return;
            }

            Debug.Log($"Panel GameObject active: {panel.gameObject.activeInHierarchy}");

            // Проверяем Canvas
            Canvas canvas = panel.GetComponentInParent<Canvas>();
            if (canvas != null)
            {
                Debug.Log($"ShowPanel: Found Canvas {canvas.name} - active: {canvas.gameObject.activeInHierarchy}");
                if (!canvas.gameObject.activeInHierarchy)
                {
                    Debug.LogWarning($"ShowPanel: Canvas {canvas.name} is inactive! Activating...");
                    canvas.gameObject.SetActive(true);
                }
            }
            else
            {
                Debug.LogWarning($"ShowPanel: No Canvas found in parents of {panel.name}!");
                
                // Создаем Canvas если его нет
                Debug.LogWarning($"ShowPanel: Creating Canvas for {panel.name}!");
                GameObject canvasObj = new GameObject("UI Canvas");
                canvas = canvasObj.AddComponent<Canvas>();
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                canvasObj.AddComponent<UnityEngine.UI.CanvasScaler>();
                canvasObj.AddComponent<UnityEngine.UI.GraphicRaycaster>();
                
                // Перемещаем панель под Canvas
                panel.transform.SetParent(canvasObj.transform);
                Debug.Log($"ShowPanel: Moved {panel.name} under Canvas {canvasObj.name}");
            }

            // Инициализируем панель, если она еще не инициализирована
            if (!panel.IsInitialized)
            {
                Debug.Log($"Initializing panel: {panel.name}");
                panel.Initialize(this);
            }

            // Скрыть текущую панель
            if (currentActivePanel != null)
            {
                currentActivePanel.Hide();
                panelHistory.Push(currentActivePanel);
            }

            // Убеждаемся, что GameObject активен перед показом
            if (!panel.gameObject.activeInHierarchy)
            {
                panel.gameObject.SetActive(true);
                Debug.Log($"Activated panel GameObject: {panel.name}");

                // Даем один кадр для активации GameObject
                StartCoroutine(ShowPanelDelayed(panel));
            }
            else
            {
                // Показать новую панель сразу
                currentActivePanel = panel;
                panel.Show();

                Debug.Log($"Panel {panel.name} should now be visible");

                OnPanelOpened?.Invoke(panel);
                PlayPanelOpenSound();
            }
        }
        
        /// <summary>
        /// Показать панель с задержкой в один кадр
        /// </summary>
        private IEnumerator ShowPanelDelayed(UIBase panel)
        {
            Debug.Log($"ShowPanelDelayed: Starting for panel {panel.name}");
            
            // Ждем один кадр, чтобы GameObject успел активироваться
            yield return null;
            
            Debug.Log($"ShowPanelDelayed: After yield, panel {panel.name} active: {panel.gameObject.activeInHierarchy}");
            
            // Если панель стала неактивной, активируем её снова
            if (!panel.gameObject.activeInHierarchy)
            {
                Debug.LogWarning($"ShowPanelDelayed: Panel {panel.name} became inactive, reactivating");
                
                // Проверяем всю иерархию родителей
                Transform parent = panel.transform.parent;
                while (parent != null)
                {
                    Debug.Log($"ShowPanelDelayed: Checking parent {parent.name} - active: {parent.gameObject.activeInHierarchy}");
                    if (!parent.gameObject.activeInHierarchy)
                    {
                        Debug.LogWarning($"ShowPanelDelayed: Parent {parent.name} is inactive! Activating...");
                        parent.gameObject.SetActive(true);
                    }
                    parent = parent.parent;
                }
                
                panel.gameObject.SetActive(true);
                yield return null; // Ждем еще один кадр
                Debug.Log($"ShowPanelDelayed: After reactivation, panel {panel.name} active: {panel.gameObject.activeInHierarchy}");
            }
            
            // Инициализируем панель, если она еще не инициализирована
            if (!panel.IsInitialized)
            {
                Debug.Log($"ShowPanelDelayed: Initializing panel (delayed): {panel.name}");
                panel.Initialize(this);
            }
            else
            {
                Debug.Log($"ShowPanelDelayed: Panel {panel.name} already initialized");
            }
            
            // Показать новую панель
            currentActivePanel = panel;
            Debug.Log($"ShowPanelDelayed: Calling panel.Show() for {panel.name}");
            panel.Show();
            
            Debug.Log($"ShowPanelDelayed: Panel {panel.name} should now be visible (delayed)");
            
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
