using UnityEngine;
using UnityEngine.SceneManagement;

namespace ScrapArchitect.Core
{
    /// <summary>
    /// Основной менеджер игры - управляет состоянием игры и переключением между режимами
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        [Header("Game State")]
        public GameState currentGameState = GameState.MainMenu;
        public GameMode currentGameMode = GameMode.None;
        
        [Header("Scenes")]
        public string mainMenuScene = "MainMenu";
        public string buildModeScene = "BuildMode";
        public string testModeScene = "TestMode";
        
        [Header("Settings")]
        public bool isPaused = false;
        public float gameTime = 0f;
        
        // Singleton pattern
        public static GameManager Instance { get; private set; }
        
        // Events
        public System.Action<GameState> OnGameStateChanged;
        public System.Action<GameMode> OnGameModeChanged;
        public System.Action<bool> OnPauseChanged;
        
        private void Awake()
        {
            // Singleton pattern
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                InitializeGame();
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
        private void Start()
        {
            // Устанавливаем начальное состояние
            SetGameState(GameState.MainMenu);
        }
        
        private void Update()
        {
            // Обновляем игровое время
            if (!isPaused && currentGameState == GameState.Playing)
            {
                gameTime += Time.deltaTime;
            }
            
            // Обработка паузы
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                TogglePause();
            }
        }
        
        /// <summary>
        /// Инициализация игры
        /// </summary>
        private void InitializeGame()
        {
            Debug.Log("GameManager initialized");
            
            // Загружаем сохранения
            LoadGameSettings();
            
            // Устанавливаем качество графики
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 60;
        }
        
        /// <summary>
        /// Установка состояния игры
        /// </summary>
        public void SetGameState(GameState newState)
        {
            if (currentGameState != newState)
            {
                GameState oldState = currentGameState;
                currentGameState = newState;
                
                Debug.Log($"Game State changed: {oldState} -> {newState}");
                
                // Обработка изменения состояния
                HandleGameStateChange(oldState, newState);
                
                // Вызываем событие
                OnGameStateChanged?.Invoke(newState);
            }
        }
        
        /// <summary>
        /// Установка режима игры
        /// </summary>
        public void SetGameMode(GameMode newMode)
        {
            if (currentGameMode != newMode)
            {
                GameMode oldMode = currentGameMode;
                currentGameMode = newMode;
                
                Debug.Log($"Game Mode changed: {oldMode} -> {newMode}");
                
                // Вызываем событие
                OnGameModeChanged?.Invoke(newMode);
            }
        }
        
        /// <summary>
        /// Обработка изменения состояния игры
        /// </summary>
        private void HandleGameStateChange(GameState oldState, GameState newState)
        {
            switch (newState)
            {
                case GameState.MainMenu:
                    LoadScene(mainMenuScene);
                    break;
                    
                case GameState.Building:
                    LoadScene(buildModeScene);
                    SetGameMode(GameMode.Build);
                    break;
                    
                case GameState.Testing:
                    LoadScene(testModeScene);
                    SetGameMode(GameMode.Test);
                    break;
                    
                case GameState.Playing:
                    isPaused = false;
                    gameTime = 0f;
                    break;
                    
                case GameState.Paused:
                    isPaused = true;
                    break;
            }
        }
        
        /// <summary>
        /// Загрузка сцены
        /// </summary>
        public void LoadScene(string sceneName)
        {
            Debug.Log($"Loading scene: {sceneName}");
            SceneManager.LoadScene(sceneName);
        }
        
        /// <summary>
        /// Переключение паузы
        /// </summary>
        public void TogglePause()
        {
            if (currentGameState == GameState.Playing)
            {
                SetGameState(GameState.Paused);
            }
            else if (currentGameState == GameState.Paused)
            {
                SetGameState(GameState.Playing);
            }
            
            isPaused = !isPaused;
            OnPauseChanged?.Invoke(isPaused);
        }
        
        /// <summary>
        /// Переход в режим строительства
        /// </summary>
        public void EnterBuildMode()
        {
            SetGameState(GameState.Building);
        }
        
        /// <summary>
        /// Переход в режим тестирования
        /// </summary>
        public void EnterTestMode()
        {
            SetGameState(GameState.Testing);
        }
        
        /// <summary>
        /// Возврат в главное меню
        /// </summary>
        public void ReturnToMainMenu()
        {
            SetGameState(GameState.MainMenu);
        }
        
        /// <summary>
        /// Загрузка настроек игры
        /// </summary>
        private void LoadGameSettings()
        {
            // TODO: Загрузить настройки из PlayerPrefs или файла
            Debug.Log("Loading game settings...");
        }
        
        /// <summary>
        /// Сохранение настроек игры
        /// </summary>
        public void SaveGameSettings()
        {
            // TODO: Сохранить настройки в PlayerPrefs или файл
            Debug.Log("Saving game settings...");
        }
        
        /// <summary>
        /// Выход из игры
        /// </summary>
        public void QuitGame()
        {
            SaveGameSettings();
            
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #else
                Application.Quit();
            #endif
        }
    }
    
    /// <summary>
    /// Состояния игры
    /// </summary>
    public enum GameState
    {
        MainMenu,
        Building,
        Testing,
        Playing,
        Paused,
        GameOver,
        Victory
    }
    
    /// <summary>
    /// Режимы игры
    /// </summary>
    public enum GameMode
    {
        None,
        Build,
        Test,
        Sandbox
    }
}
