using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using ScrapArchitect.Gameplay;

namespace ScrapArchitect.UI
{
    /// <summary>
    /// Экран победы
    /// </summary>
    public class VictoryScreen : UIBase
    {
        [Header("Victory Elements")]
        public TextMeshProUGUI victoryTitleText;
        public TextMeshProUGUI contractTitleText;
        public TextMeshProUGUI completionTimeText;
        public TextMeshProUGUI scoreText;
        
        [Header("Rewards")]
        public TextMeshProUGUI scrapRewardText;
        public TextMeshProUGUI experienceRewardText;
        public GameObject[] rewardIcons;
        
        [Header("Objectives")]
        public Transform objectivesContainer;
        public GameObject objectiveItemPrefab;
        
        [Header("Buttons")]
        public Button continueButton;
        public Button retryButton;
        public Button mainMenuButton;
        
        [Header("Animation")]
        public float rewardAnimationDelay = 0.5f;
        public float rewardAnimationDuration = 1f;
        
        private Contract completedContract;
        
        private void Start()
        {
            SetupButtons();
        }
        
        /// <summary>
        /// Настройка кнопок
        /// </summary>
        private void SetupButtons()
        {
            if (continueButton != null)
            {
                continueButton.onClick.AddListener(OnContinueButtonClick);
            }
            
            if (retryButton != null)
            {
                retryButton.onClick.AddListener(OnRetryButtonClick);
            }
            
            if (mainMenuButton != null)
            {
                mainMenuButton.onClick.AddListener(OnMainMenuButtonClick);
            }
        }
        
        /// <summary>
        /// Показать экран победы
        /// </summary>
        public void ShowVictory(Contract contract)
        {
            completedContract = contract;
            Show();
        }
        
        /// <summary>
        /// Вызывается при показе экрана победы
        /// </summary>
        protected override void OnShow()
        {
            base.OnShow();
            
            if (completedContract != null)
            {
                UpdateVictoryDisplay();
                StartCoroutine(AnimateRewards());
            }
        }
        
        /// <summary>
        /// Обновить отображение победы
        /// </summary>
        private void UpdateVictoryDisplay()
        {
            if (completedContract == null) return;
            
            // Заголовок
            if (victoryTitleText != null)
            {
                victoryTitleText.text = "ПОБЕДА!";
            }
            
            // Название контракта
            if (contractTitleText != null)
            {
                contractTitleText.text = completedContract.title;
            }
            
            // Время выполнения
            if (completionTimeText != null)
            {
                float completionTime = completedContract.GetCompletionTime();
                int minutes = Mathf.FloorToInt(completionTime / 60f);
                int seconds = Mathf.FloorToInt(completionTime % 60f);
                completionTimeText.text = $"Время выполнения: {minutes:00}:{seconds:00}";
            }
            
            // Очки
            if (scoreText != null)
            {
                int score = CalculateScore();
                scoreText.text = $"Очки: {score}";
            }
            
            // Награды
            if (scrapRewardText != null)
            {
                scrapRewardText.text = $"+{completedContract.reward.scrapReward}";
            }
            
            if (experienceRewardText != null)
            {
                experienceRewardText.text = $"+{completedContract.reward.experienceReward}";
            }
            
            // Цели
            UpdateObjectivesDisplay();
        }
        
        /// <summary>
        /// Обновить отображение целей
        /// </summary>
        private void UpdateObjectivesDisplay()
        {
            if (objectivesContainer == null || objectiveItemPrefab == null) return;
            
            // Очистить существующие элементы
            foreach (Transform child in objectivesContainer)
            {
                Destroy(child.gameObject);
            }
            
            // Создать элементы для каждой цели
            foreach (var objective in completedContract.objectives)
            {
                GameObject objectiveItemObj = Instantiate(objectiveItemPrefab, objectivesContainer);
                ObjectiveItemUI objectiveItem = objectiveItemObj.GetComponent<ObjectiveItemUI>();
                
                if (objectiveItem != null)
                {
                    objectiveItem.Initialize(objective, true);
                }
            }
        }
        
        /// <summary>
        /// Рассчитать очки
        /// </summary>
        private int CalculateScore()
        {
            if (completedContract == null) return 0;
            
            int baseScore = completedContract.reward.scrapReward;
            float timeBonus = Mathf.Max(0, 1000 - completedContract.GetCompletionTime() * 10);
            float difficultyMultiplier = completedContract.GetDifficultyMultiplier();
            
            return Mathf.RoundToInt((baseScore + timeBonus) * difficultyMultiplier);
        }
        
