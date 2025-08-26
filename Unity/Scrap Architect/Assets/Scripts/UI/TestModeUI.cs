using UnityEngine;
using UnityEngine.UI;
using TMPro;
using ScrapArchitect.Level;
using ScrapArchitect.Core;

namespace ScrapArchitect.UI
{
    /// <summary>
    /// UI для тестового режима
    /// </summary>
    public class TestModeUI : MonoBehaviour
    {
        [Header("Test Controls")]
        public Button startTestButton;
        public Button stopTestButton;
        public Button resetTestButton;
        public Button backToBuildButton;
        
        [Header("Test Info")]
        public TextMeshProUGUI timeText;
        public TextMeshProUGUI scoreText;
        public TextMeshProUGUI statusText;
        
        [Header("Goals Panel")]
        public GameObject goalsPanel;
        public Transform goalsContainer;
        public GameObject goalItemPrefab;
        
        [Header("Results Panel")]
        public GameObject resultsPanel;
        public TextMeshProUGUI finalScoreText;
        public TextMeshProUGUI finalTimeText;
        public TextMeshProUGUI goalsCompletedText;
        public Button restartButton;
        public Button backToMenuButton;
        
        [Header("HUD Elements")]
        public GameObject hudPanel;
        public TextMeshProUGUI currentGoalText;
        public TextMeshProUGUI partsCountText;
        public TextMeshProUGUI speedText;
        
        // Приватные переменные
        private TestPolygon testPolygon;
        private List<GameObject> goalItems = new List<GameObject>();
        
        private void Start()
        {
            InitializeTestUI();
        }
        
        /// <summary>
        /// Инициализация UI тестирования
        /// </summary>
        private void InitializeTestUI()
        {
            testPolygon = FindObjectOfType<TestPolygon>();
            
            if (testPolygon != null)
            {
                // Подписываемся на события полигона
                testPolygon.OnTimeChanged += UpdateTimeDisplay;
                testPolygon.OnScoreChanged += UpdateScoreDisplay;
                testPolygon.OnTestStateChanged += UpdateTestState;
                testPolygon.OnTestCompleted += ShowResults;
                testPolygon.OnTestFailed += ShowResults;
            }
            
            // Настройка кнопок
            SetupButtons();
            
            // Создание элементов целей
            CreateGoalItems();
            
            // Начальное состояние
            UpdateUI();
            
            Debug.Log("TestModeUI initialized");
        }
        
        /// <summary>
        /// Настройка кнопок
        /// </summary>
        private void SetupButtons()
        {
            if (startTestButton != null)
                startTestButton.onClick.AddListener(OnStartTestClick);
                
            if (stopTestButton != null)
                stopTestButton.onClick.AddListener(OnStopTestClick);
                
            if (resetTestButton != null)
                resetTestButton.onClick.AddListener(OnResetTestClick);
                
            if (backToBuildButton != null)
                backToBuildButton.onClick.AddListener(OnBackToBuildClick);
                
            if (restartButton != null)
                restartButton.onClick.AddListener(OnRestartClick);
                
            if (backToMenuButton != null)
                backToMenuButton.onClick.AddListener(OnBackToMenuClick);
        }
        
        /// <summary>
        /// Создание элементов целей
        /// </summary>
        private void CreateGoalItems()
        {
            if (testPolygon == null || goalsContainer == null)
                return;
                
            // Очищаем существующие элементы
            foreach (var item in goalItems)
            {
                if (item != null)
                    Destroy(item);
            }
            goalItems.Clear();
            
            // Создаем элементы для каждой цели
            foreach (var goal in testPolygon.goals)
            {
                CreateGoalItem(goal);
            }
        }
        
        /// <summary>
        /// Создание элемента цели
        /// </summary>
        private void CreateGoalItem(TestGoal goal)
        {
            if (goalItemPrefab == null || goalsContainer == null)
                return;
                
            GameObject goalItem = Instantiate(goalItemPrefab, goalsContainer);
            goalItems.Add(goalItem);
            
            // Настраиваем элемент цели
            var goalUI = goalItem.GetComponent<GoalItemUI>();
            if (goalUI != null)
            {
                goalUI.Initialize(goal);
            }
            else
            {
                // Простая настройка без специального компонента
                var textComponent = goalItem.GetComponentInChildren<TextMeshProUGUI>();
                if (textComponent != null)
                {
                    textComponent.text = goal.description;
                }
            }
        }
        
        /// <summary>
        /// Обновление отображения времени
        /// </summary>
        private void UpdateTimeDisplay(float time)
        {
            if (timeText != null)
            {
                int minutes = Mathf.FloorToInt(time / 60f);
                int seconds = Mathf.FloorToInt(time % 60f);
                timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
            }
        }
        
        /// <summary>
        /// Обновление отображения счета
        /// </summary>
        private void UpdateScoreDisplay(float score)
        {
            if (scoreText != null)
            {
                scoreText.text = $"Score: {score:F0}";
            }
        }
        
        /// <summary>
        /// Обновление состояния теста
        /// </summary>
        private void UpdateTestState(bool isActive)
        {
            UpdateUI();
            
            if (statusText != null)
            {
                statusText.text = isActive ? "TEST ACTIVE" : "TEST READY";
                statusText.color = isActive ? Color.green : Color.white;
            }
        }
        
