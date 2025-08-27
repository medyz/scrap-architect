using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections;
using System.Collections.Generic;

namespace ScrapArchitect.UI
{
    /// <summary>
    /// Система туториала для новичков
    /// </summary>
    public class TutorialSystem : MonoBehaviour
    {
        // Singleton pattern
        public static TutorialSystem Instance { get; private set; }
        
        [Header("Tutorial UI")]
        public GameObject tutorialPanel;
        public TextMeshProUGUI tutorialTitleText;
        public TextMeshProUGUI tutorialDescriptionText;
        public Button nextButton;
        public Button previousButton;
        public Button skipButton;
        public Button closeButton;
        
        [Header("Tutorial Steps")]
        public List<TutorialStep> tutorialSteps = new List<TutorialStep>();
        
        [Header("Tutorial Settings")]
        public bool showTutorialOnFirstLaunch = true;
        public bool enableTooltips = true;
        public float tooltipDelay = 2f;
        
        private int currentStepIndex = 0;
        private bool isTutorialActive = false;
        private bool hasCompletedTutorial = false;
        
        [Serializable]
        public class TutorialStep
        {
            public string title;
            public string description;
            public Sprite image;
            public string targetUIElement;
            public bool highlightElement;
            public bool requireInteraction;
            public string[] tips;
        }
        