        /// <summary>
        /// Анимация наград
        /// </summary>
        private IEnumerator AnimateRewards()
        {
            yield return new WaitForSecondsRealtime(rewardAnimationDelay);
            
            // Анимация скрапа
            if (scrapRewardText != null)
            {
                yield return StartCoroutine(AnimateRewardText(scrapRewardText));
            }
            
            // Анимация опыта
            if (experienceRewardText != null)
            {
                yield return StartCoroutine(AnimateRewardText(experienceRewardText));
            }
            
            // Анимация иконок наград
            yield return StartCoroutine(AnimateRewardIcons());
        }
        
        /// <summary>
        /// Анимация текста награды
        /// </summary>
        private IEnumerator AnimateRewardText(TextMeshProUGUI rewardText)
        {
            if (rewardText == null) yield break;
            
            Color originalColor = rewardText.color;
            Vector3 originalScale = rewardText.transform.localScale;
            
            // Начальное состояние
            rewardText.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
            rewardText.transform.localScale = originalScale * 0.5f;
            
            float elapsedTime = 0f;
            
            while (elapsedTime < rewardAnimationDuration)
            {
                elapsedTime += Time.unscaledDeltaTime;
                float progress = elapsedTime / rewardAnimationDuration;
                float curveValue = showAnimationCurve.Evaluate(progress);
                
                rewardText.color = new Color(originalColor.r, originalColor.g, originalColor.b, curveValue);
                rewardText.transform.localScale = Vector3.Lerp(originalScale * 0.5f, originalScale, curveValue);
                
                yield return null;
            }
            
            rewardText.color = originalColor;
            rewardText.transform.localScale = originalScale;
        }
        
        /// <summary>
        /// Анимация иконок наград
        /// </summary>
        private IEnumerator AnimateRewardIcons()
        {
            if (rewardIcons == null) yield break;
            
            foreach (GameObject icon in rewardIcons)
            {
                if (icon != null)
                {
                    yield return StartCoroutine(AnimateRewardIcon(icon));
                    yield return new WaitForSecondsRealtime(0.2f);
                }
            }
        }
        
        /// <summary>
        /// Анимация иконки награды
        /// </summary>
        private IEnumerator AnimateRewardIcon(GameObject icon)
        {
            if (icon == null) yield break;
            
            CanvasGroup iconCanvasGroup = icon.GetComponent<CanvasGroup>();
            if (iconCanvasGroup == null)
            {
                iconCanvasGroup = icon.AddComponent<CanvasGroup>();
            }
            
            Vector3 originalScale = icon.transform.localScale;
            
            // Начальное состояние
            iconCanvasGroup.alpha = 0f;
            icon.transform.localScale = originalScale * 0.5f;
            
            float elapsedTime = 0f;
            float animationDuration = 0.5f;
            
            while (elapsedTime < animationDuration)
            {
                elapsedTime += Time.unscaledDeltaTime;
                float progress = elapsedTime / animationDuration;
                float curveValue = showAnimationCurve.Evaluate(progress);
                
                iconCanvasGroup.alpha = curveValue;
                icon.transform.localScale = Vector3.Lerp(originalScale * 0.5f, originalScale, curveValue);
                
                yield return null;
            }
            
            iconCanvasGroup.alpha = 1f;
            icon.transform.localScale = originalScale;
        }
        
        /// <summary>
        /// Обработчик кнопки "Продолжить"
        /// </summary>
        public void OnContinueButtonClick()
        {
            if (uiManager != null)
            {
                uiManager.PlayButtonClickSound();
                uiManager.ShowContractSelection();
            }
        }
        
        /// <summary>
        /// Обработчик кнопки "Повторить"
        /// </summary>
        public void OnRetryButtonClick()
        {
            if (uiManager != null)
            {
                uiManager.PlayButtonClickSound();
                
                // Повторить тот же контракт
                if (completedContract != null && ContractManager.Instance != null)
                {
                    ContractManager.Instance.AcceptContract(completedContract);
                    uiManager.ShowGameplayUI();
                }
            }
        }
        
        /// <summary>
        /// Обработчик кнопки "Главное меню"
        /// </summary>
        public void OnMainMenuButtonClick()
        {
            if (uiManager != null)
            {
                uiManager.PlayButtonClickSound();
                uiManager.ShowMainMenu();
            }
        }
        
        /// <summary>
        /// Обработка нажатия клавиши Escape
        /// </summary>
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                OnMainMenuButtonClick();
            }
        }
    }
}
