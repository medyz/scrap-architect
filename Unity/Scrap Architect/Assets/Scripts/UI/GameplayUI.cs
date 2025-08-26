using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using ScrapArchitect.Gameplay;

namespace ScrapArchitect.UI
{
    /// <summary>
    /// Игровой интерфейс
    /// </summary>
    public class GameplayUI : UIBase
    {
        [Header("Contract Info")]
        public TextMeshProUGUI contractTitleText;
        public TextMeshProUGUI contractDescriptionText;
        public TextMeshProUGUI timeText;
        public TextMeshProUGUI difficultyText;
        
        [Header("Objectives")]
        public Transform objectivesContainer;
        public GameObject objectiveItemPrefab;
        
        [Header("Progress")]
        public Slider overallProgressSlider;
        public TextMeshProUGUI progressText;
        
        [Header("Controls")]
        public Button pauseButton;
        public Button objectivesButton;
        public Button helpButton;
        
        [Header("HUD Elements")]
        public GameObject objectivesPanel;
        public GameObject helpPanel;
        public TextMeshProUGUI helpText;
        
        [Header("Animation")]
        public float objectiveUpdateDelay = 0.2f;
        
        private List<ObjectiveItemUI> objectiveItems = new List<ObjectiveItemUI>();
        private Contract currentContract;
        private bool objectivesPanelVisible = false;
        private bool helpPanelVisible = false;
        
        private void Start()
        {
            SetupButtons();
            SetupPanels();
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
            
            if (objectivesButton != null)
            {
                objectivesButton.onClick.AddListener(OnObjectivesButtonClick);
            }
            
            if (helpButton != null)
            {
                helpButton.onClick.AddListener(OnHelpButtonClick);
            }
        }
        
        /// <summary>
        /// Настройка панелей
        /// </summary>
        private void SetupPanels()
        {
            if (objectivesPanel != null)
            {
                objectivesPanel.SetActive(false);
            }
            
            if (helpPanel != null)
            {
                helpPanel.SetActive(false);
            }
        }
        
        /// <summary>
        /// Вызывается при показе игрового интерфейса
        /// </summary>
        protected override void OnShow()
        {
            base.OnShow();
            
            LoadCurrentContract();
            UpdateDisplay();
        }
        
        /// <summary>
        /// Загрузить текущий контракт
        /// </summary>
        private void LoadCurrentContract()
        {
            if (ContractManager.Instance != null)
            {
                List<Contract> activeContracts = ContractManager.Instance.GetActiveContracts();
                if (activeContracts.Count > 0)
                {
                    currentContract = activeContracts[0];
                }
            }
        }
        
        /// <summary>
        /// Обновить отображение
        /// </summary>
        private void UpdateDisplay()
        {
            if (currentContract == null) return;
            
            // Информация о контракте
            if (contractTitleText != null)
            {
                contractTitleText.text = currentContract.title;
            }
            
            if (contractDescriptionText != null)
            {
                contractDescriptionText.text = currentContract.description;
            }
            
            if (difficultyText != null)
            {
                difficultyText.text = $"Сложность: {GetDifficultyText(currentContract.difficulty)}";
            }
            
            // Цели
            UpdateObjectivesDisplay();
            
            // Общий прогресс
            UpdateOverallProgress();
        }
        
        /// <summary>
        /// Обновить отображение целей
        /// </summary>
        private void UpdateObjectivesDisplay()
        {
            if (objectivesContainer == null || objectiveItemPrefab == null) return;
            
            // Очистить существующие элементы
            ClearObjectiveItems();
            
            // Создать элементы для каждой цели
            foreach (var objective in currentContract.objectives)
            {
                GameObject objectiveItemObj = Instantiate(objectiveItemPrefab, objectivesContainer);
                ObjectiveItemUI objectiveItem = objectiveItemObj.GetComponent<ObjectiveItemUI>();
                
                if (objectiveItem != null)
                {
                    objectiveItem.Initialize(objective, false);
                    objectiveItems.Add(objectiveItem);
                }
            }
        }
        
        /// <summary>
        /// Очистить элементы целей
        /// </summary>
        private void ClearObjectiveItems()
        {
            foreach (ObjectiveItemUI item in objectiveItems)
            {
                if (item != null)
                {
                    Destroy(item.gameObject);
                }
            }
            objectiveItems.Clear();
        }
        
        /// <summary>
        /// Обновить общий прогресс
        /// </summary>
        private void UpdateOverallProgress()
        {
            if (currentContract == null) return;
            
            float progress = currentContract.GetOverallProgress();
            
            if (overallProgressSlider != null)
            {
                overallProgressSlider.value = progress;
            }
            
            if (progressText != null)
            {
                progressText.text = $"{Mathf.RoundToInt(progress * 100)}%";
            }
        }
        
