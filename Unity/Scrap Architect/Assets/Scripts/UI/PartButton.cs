using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System;

namespace ScrapArchitect.UI
{
    /// <summary>
    /// Кнопка детали в панели строительства
    /// </summary>
    public class PartButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [Header("UI Elements")]
        public Image partIcon;
        public TextMeshProUGUI partNameText;
        public TextMeshProUGUI costText;
        public Image backgroundImage;
        public Button button;
        
        [Header("Visual States")]
        public Color normalColor = Color.white;
        public Color hoverColor = Color.yellow;
        public Color selectedColor = Color.green;
        public Color lockedColor = Color.gray;
        
        // Данные детали
        private PartData partData;
        private bool isLocked = false;
        private bool isSelected = false;
        
        // Events
        public Action<PartData> OnPartSelected;
        
        private void Awake()
        {
            // Получаем компоненты если не назначены
            if (partIcon == null)
                partIcon = transform.Find("Icon")?.GetComponent<Image>();
            
            if (partNameText == null)
                partNameText = transform.Find("Name")?.GetComponent<TextMeshProUGUI>();
            
            if (costText == null)
                costText = transform.Find("Cost")?.GetComponent<TextMeshProUGUI>();
            
            if (backgroundImage == null)
                backgroundImage = GetComponent<Image>();
            
            if (button == null)
                button = GetComponent<Button>();
        }
        
        /// <summary>
        /// Инициализация кнопки детали
        /// </summary>
        public void Initialize(PartData data)
        {
            partData = data;
            
            // Устанавливаем текст
            if (partNameText != null)
            {
                partNameText.text = data.partName;
            }
            
            if (costText != null)
            {
                costText.text = $"${data.cost:F0}";
            }
            
            // Устанавливаем иконку
            if (partIcon != null && data.icon != null)
            {
                partIcon.sprite = data.icon;
                partIcon.color = normalColor;
            }
            
            // Проверяем доступность детали
            CheckAvailability();
            
            // Устанавливаем нормальный цвет
            SetVisualState(ButtonState.Normal);
        }
        
        /// <summary>
        /// Проверка доступности детали
        /// </summary>
        private void CheckAvailability()
        {
            // TODO: Проверить уровень игрока и деньги
            isLocked = false; // Пока все детали доступны
            
            if (button != null)
            {
                button.interactable = !isLocked;
            }
        }
        
        /// <summary>
        /// Установка визуального состояния
        /// </summary>
        private void SetVisualState(ButtonState state)
        {
            Color targetColor = normalColor;
            
            switch (state)
            {
                case ButtonState.Normal:
                    targetColor = isLocked ? lockedColor : normalColor;
                    break;
                    
                case ButtonState.Hover:
                    targetColor = isLocked ? lockedColor : hoverColor;
                    break;
                    
                case ButtonState.Selected:
                    targetColor = selectedColor;
                    break;
            }
            
            if (backgroundImage != null)
            {
                backgroundImage.color = targetColor;
            }
            
            if (partIcon != null)
            {
                partIcon.color = targetColor;
            }
        }
        
        /// <summary>
        /// Обработчик клика по кнопке
        /// </summary>
        public void OnPointerClick(PointerEventData eventData)
        {
            if (isLocked)
            {
                ShowLockedMessage();
                return;
            }
            
            // Вызываем событие выбора детали
            OnPartSelected?.Invoke(partData);
            
            // Устанавливаем состояние выбранной
            isSelected = true;
            SetVisualState(ButtonState.Selected);
            
            Debug.Log($"Part button clicked: {partData.partName}");
        }
        
        /// <summary>
        /// Обработчик наведения мыши
        /// </summary>
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!isSelected)
            {
                SetVisualState(ButtonState.Hover);
            }
            
            // Показываем подсказку
            ShowTooltip();
        }
        
        /// <summary>
        /// Обработчик ухода мыши
        /// </summary>
        public void OnPointerExit(PointerEventData eventData)
        {
            if (!isSelected)
            {
                SetVisualState(ButtonState.Normal);
            }
            
            // Скрываем подсказку
            HideTooltip();
        }
        
        /// <summary>
        /// Показать подсказку
        /// </summary>
        private void ShowTooltip()
        {
            if (UIManager.Instance != null && partData != null)
            {
                string tooltipText = $"{partData.partName}\nType: {partData.partType}\nCost: ${partData.cost:F0}";
                
                if (isLocked)
                {
                    tooltipText += "\n[LOCKED]";
                }
                
                UIManager.Instance.ShowPartInfo(partData.partName, tooltipText);
            }
        }
        
        /// <summary>
        /// Скрыть подсказку
        /// </summary>
        private void HideTooltip()
        {
            if (UIManager.Instance != null)
            {
                UIManager.Instance.HidePartInfo();
            }
        }
        
        /// <summary>
        /// Показать сообщение о заблокированной детали
        /// </summary>
        private void ShowLockedMessage()
        {
            Debug.Log($"Part {partData.partName} is locked!");
            
            // TODO: Показать UI сообщение о том, что деталь заблокирована
        }
        
        /// <summary>
        /// Сбросить состояние выбора
        /// </summary>
        public void Deselect()
        {
            isSelected = false;
            SetVisualState(ButtonState.Normal);
        }
        
        /// <summary>
        /// Установить заблокированное состояние
        /// </summary>
        public void SetLocked(bool locked)
        {
            isLocked = locked;
            CheckAvailability();
            SetVisualState(ButtonState.Normal);
        }
        
        /// <summary>
        /// Получить данные детали
        /// </summary>
        public PartData GetPartData()
        {
            return partData;
        }
    }
    
}
