using UnityEngine;
using System;
using ScrapArchitect.Core;
using ScrapArchitect.UI;

namespace ScrapArchitect.Controls
{
    /// <summary>
    /// Главный менеджер ввода - координирует все системы управления
    /// </summary>
    public class InputManager : MonoBehaviour
    {
        [Header("Global Input Settings")]
        public KeyCode pauseKey = KeyCode.Escape;
        public KeyCode buildModeKey = KeyCode.B;
        public KeyCode testModeKey = KeyCode.T;
        public KeyCode sandboxModeKey = KeyCode.G;
        public KeyCode mainMenuKey = KeyCode.M;
        
        [Header("UI Input Settings")]
        public KeyCode toggleUIKey = KeyCode.Tab;
        public KeyCode helpKey = KeyCode.F1;
        public KeyCode settingsKey = KeyCode.F2;
        
        [Header("Debug Input Settings")]
        public KeyCode debugModeKey = KeyCode.F3;
        public KeyCode screenshotKey = KeyCode.F12;
        
        // Компоненты
        private CameraController cameraController;
        private PartManipulator partManipulator;
        private GameController gameController;
        private UIManager uiManager;
        
        // Состояние
        private bool isDebugMode = false;
        private bool isUIVisible = true;
        
        // События
        public Action OnPauseToggle;
        public Action OnBuildModeToggle;
        public Action OnTestModeToggle;
        public Action OnSandboxModeToggle;
        public Action OnMainMenuRequest;
        
        private void Start()
        {
            InitializeInputManager();
        }
        
        private void Update()
        {
            HandleGlobalInput();
        }
        
        /// <summary>
        /// Инициализация менеджера ввода
        /// </summary>
        private void InitializeInputManager()
        {
            // Находим компоненты
            cameraController = FindObjectOfType<CameraController>();
            partManipulator = FindObjectOfType<PartManipulator>();
            gameController = FindObjectOfType<GameController>();
            uiManager = FindObjectOfType<UIManager>();
            
            Debug.Log("InputManager initialized");
        }
        
        /// <summary>
        /// Обработка глобального ввода
        /// </summary>
        private void HandleGlobalInput()
        {
            // Пауза
            if (Input.GetKeyDown(pauseKey))
            {
                TogglePause();
            }
            
            // Переключение режимов игры
            if (Input.GetKeyDown(buildModeKey))
            {
                ToggleBuildMode();
            }
            
            if (Input.GetKeyDown(testModeKey))
            {
                ToggleTestMode();
            }
            
            if (Input.GetKeyDown(sandboxModeKey))
            {
                ToggleSandboxMode();
            }
            
            if (Input.GetKeyDown(mainMenuKey))
            {
                ReturnToMainMenu();
            }
            
            // UI управление
            if (Input.GetKeyDown(toggleUIKey))
            {
                ToggleUI();
            }
            
            if (Input.GetKeyDown(helpKey))
            {
                ShowHelp();
            }
            
            if (Input.GetKeyDown(settingsKey))
            {
                ShowSettings();
            }
            
            // Отладочные функции
            if (Input.GetKeyDown(debugModeKey))
            {
                ToggleDebugMode();
            }
            
            if (Input.GetKeyDown(screenshotKey))
            {
                TakeScreenshot();
            }
            
            // Обработка ввода в зависимости от состояния игры
            HandleStateSpecificInput();
        }
        
        /// <summary>
        /// Обработка ввода в зависимости от состояния игры
        /// </summary>
        private void HandleStateSpecificInput()
        {
            if (Core.GameManager.Instance == null)
                return;
            
            switch (Core.GameManager.Instance.currentGameState)
            {
                case GameState.Building:
                    HandleBuildModeInput();
                    break;
                    
                case GameState.Testing:
                    HandleTestModeInput();
                    break;
                    
                case GameState.Playing:
                    HandlePlayModeInput();
                    break;
                    
                case GameState.Paused:
                    HandlePauseModeInput();
                    break;
            }
        }
        
        /// <summary>
        /// Обработка ввода в режиме строительства
        /// </summary>
        private void HandleBuildModeInput()
        {
            // В режиме строительства управление деталями активно
            if (partManipulator != null)
            {
                // Дополнительные горячие клавиши для строительства
                if (Input.GetKeyDown(KeyCode.C))
                {
                    // Копировать выбранную деталь
                    if (partManipulator.GetSelectedPart() != null)
                    {
                        // TODO: Реализовать копирование
                        Debug.Log("Copy part (not implemented yet)");
                    }
                }
                
                if (Input.GetKeyDown(KeyCode.V))
                {
                    // Вставить скопированную деталь
                    Debug.Log("Paste part (not implemented yet)");
                }
                
                if (Input.GetKeyDown(KeyCode.Z) && Input.GetKey(KeyCode.LeftControl))
                {
                    // Отменить последнее действие
                    Debug.Log("Undo (not implemented yet)");
                }
                
                if (Input.GetKeyDown(KeyCode.Y) && Input.GetKey(KeyCode.LeftControl))
                {
                    // Повторить отмененное действие
                    Debug.Log("Redo (not implemented yet)");
                }
            }
        }
        
        /// <summary>
        /// Обработка ввода в режиме тестирования
        /// </summary>
        private void HandleTestModeInput()
        {
            // В режиме тестирования управление транспортом активно
            if (gameController != null)
            {
                // Дополнительные горячие клавиши для тестирования
                if (Input.GetKeyDown(KeyCode.R))
                {
                    // Сбросить позицию транспорта
                    Debug.Log("Reset vehicle position (not implemented yet)");
                }
                
                if (Input.GetKeyDown(KeyCode.L))
                {
                    // Загрузить сохраненную позицию
                    Debug.Log("Load saved position (not implemented yet)");
                }
            }
        }
        
