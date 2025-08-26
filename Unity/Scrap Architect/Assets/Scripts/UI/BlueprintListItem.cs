using UnityEngine;
using UnityEngine.UI;
using TMPro;
using ScrapArchitect.System;

namespace ScrapArchitect.UI
{
    /// <summary>
    /// UI элемент для отображения blueprint в списке
    /// </summary>
    public class BlueprintListItem : MonoBehaviour
    {
        [Header("UI References")]
        public TextMeshProUGUI nameText;
        public TextMeshProUGUI infoText;
        public TextMeshProUGUI dateText;
        public Image thumbnailImage;
        public Button loadButton;
        public Button deleteButton;
        public Button exportButton;
        public Button infoButton;
        
        [Header("Visual Settings")]
        public Color normalColor = Color.white;
        public Color selectedColor = Color.yellow;
        public Color hoverColor = Color.cyan;
        
        [Header("Audio Settings")]
        public AudioClip buttonClickSound;
        public AudioClip hoverSound;
        
        private VehicleBlueprint blueprint;
        private BlueprintManager blueprintManager;
        private bool isSelected = false;
        
        private void Start()
        {
            SetupButtons();
        }
        
        /// <summary>
        /// Инициализация элемента списка
        /// </summary>
        public void Initialize(VehicleBlueprint blueprint, BlueprintManager manager)
        {
            this.blueprint = blueprint;
            this.blueprintManager = manager;
            
            UpdateUI();
        }
        
        /// <summary>
        /// Настройка кнопок
        /// </summary>
        private void SetupButtons()
        {
            if (loadButton != null)
            {
                loadButton.onClick.AddListener(OnLoadButtonClicked);
            }
            
            if (deleteButton != null)
            {
                deleteButton.onClick.AddListener(OnDeleteButtonClicked);
            }
            
            if (exportButton != null)
            {
                exportButton.onClick.AddListener(OnExportButtonClicked);
            }
            
            if (infoButton != null)
            {
                infoButton.onClick.AddListener(OnInfoButtonClicked);
            }
        }
        
        /// <summary>
        /// Обновить UI
        /// </summary>
        private void UpdateUI()
        {
            if (blueprint == null)
            {
                return;
            }
            
            // Название
            if (nameText != null)
            {
                nameText.text = blueprint.blueprintName;
            }
            
            // Информация
            if (infoText != null)
            {
                infoText.text = $"Parts: {blueprint.totalParts} | Mass: {blueprint.totalMass:F1} | Connections: {blueprint.connections.Count}";
            }
            
            // Дата
            if (dateText != null)
            {
                dateText.text = blueprint.lastModified.ToString("yyyy-MM-dd HH:mm");
            }
            
            // Миниатюра (если есть)
            if (thumbnailImage != null)
            {
                // В будущем можно добавить генерацию миниатюр
                UpdateThumbnail();
            }
        }
        
        /// <summary>
        /// Обновить миниатюру
        /// </summary>
        private void UpdateThumbnail()
        {
            // Пока что используем цвет в зависимости от типа деталей
            Color thumbnailColor = GetThumbnailColor();
            thumbnailImage.color = thumbnailColor;
        }
        
        /// <summary>
        /// Получить цвет миниатюры
        /// </summary>
        private Color GetThumbnailColor()
        {
            if (blueprint == null || blueprint.parts.Count == 0)
            {
                return Color.gray;
            }
            
            // Определить основной тип деталей
            int blockCount = 0;
            int wheelCount = 0;
            int motorCount = 0;
            int seatCount = 0;
            int toolCount = 0;
            
            foreach (var part in blueprint.parts)
            {
                switch (part.partType.ToLower())
                {
                    case "block":
                        blockCount++;
                        break;
                    case "wheel":
                        wheelCount++;
                        break;
                    case "motor":
                        motorCount++;
                        break;
                    case "seat":
                        seatCount++;
                        break;
                    case "tool":
                        toolCount++;
                        break;
                }
            }
            
            // Определить доминирующий тип
            if (motorCount > 0)
            {
                return Color.red; // Двигатели - красный
            }
            else if (wheelCount > 0)
            {
                return Color.black; // Колеса - черный
            }
            else if (seatCount > 0)
            {
                return Color.blue; // Сиденья - синий
            }
            else if (toolCount > 0)
            {
                return Color.yellow; // Инструменты - желтый
            }
            else if (blockCount > 0)
            {
                return Color.gray; // Блоки - серый
            }
            
            return Color.white;
        }
        
        /// <summary>
        /// Обработка нажатия кнопки загрузки
        /// </summary>
        private void OnLoadButtonClicked()
        {
            if (blueprintManager != null && blueprint != null)
            {
                PlayButtonClickSound();
                
                // Загрузить blueprint в центр сцены
                Vector3 loadPosition = Vector3.zero;
                Quaternion loadRotation = Quaternion.identity;
                
                GameObject vehicle = blueprintManager.LoadBlueprint(blueprint, loadPosition, loadRotation);
                
                if (vehicle != null)
                {
                    Debug.Log($"Loaded blueprint: {blueprint.blueprintName}");
                }
            }
        }
        
        /// <summary>
        /// Обработка нажатия кнопки удаления
        /// </summary>
        private void OnDeleteButtonClicked()
        {
            if (blueprintManager != null && blueprint != null)
            {
                PlayButtonClickSound();
                
                // Показать диалог подтверждения
                ShowDeleteConfirmation();
            }
        }
        