        private void Awake()
        {
            // Singleton pattern
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                InitializeTutorialSystem();
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
        /// <summary>
        /// Инициализация системы туториала
        /// </summary>
        private void InitializeTutorialSystem()
        {
            LoadTutorialProgress();
            SetupTutorialSteps();
            SetupButtons();
            
            if (showTutorialOnFirstLaunch && !hasCompletedTutorial)
            {
                StartCoroutine(ShowTutorialOnDelay());
            }
            
            Debug.Log("TutorialSystem initialized");
        }
        
        /// <summary>
        /// Загрузка прогресса туториала
        /// </summary>
        private void LoadTutorialProgress()
        {
            hasCompletedTutorial = PlayerPrefs.GetInt("TutorialCompleted", 0) == 1;
        }
        
        /// <summary>
        /// Сохранение прогресса туториала
        /// </summary>
        private void SaveTutorialProgress()
        {
            PlayerPrefs.SetInt("TutorialCompleted", hasCompletedTutorial ? 1 : 0);
            PlayerPrefs.Save();
        }
        
        /// <summary>
        /// Настройка шагов туториала
        /// </summary>
        private void SetupTutorialSteps()
        {
            tutorialSteps.Clear();
            
            // Шаг 1: Добро пожаловать
            tutorialSteps.Add(new TutorialStep
            {
                title = "Добро пожаловать в Scrap Architect!",
                description = "В этой игре вы будете создавать безумные машины из хлама и выполнять различные контракты. Давайте начнем с основ!",
                tips = new string[] { "Используйте мышь для навигации", "Нажмите 'Далее' для продолжения" }
            });
            
            // Шаг 2: Главное меню
            tutorialSteps.Add(new TutorialStep
            {
                title = "Главное меню",
                description = "Здесь вы можете начать новую игру, открыть карту мира, настроить игру или выйти.",
                targetUIElement = "MainMenuPanel",
                highlightElement = true,
                tips = new string[] { "Нажмите 'Играть' для начала", "Карта мира покажет доступные контракты" }
            });
            
            // Шаг 3: Карта мира
            tutorialSteps.Add(new TutorialStep
            {
                title = "Карта мира",
                description = "На карте мира вы увидите различные локации с доступными контрактами. Кликните на локацию, чтобы увидеть контракты.",
                targetUIElement = "WorldMapPanel",
                highlightElement = true,
                tips = new string[] { "Используйте колесико мыши для зума", "Перетаскивайте карту для навигации" }
            });
            
            // Шаг 4: Выбор контракта
            tutorialSteps.Add(new TutorialStep
            {
                title = "Выбор контракта",
                description = "Каждый контракт имеет свои цели, награды и ограничения по времени. Выберите подходящий для вас!",
                targetUIElement = "ContractSelectionPanel",
                highlightElement = true,
                tips = new string[] { "Обратите внимание на сложность", "Проверьте награды перед принятием" }
            });
            
            // Шаг 5: Строительство
            tutorialSteps.Add(new TutorialStep
            {
                title = "Строительство машины",
                description = "Перетаскивайте детали из панели слева на рабочую область. Соединяйте их для создания функциональной машины.",
                targetUIElement = "BuildingPanel",
                highlightElement = true,
                tips = new string[] { "Используйте Snap Points для соединения", "Правая кнопка мыши для поворота деталей" }
            });
            
            // Шаг 6: Детали
            tutorialSteps.Add(new TutorialStep
            {
                title = "Типы деталей",
                description = "Двигатели приводят машину в движение, колеса обеспечивают сцепление, а инструменты помогают выполнять задачи.",
                targetUIElement = "PartsPanel",
                highlightElement = true,
                tips = new string[] { "Двигатели требуют топливо", "Разные колеса для разных поверхностей" }
            });
            
            // Шаг 7: Тестирование
            tutorialSteps.Add(new TutorialStep
            {
                title = "Тестирование машины",
                description = "Нажмите 'Тест' чтобы проверить вашу машину. Используйте WASD для управления.",
                targetUIElement = "TestButton",
                highlightElement = true,
                tips = new string[] { "Тестируйте перед принятием контракта", "Используйте камеру для лучшего обзора" }
            });
            
            // Шаг 8: Выполнение контракта
            tutorialSteps.Add(new TutorialStep
            {
                title = "Выполнение контракта",
                description = "Управляйте машиной для выполнения целей контракта. Следите за временем и состоянием машины!",
                targetUIElement = "GameplayPanel",
                highlightElement = true,
                tips = new string[] { "Следите за временем", "Выполняйте бонусные цели для звезд" }
            });
            
            // Шаг 9: Звездный рейтинг
            tutorialSteps.Add(new TutorialStep
            {
                title = "Звездный рейтинг",
                description = "За быстрое и качественное выполнение вы получите звезды. Больше звезд = больше наград!",
                targetUIElement = "StarRatingPanel",
                highlightElement = true,
                tips = new string[] { "3 звезды = максимальная награда", "Повторяйте контракты для улучшения результата" }
            });
            
            // Шаг 10: Завершение
            tutorialSteps.Add(new TutorialStep
            {
                title = "Готово к игре!",
                description = "Теперь вы знаете основы Scrap Architect! Создавайте удивительные машины и покоряйте мир!",
                tips = new string[] { "Экспериментируйте с разными комбинациями", "Присоединяйтесь к сообществу игроков" }
            });
        }
        
        /// <summary>
        /// Настройка кнопок
        /// </summary>
        private void SetupButtons()
        {
            if (nextButton != null)
            {
                nextButton.onClick.AddListener(OnNextButtonClick);
            }
            
            if (previousButton != null)
            {
                previousButton.onClick.AddListener(OnPreviousButtonClick);
            }
            
            if (skipButton != null)
            {
                skipButton.onClick.AddListener(OnSkipButtonClick);
            }
            
            if (closeButton != null)
            {
                closeButton.onClick.AddListener(OnCloseButtonClick);
            }
        }
        
        /// <summary>
        /// Показать туториал с задержкой
        /// </summary>
        private IEnumerator ShowTutorialOnDelay()
        {
            yield return new WaitForSeconds(1f);
            ShowTutorial();
        }
        
        /// <summary>
        /// Показать туториал
        /// </summary>
        public void ShowTutorial()
        {
            if (isTutorialActive) return;
            
            isTutorialActive = true;
            currentStepIndex = 0;
            
            if (tutorialPanel != null)
            {
                tutorialPanel.SetActive(true);
            }
            
            ShowCurrentStep();
        }
        
        /// <summary>
        /// Скрыть туториал
        /// </summary>
        public void HideTutorial()
        {
            if (!isTutorialActive) return;
            
            isTutorialActive = false;
            
            if (tutorialPanel != null)
            {
                tutorialPanel.SetActive(false);
            }
            
            // Снимаем подсветку с элементов
            ClearHighlights();
        }
        
        /// <summary>
        /// Показать текущий шаг
        /// </summary>
        private void ShowCurrentStep()
        {
            if (currentStepIndex < 0 || currentStepIndex >= tutorialSteps.Count)
            {
                HideTutorial();
                return;
            }
            
            TutorialStep step = tutorialSteps[currentStepIndex];
            
            if (tutorialTitleText != null)
            {
                tutorialTitleText.text = step.title;
            }
            
            if (tutorialDescriptionText != null)
            {
                tutorialDescriptionText.text = step.description;
            }
            
            // Обновляем состояние кнопок
            UpdateButtonStates();
            
            // Подсвечиваем элемент, если нужно
            if (step.highlightElement && !string.IsNullOrEmpty(step.targetUIElement))
            {
                HighlightUIElement(step.targetUIElement);
            }
            
            // Показываем подсказки
            ShowTips(step.tips);
        }
        
        /// <summary>
        /// Обновить состояние кнопок
        /// </summary>
        private void UpdateButtonStates()
        {
            if (previousButton != null)
            {
                previousButton.interactable = currentStepIndex > 0;
            }
            
            if (nextButton != null)
            {
                bool isLastStep = currentStepIndex >= tutorialSteps.Count - 1;
                nextButton.GetComponentInChildren<TextMeshProUGUI>().text = isLastStep ? "Завершить" : "Далее";
            }
        }
        
        /// <summary>
        /// Подсветить UI элемент
        /// </summary>
        private void HighlightUIElement(string elementName)
        {
            // TODO: Реализовать подсветку UI элементов
            Debug.Log($"Highlighting UI element: {elementName}");
        }
        
        /// <summary>
        /// Убрать подсветку
        /// </summary>
        private void ClearHighlights()
        {
            // TODO: Реализовать снятие подсветки
            Debug.Log("Clearing highlights");
        }
        
        /// <summary>
        /// Показать подсказки
        /// </summary>
        private void ShowTips(string[] tips)
        {
            if (tips == null || tips.Length == 0) return;
            
            string tipsText = "Советы:\n";
            foreach (string tip in tips)
            {
                tipsText += $"• {tip}\n";
            }
            
            Debug.Log(tipsText);
            // TODO: Показать подсказки в UI
        }
        
        /// <summary>
        /// Следующий шаг
        /// </summary>
        public void NextStep()
        {
            if (currentStepIndex < tutorialSteps.Count - 1)
            {
                currentStepIndex++;
                ShowCurrentStep();
            }
            else
            {
                CompleteTutorial();
            }
        }
        
        /// <summary>
        /// Предыдущий шаг
        /// </summary>
        public void PreviousStep()
        {
            if (currentStepIndex > 0)
            {
                currentStepIndex--;
                ShowCurrentStep();
            }
        }
        
        /// <summary>
        /// Завершить туториал
        /// </summary>
        public void CompleteTutorial()
        {
            hasCompletedTutorial = true;
            SaveTutorialProgress();
            HideTutorial();
            
            Debug.Log("Tutorial completed!");
        }
        
        /// <summary>
        /// Пропустить туториал
        /// </summary>
        public void SkipTutorial()
        {
            HideTutorial();
            Debug.Log("Tutorial skipped");
        }
        
        /// <summary>
        /// Сбросить прогресс туториала
        /// </summary>
        public void ResetTutorialProgress()
        {
            hasCompletedTutorial = false;
            SaveTutorialProgress();
            Debug.Log("Tutorial progress reset");
        }
        
        /// <summary>
        /// Показать подсказку для элемента
        /// </summary>
        public void ShowTooltip(string elementName, string tooltipText)
        {
            if (!enableTooltips) return;
            
            StartCoroutine(ShowTooltipCoroutine(elementName, tooltipText));
        }
        
        /// <summary>
        /// Корутина для показа подсказки
        /// </summary>
        private IEnumerator ShowTooltipCoroutine(string elementName, string tooltipText)
        {
            yield return new WaitForSeconds(tooltipDelay);
            
            // TODO: Показать подсказку в UI
            Debug.Log($"Tooltip for {elementName}: {tooltipText}");
        }
        
        // Обработчики кнопок
        private void OnNextButtonClick()
        {
            NextStep();
        }
        
        private void OnPreviousButtonClick()
        {
            PreviousStep();
        }
        
        private void OnSkipButtonClick()
        {
            SkipTutorial();
        }
        
        private void OnCloseButtonClick()
        {
            HideTutorial();
        }
        
        /// <summary>
        /// Проверить, завершен ли туториал
        /// </summary>
        public bool IsTutorialCompleted()
        {
            return hasCompletedTutorial;
        }
        
        /// <summary>
        /// Проверить, активен ли туториал
        /// </summary>
        public bool IsTutorialActive()
        {
            return isTutorialActive;
        }
    }
}
