using UnityEngine;
using UnityEngine.UI;
using TMPro;
using ScrapArchitect.Gameplay;

namespace ScrapArchitect.UI
{
    /// <summary>
    /// Элемент цели контракта
    /// </summary>
    public class ObjectiveItemUI : MonoBehaviour
    {
        [Header("Objective Elements")]
        public TextMeshProUGUI descriptionText;
        public TextMeshProUGUI progressText;
        
        [Header("Visual Elements")]
        public Image backgroundImage;
        public Image checkmarkIcon;
        public Image crossIcon;
        public Slider progressSlider;
        
        [Header("Colors")]
        public Color incompleteColor = Color.gray;
        public Color completeColor = Color.green;
        public Color failedColor = Color.red;
        
        private ContractObjective objective;
        private bool isCompleted;
        
        private void Start()
        {
            UpdateVisuals();
        }
        
        /// <summary>
        /// Инициализация элемента цели
        /// </summary>
        public void Initialize(ContractObjective objectiveData, bool completed = false)
        {
            objective = objectiveData;
            isCompleted = completed;
            
            UpdateDisplay();
        }
        
        /// <summary>
        /// Обновить отображение
        /// </summary>
        private void UpdateDisplay()
        {
            if (objective == null) return;
            
            // Описание
            if (descriptionText != null)
            {
                descriptionText.text = objective.description;
            }
            
            // Прогресс
            if (progressText != null)
            {
                float progress = objective.currentValue;
                float targetValue = objective.targetValue;
                
                if (targetValue > 1f)
                {
                    progressText.text = $"{progress:F0}/{targetValue:F0}";
                }
                else
                {
                    progressText.text = isCompleted ? "Выполнено" : "Не выполнено";
                }
            }
            
            // Слайдер прогресса
            if (progressSlider != null)
            {
                progressSlider.value = objective.GetProgressPercentage();
            }
            
            UpdateVisuals();
        }
        
        /// <summary>
        /// Обновить визуальные элементы
        /// </summary>
        private void UpdateVisuals()
        {
            if (objective == null) return;
            
            Color targetColor = incompleteColor;
            
            if (isCompleted)
            {
                targetColor = completeColor;
            }
            else if (objective.isCompleted == false && objective.currentValue > 0)
            {
                targetColor = failedColor;
            }
            
            // Цвет фона
            if (backgroundImage != null)
            {
                backgroundImage.color = targetColor;
            }
            
            // Иконки
            if (checkmarkIcon != null)
            {
                checkmarkIcon.gameObject.SetActive(isCompleted);
            }
            
            if (crossIcon != null)
            {
                crossIcon.gameObject.SetActive(objective.isCompleted == false && objective.currentValue > 0);
            }
        }
        
        /// <summary>
        /// Обновить прогресс
        /// </summary>
        public void UpdateProgress()
        {
            if (objective != null)
            {
                isCompleted = objective.isCompleted;
                UpdateDisplay();
            }
        }
        
        /// <summary>
        /// Получить цель
        /// </summary>
        public ContractObjective GetObjective()
        {
            return objective;
        }
        
        /// <summary>
        /// Проверить, выполнена ли цель
        /// </summary>
        public bool IsCompleted()
        {
            return isCompleted;
        }
        
        /// <summary>
        /// Проверить, провалена ли цель
        /// </summary>
        public bool IsFailed()
        {
            return objective != null && objective.isCompleted == false && objective.currentValue > 0;
        }
    }
}