        /// <summary>
        /// Обработка нажатия кнопки экспорта
        /// </summary>
        private void OnExportButtonClicked()
        {
            if (blueprintManager != null && blueprint != null)
            {
                PlayButtonClickSound();
                
                // Экспортировать blueprint
                string exportPath = GetExportPath();
                bool success = blueprintManager.ExportBlueprint(blueprint, exportPath);
                
                if (success)
                {
                    Debug.Log($"Blueprint exported to: {exportPath}");
                    ShowExportSuccessMessage();
                }
                else
                {
                    Debug.LogError("Failed to export blueprint");
                    ShowExportErrorMessage();
                }
            }
        }
        
        /// <summary>
        /// Обработка нажатия кнопки информации
        /// </summary>
        private void OnInfoButtonClicked()
        {
            if (blueprint != null)
            {
                PlayButtonClickSound();
                
                // Показать подробную информацию
                ShowBlueprintInfo();
            }
        }
        
        /// <summary>
        /// Показать диалог подтверждения удаления
        /// </summary>
        private void ShowDeleteConfirmation()
        {
            // В будущем можно добавить UI диалог
            // Пока что удаляем сразу
            if (blueprintManager != null)
            {
                bool success = blueprintManager.DeleteBlueprint(blueprint);
                if (success)
                {
                    // Уничтожить этот элемент списка
                    Destroy(gameObject);
                }
            }
        }
        
        /// <summary>
        /// Показать информацию о blueprint
        /// </summary>
        private void ShowBlueprintInfo()
        {
            if (blueprint == null)
            {
                return;
            }
            
            string info = blueprint.GetBlueprintInfo();
            
            // В будущем можно добавить UI диалог
            Debug.Log($"Blueprint Info:\n{info}");
        }
        
        /// <summary>
        /// Получить путь для экспорта
        /// </summary>
        private string GetExportPath()
        {
            string fileName = $"{blueprint.blueprintName}_{System.DateTime.Now:yyyyMMdd_HHmmss}.json";
            string exportPath = System.IO.Path.Combine(Application.persistentDataPath, "Exports", fileName);
            
            // Создать папку если не существует
            string exportDir = System.IO.Path.GetDirectoryName(exportPath);
            if (!System.IO.Directory.Exists(exportDir))
            {
                System.IO.Directory.CreateDirectory(exportDir);
            }
            
            return exportPath;
        }
        
        /// <summary>
        /// Показать сообщение об успешном экспорте
        /// </summary>
        private void ShowExportSuccessMessage()
        {
            // В будущем можно добавить UI уведомление
            Debug.Log("Blueprint exported successfully!");
        }
        
        /// <summary>
        /// Показать сообщение об ошибке экспорта
        /// </summary>
        private void ShowExportErrorMessage()
        {
            // В будущем можно добавить UI уведомление
            Debug.LogError("Failed to export blueprint!");
        }
        
        /// <summary>
        /// Установить выбранное состояние
        /// </summary>
        public void SetSelected(bool selected)
        {
            isSelected = selected;
            UpdateVisualState();
        }
        
        /// <summary>
        /// Обновить визуальное состояние
        /// </summary>
        private void UpdateVisualState()
        {
            Image backgroundImage = GetComponent<Image>();
            if (backgroundImage != null)
            {
                if (isSelected)
                {
                    backgroundImage.color = selectedColor;
                }
                else
                {
                    backgroundImage.color = normalColor;
                }
            }
        }
        
        /// <summary>
        /// Обработка входа мыши
        /// </summary>
        public void OnPointerEnter()
        {
            if (!isSelected)
            {
                Image backgroundImage = GetComponent<Image>();
                if (backgroundImage != null)
                {
                    backgroundImage.color = hoverColor;
                }
                
                PlayHoverSound();
            }
        }
        
        /// <summary>
        /// Обработка выхода мыши
        /// </summary>
        public void OnPointerExit()
        {
            if (!isSelected)
            {
                Image backgroundImage = GetComponent<Image>();
                if (backgroundImage != null)
                {
                    backgroundImage.color = normalColor;
                }
            }
        }
        
        /// <summary>
        /// Воспроизвести звук нажатия кнопки
        /// </summary>
        private void PlayButtonClickSound()
        {
            if (buttonClickSound != null)
            {
                AudioSource.PlayClipAtPoint(buttonClickSound, Camera.main.transform.position);
            }
        }
        
        /// <summary>
        /// Воспроизвести звук наведения
        /// </summary>
        private void PlayHoverSound()
        {
            if (hoverSound != null)
            {
                AudioSource.PlayClipAtPoint(hoverSound, Camera.main.transform.position);
            }
        }
        
        /// <summary>
        /// Получить blueprint
        /// </summary>
        public VehicleBlueprint GetBlueprint()
        {
            return blueprint;
        }
        
        /// <summary>
        /// Проверить, выбран ли элемент
        /// </summary>
        public bool IsSelected()
        {
            return isSelected;
        }
        
        /// <summary>
        /// Обновить информацию о blueprint
        /// </summary>
        public void RefreshInfo()
        {
            UpdateUI();
        }
        
        private void OnDestroy()
        {
            // Отписаться от событий
            if (loadButton != null)
            {
                loadButton.onClick.RemoveListener(OnLoadButtonClicked);
            }
            
            if (deleteButton != null)
            {
                deleteButton.onClick.RemoveListener(OnDeleteButtonClicked);
            }
            
            if (exportButton != null)
            {
                exportButton.onClick.RemoveListener(OnExportButtonClicked);
            }
            
            if (infoButton != null)
            {
                infoButton.onClick.RemoveListener(OnInfoButtonClicked);
            }
        }
    }
}