        /// <summary>
        /// Обновить время
        /// </summary>
        public void UpdateTime(float elapsedTime)
        {
            if (timeText != null)
            {
                int minutes = Mathf.FloorToInt(elapsedTime / 60f);
                int seconds = Mathf.FloorToInt(elapsedTime % 60f);
                timeText.text = $"{minutes:00}:{seconds:00}";
            }
        }
        
        /// <summary>
        /// Обновить цели
        /// </summary>
        public void UpdateObjectives()
        {
            foreach (ObjectiveItemUI item in objectiveItems)
            {
                if (item != null)
                {
                    item.UpdateProgress();
                }
            }
            
            UpdateOverallProgress();
        }
        
        /// <summary>
        /// Получить текст сложности
        /// </summary>
        private string GetDifficultyText(ContractDifficulty difficulty)
        {
            return difficulty switch
            {
                ContractDifficulty.Easy => "Легкая",
                ContractDifficulty.Medium => "Средняя",
                ContractDifficulty.Hard => "Сложная",
                ContractDifficulty.Expert => "Эксперт",
                ContractDifficulty.Master => "Мастер",
                _ => "Неизвестно"
            };
        }
        
        /// <summary>
        /// Показать/скрыть панель целей
        /// </summary>
        public void ToggleObjectivesPanel()
        {
            if (objectivesPanel != null)
            {
                objectivesPanelVisible = !objectivesPanelVisible;
                objectivesPanel.SetActive(objectivesPanelVisible);
            }
        }
        
        /// <summary>
        /// Показать/скрыть панель помощи
        /// </summary>
        public void ToggleHelpPanel()
        {
            if (helpPanel != null)
            {
                helpPanelVisible = !helpPanelVisible;
                helpPanel.SetActive(helpPanelVisible);
                
                if (helpPanelVisible && helpText != null)
                {
                    UpdateHelpText();
                }
            }
        }
        
        /// <summary>
        /// Обновить текст помощи
        /// </summary>
        private void UpdateHelpText()
        {
            if (helpText == null) return;
            
            string helpContent = "Управление:\n";
            helpContent += "• WASD - Движение\n";
            helpContent += "• Мышь - Вращение камеры\n";
            helpContent += "• Пробел - Прыжок\n";
            helpContent += "• E - Взаимодействие\n";
            helpContent += "• ESC - Пауза\n";
            helpContent += "• Tab - Цели\n";
            helpContent += "\nСоветы:\n";
            helpContent += "• Внимательно читайте цели контракта\n";
            helpContent += "• Используйте физику в своих интересах\n";
            helpContent += "• Не бойтесь экспериментировать!";
            
            helpText.text = helpContent;
        }
        
        /// <summary>
        /// Показать уведомление
        /// </summary>
        public void ShowNotification(string message, float duration = 3f)
        {
            // TODO: Реализовать систему уведомлений
            Debug.Log($"Notification: {message}");
        }
        
        /// <summary>
        /// Обработчик кнопки "Пауза"
        /// </summary>
        public void OnPauseButtonClick()
        {
            if (uiManager != null)
            {
                uiManager.PlayButtonClickSound();
                uiManager.ShowPauseMenu();
            }
        }
        
        /// <summary>
        /// Обработчик кнопки "Цели"
        /// </summary>
        public void OnObjectivesButtonClick()
        {
            if (uiManager != null)
            {
                uiManager.PlayButtonClickSound();
                ToggleObjectivesPanel();
            }
        }
        
        /// <summary>
        /// Обработчик кнопки "Помощь"
        /// </summary>
        public void OnHelpButtonClick()
        {
            if (uiManager != null)
            {
                uiManager.PlayButtonClickSound();
                ToggleHelpPanel();
            }
        }
        
        /// <summary>
        /// Обработка нажатия клавиши Escape
        /// </summary>
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                OnPauseButtonClick();
            }
            
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                OnObjectivesButtonClick();
            }
            
            // Обновление времени
            if (currentContract != null)
            {
                UpdateTime(currentContract.GetElapsedTime());
            }
        }
        
        /// <summary>
        /// Получить текущий контракт
        /// </summary>
        public Contract GetCurrentContract()
        {
            return currentContract;
        }
        
        /// <summary>
        /// Проверить, видима ли панель целей
        /// </summary>
        public bool IsObjectivesPanelVisible()
        {
            return objectivesPanelVisible;
        }
        
        /// <summary>
        /// Проверить, видима ли панель помощи
        /// </summary>
        public bool IsHelpPanelVisible()
        {
            return helpPanelVisible;
        }
    }
}
