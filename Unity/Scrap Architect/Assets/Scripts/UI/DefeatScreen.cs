using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using ScrapArchitect.Gameplay;

namespace ScrapArchitect.UI
{
    /// <summary>
    /// Экран поражения
    /// </summary>
    public class DefeatScreen : UIBase
    {
        [Header("Defeat Elements")]
        public TextMeshProUGUI defeatTitleText;
        public TextMeshProUGUI contractTitleText;
        public TextMeshProUGUI failureReasonText;
        public TextMeshProUGUI timeElapsedText;
        
        [Header("Objectives")]
        public Transform objectivesContainer;
        public GameObject objectiveItemPrefab;
        
        [Header("Buttons")]
        public Button retryButton;
        public Button mainMenuButton;
        public Button contractSelectionButton;
        
        [Header("Animation")]
        public float failureAnimationDelay = 0.5f;
        public float failureAnimationDuration = 1f;
        
        private Contract failedContract;
        private string failureReason;
        
        private void Start()
        {
            SetupButtons();
        }
        
        /// <summary>
        /// Настройка кнопок
        /// </summary>
        private void SetupButtons()
        {
            if (retryButton != null)
            {
                retryButton.onClick.AddListener(OnRetryButtonClick);
            }
            
            if (mainMenuButton != null)
            {
                mainMenuButton.onClick.AddListener(OnMainMenuButtonClick);
            }
            
            if (contractSelectionButton != null)
            {
                contractSelectionButton.onClick.AddListener(OnContractSelectionButtonClick);
            }
        }
        
        /// <summary>
        /// Показать экран поражения
        /// </summary>
        public void ShowDefeat(Contract contract, string reason = "Время истекло")
        {
            failedContract = contract;
            failureReason = reason;
            Show();
        }
        
        /// <summary>
        /// Вызывается при показе экрана поражения
        /// </summary>
        protected override void OnShow()
        {
            base.OnShow();
            
            if (failedContract != null)
            {
                UpdateDefeatDisplay();
                StartCoroutine(AnimateFailure());
            }
        }
        
        /// <summary>
        /// Обновить отображение поражения
        /// </summary>
        private void UpdateDefeatDisplay()
        {
            if (failedContract == null) return;
            
            // Заголовок
            if (defeatTitleText != null)
            {
                defeatTitleText.text = "ПОРАЖЕНИЕ";
            }
            
            // Название контракта
            if (contractTitleText != null)
            {
                contractTitleText.text = failedContract.title;
            }
            
            // Причина неудачи
            if (failureReasonText != null)
            {
                failureReasonText.text = $"Причина: {failureReason}";
            }
            
            // Прошедшее время
            if (timeElapsedText != null)
            {
                float elapsedTime = failedContract.GetElapsedTime();
                int minutes = Mathf.FloorToInt(elapsedTime / 60f);
                int seconds = Mathf.FloorToInt(elapsedTime % 60f);
                timeElapsedText.text = $"Прошедшее время: {minutes:00}:{seconds:00}";
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
            foreach (var objective in failedContract.objectives)
            {
                GameObject objectiveItemObj = Instantiate(objectiveItemPrefab, objectivesContainer);
                ObjectiveItemUI objectiveItem = objectiveItemObj.GetComponent<ObjectiveItemUI>();
                
                if (objectiveItem != null)
                {
                    bool isCompleted = objective.IsCompleted();
                    objectiveItem.Initialize(objective, isCompleted);
                }
            }
        }
        
        /// <summary>
        /// Анимация поражения
        /// </summary>
        private IEnumerator AnimateFailure()
        {
            yield return new WaitForSecondsRealtime(failureAnimationDelay);
            
            // Анимация заголовка
            if (defeatTitleText != null)
            {
                yield return StartCoroutine(AnimateFailureText(defeatTitleText));
            }
            
            // Анимация причины
            if (failureReasonText != null)
            {
                yield return StartCoroutine(AnimateFailureText(failureReasonText));
            }
        }
        
        /// <summary>
        /// Анимация текста поражения
        /// </summary>
        private IEnumerator AnimateFailureText(TextMeshProUGUI text)
        {
            if (text == null) yield break;
            
            Color originalColor = text.color;
            Vector3 originalScale = text.transform.localScale;
            
            // Начальное состояние
            text.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
            text.transform.localScale = originalScale * 0.5f;
            
            float elapsedTime = 0f;
            
            while (elapsedTime < failureAnimationDuration)
            {
                elapsedTime += Time.unscaledDeltaTime;
                float progress = elapsedTime / failureAnimationDuration;
                float curveValue = showAnimationCurve.Evaluate(progress);
                
                text.color = new Color(originalColor.r, originalColor.g, originalColor.b, curveValue);
                text.transform.localScale = Vector3.Lerp(originalScale * 0.5f, originalScale, curveValue);
                
                yield return null;
            }
            
            text.color = originalColor;
            text.transform.localScale = originalScale;
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
                if (failedContract != null && ContractManager.Instance != null)
                {
                    ContractManager.Instance.AcceptContract(failedContract);
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
        /// Обработчик кнопки "Выбор контрактов"
        /// </summary>
        public void OnContractSelectionButtonClick()
        {
            if (uiManager != null)
            {
                uiManager.PlayButtonClickSound();
                uiManager.ShowContractSelection();
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