        /// <summary>
        /// Обработка ввода в режиме игры
        /// </summary>
        private void HandlePlayModeInput()
        {
            // В режиме игры все системы управления активны
            // Дополнительные горячие клавиши для игры
            if (Input.GetKeyDown(KeyCode.Q))
            {
                // Быстрое сохранение
                Debug.Log("Quick save (not implemented yet)");
            }
            
            if (Input.GetKeyDown(KeyCode.F))
            {
                // Быстрая загрузка
                Debug.Log("Quick load (not implemented yet)");
            }
        }
        
        /// <summary>
        /// Обработка ввода в режиме паузы
        /// </summary>
        private void HandlePauseModeInput()
        {
            // В режиме паузы только базовые функции доступны
            // Большинство ввода обрабатывается через UI
        }
        
        /// <summary>
        /// Переключить паузу
        /// </summary>
        private void TogglePause()
        {
            if (Core.GameManager.Instance != null)
            {
                Core.GameManager.Instance.TogglePause();
                OnPauseToggle?.Invoke();
                
                Debug.Log($"Pause toggled: {Core.GameManager.Instance.currentGameState == GameState.Paused}");
            }
        }
        
        /// <summary>
        /// Переключить режим строительства
        /// </summary>
        private void ToggleBuildMode()
        {
            if (Core.GameManager.Instance != null)
            {
                if (Core.GameManager.Instance.currentGameState == GameState.Building)
                {
                    Core.GameManager.Instance.EnterTestMode();
                }
                else
                {
                    Core.GameManager.Instance.EnterBuildMode();
                }
                
                OnBuildModeToggle?.Invoke();
                Debug.Log("Build mode toggled");
            }
        }
        
        /// <summary>
        /// Переключить режим тестирования
        /// </summary>
        private void ToggleTestMode()
        {
            if (Core.GameManager.Instance != null)
            {
                Core.GameManager.Instance.EnterTestMode();
                OnTestModeToggle?.Invoke();
                Debug.Log("Test mode activated");
            }
        }
        
        /// <summary>
        /// Переключить режим песочницы
        /// </summary>
        private void ToggleSandboxMode()
        {
            if (Core.GameManager.Instance != null)
            {
                Core.GameManager.Instance.SetGameMode(GameMode.Sandbox);
                OnSandboxModeToggle?.Invoke();
                Debug.Log("Sandbox mode activated");
            }
        }
        
        /// <summary>
        /// Вернуться в главное меню
        /// </summary>
        private void ReturnToMainMenu()
        {
            if (Core.GameManager.Instance != null)
            {
                Core.GameManager.Instance.ReturnToMainMenu();
                OnMainMenuRequest?.Invoke();
                Debug.Log("Returning to main menu");
            }
        }
        
        /// <summary>
        /// Переключить видимость UI
        /// </summary>
        private void ToggleUI()
        {
            isUIVisible = !isUIVisible;
            
            if (uiManager != null)
            {
                // TODO: Реализовать скрытие/показ UI
                Debug.Log($"UI visibility toggled: {isUIVisible}");
            }
        }
        
        /// <summary>
        /// Показать справку
        /// </summary>
        private void ShowHelp()
        {
            Debug.Log("Help requested");
            // TODO: Показать панель справки
        }
        
        /// <summary>
        /// Показать настройки
        /// </summary>
        private void ShowSettings()
        {
            if (uiManager != null)
            {
                uiManager.ShowSettings();
                Debug.Log("Settings opened");
            }
        }
        
        /// <summary>
        /// Переключить отладочный режим
        /// </summary>
        private void ToggleDebugMode()
        {
            isDebugMode = !isDebugMode;
            
            // Включаем/выключаем отладочную информацию
            Debug.unityLogger.logEnabled = isDebugMode;
            
            Debug.Log($"Debug mode toggled: {isDebugMode}");
        }
        
        /// <summary>
        /// Сделать скриншот
        /// </summary>
        private void TakeScreenshot()
        {
            string fileName = $"Screenshot_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.png";
            ScreenCapture.CaptureScreenshotAsTexture();
            
            Debug.Log($"Screenshot saved: {fileName}");
        }
        
        /// <summary>
        /// Получить состояние отладочного режима
        /// </summary>
        public bool IsDebugMode()
        {
            return isDebugMode;
        }
        
        /// <summary>
        /// Получить состояние видимости UI
        /// </summary>
        public bool IsUIVisible()
        {
            return isUIVisible;
        }
        
        /// <summary>
        /// Отключить все системы ввода
        /// </summary>
        public void DisableAllInput()
        {
            if (cameraController != null)
                cameraController.enabled = false;
            
            if (partManipulator != null)
                partManipulator.enabled = false;
            
            if (gameController != null)
                gameController.enabled = false;
            
            Debug.Log("All input systems disabled");
        }
        
        /// <summary>
        /// Включить все системы ввода
        /// </summary>
        public void EnableAllInput()
        {
            if (cameraController != null)
                cameraController.enabled = true;
            
            if (partManipulator != null)
                partManipulator.enabled = true;
            
            if (gameController != null)
                gameController.enabled = true;
            
            Debug.Log("All input systems enabled");
        }
        
        /// <summary>
        /// Сбросить все системы ввода
        /// </summary>
        public void ResetAllInput()
        {
            if (gameController != null)
                gameController.ResetControls();
            
            if (partManipulator != null)
                partManipulator.DeselectPart();
            
            if (cameraController != null)
                cameraController.ResetView();
            
            Debug.Log("All input systems reset");
        }
    }
}