        /// <summary>
        /// Показ результатов
        /// </summary>
        private void ShowResults()
        {
            if (resultsPanel != null)
            {
                resultsPanel.SetActive(true);
                
                if (testPolygon != null)
                {
                    if (finalScoreText != null)
                        finalScoreText.text = $"Final Score: {testPolygon.GetCurrentScore():F0}";
                        
                    if (finalTimeText != null)
                    {
                        float time = testPolygon.GetCurrentTime();
                        int minutes = Mathf.FloorToInt(time / 60f);
                        int seconds = Mathf.FloorToInt(time % 60f);
                        finalTimeText.text = $"Time: {minutes:00}:{seconds:00}";
                    }
                    
                    if (goalsCompletedText != null)
                    {
                        int completedGoals = 0;
                        foreach (var goal in testPolygon.goals)
                        {
                            if (goal.isCompleted)
                                completedGoals++;
                        }
                        goalsCompletedText.text = $"Goals Completed: {completedGoals}/{testPolygon.goals.Count}";
                    }
                }
            }
        }
        
        /// <summary>
        /// Обновление UI
        /// </summary>
        private void UpdateUI()
        {
            bool isTestActive = testPolygon != null && testPolygon.IsTestActive();
            
            // Обновляем состояние кнопок
            if (startTestButton != null)
                startTestButton.interactable = !isTestActive;
                
            if (stopTestButton != null)
                stopTestButton.interactable = isTestActive;
                
            if (resetTestButton != null)
                resetTestButton.interactable = !isTestActive;
                
            // Обновляем HUD
            UpdateHUD();
        }
        
        /// <summary>
        /// Обновление HUD
        /// </summary>
        private void UpdateHUD()
        {
            if (hudPanel != null)
            {
                bool isTestActive = testPolygon != null && testPolygon.IsTestActive();
                hudPanel.SetActive(isTestActive);
            }
            
            // Обновляем информацию о деталях
            if (partsCountText != null)
            {
                var parts = FindObjectsOfType<ScrapArchitect.Physics.PartController>();
                partsCountText.text = $"Parts: {parts.Length}";
            }
            
            // Обновляем информацию о скорости
            if (speedText != null)
            {
                // TODO: Получить скорость от GameController
                speedText.text = "Speed: 0 km/h";
            }
            
            // Обновляем текущую цель
            if (currentGoalText != null && testPolygon != null)
            {
                string currentGoal = "No active goal";
                foreach (var goal in testPolygon.goals)
                {
                    if (!goal.isCompleted)
                    {
                        currentGoal = goal.description;
                        break;
                    }
                }
                currentGoalText.text = $"Current Goal: {currentGoal}";
            }
        }
        
        /// <summary>
        /// Обработчик клика по кнопке "Начать тест"
        /// </summary>
        private void OnStartTestClick()
        {
            if (testPolygon != null)
            {
                testPolygon.StartTest();
            }
        }
        
        /// <summary>
        /// Обработчик клика по кнопке "Остановить тест"
        /// </summary>
        private void OnStopTestClick()
        {
            if (testPolygon != null)
            {
                testPolygon.StopTest();
            }
        }
        
        /// <summary>
        /// Обработчик клика по кнопке "Сбросить тест"
        /// </summary>
        private void OnResetTestClick()
        {
            if (testPolygon != null)
            {
                testPolygon.ResetTest();
            }
            
            if (resultsPanel != null)
            {
                resultsPanel.SetActive(false);
            }
        }
        
        /// <summary>
        /// Обработчик клика по кнопке "Вернуться к строительству"
        /// </summary>
        private void OnBackToBuildClick()
        {
            if (Core.GameManager.Instance != null)
            {
                Core.GameManager.Instance.EnterBuildMode();
            }
        }
        
        /// <summary>
        /// Обработчик клика по кнопке "Перезапустить"
        /// </summary>
        private void OnRestartClick()
        {
            if (resultsPanel != null)
            {
                resultsPanel.SetActive(false);
            }
            
            if (testPolygon != null)
            {
                testPolygon.ResetTest();
            }
        }
        
        /// <summary>
        /// Обработчик клика по кнопке "Вернуться в меню"
        /// </summary>
        private void OnBackToMenuClick()
        {
            if (Core.GameManager.Instance != null)
            {
                Core.GameManager.Instance.ReturnToMainMenu();
            }
        }
        
        /// <summary>
        /// Показать UI
        /// </summary>
        public void Show()
        {
            gameObject.SetActive(true);
            UpdateUI();
        }
        
        /// <summary>
        /// Скрыть UI
        /// </summary>
        public void Hide()
        {
            gameObject.SetActive(false);
        }
        
        /// <summary>
        /// Обновить элементы целей
        /// </summary>
        public void RefreshGoalItems()
        {
            CreateGoalItems();
        }
        
        private void OnDestroy()
        {
            // Отписываемся от событий
            if (testPolygon != null)
            {
                testPolygon.OnTimeChanged -= UpdateTimeDisplay;
                testPolygon.OnScoreChanged -= UpdateScoreDisplay;
                testPolygon.OnTestStateChanged -= UpdateTestState;
                testPolygon.OnTestCompleted -= ShowResults;
                testPolygon.OnTestFailed -= ShowResults;
            }
        }
    }
    
    /// <summary>
    /// UI элемент цели (если нужен отдельный компонент)
    /// </summary>
    public class GoalItemUI : MonoBehaviour
    {
        public TextMeshProUGUI goalText;
        public Image statusIcon;
        public Color completedColor = Color.green;
        public Color pendingColor = Color.yellow;
        
        private TestGoal goal;
        
        public void Initialize(TestGoal testGoal)
        {
            goal = testGoal;
            UpdateDisplay();
        }
        
        public void UpdateDisplay()
        {
            if (goal == null)
                return;
                
            if (goalText != null)
            {
                goalText.text = goal.description;
            }
            
            if (statusIcon != null)
            {
                statusIcon.color = goal.isCompleted ? completedColor : pendingColor;
            }
        }
    }
}
