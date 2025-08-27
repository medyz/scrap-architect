using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

namespace ScrapArchitect.UI
{
    /// <summary>
    /// UI для режима строительства
    /// </summary>
    public class BuildingModeUI : MonoBehaviour
    {
        [Header("Building UI")]
        public GameObject buildingPanel;
        public Transform partsContainer;
        public GameObject partButtonPrefab;
        
        [Header("Control Buttons")]
        public Button testButton;
        public Button clearButton;
        public Button backButton;
        public Button saveButton;
        
        [Header("Info Panel")]
        public TextMeshProUGUI instructionText;
        public TextMeshProUGUI partsCountText;
        public TextMeshProUGUI statusText;
        
        [Header("Parts Library")]
        public PartData[] availableParts;
        
        private BuildingMode buildingMode;
        private List<GameObject> partButtons = new List<GameObject>();
        
        [System.Serializable]
        public class PartData
        {
            public string partName;
            public string partType;
            public Color partColor;
            public Sprite partIcon;
            public float weight = 1f;
            public float cost = 10f;
        }
        
        private void Start()
        {
            SetupUI();
            CreatePartButtons();
        }
        
        /// <summary>
        /// Настройка UI
        /// </summary>
        private void SetupUI()
        {
            buildingMode = FindObjectOfType<BuildingMode>();
            
            if (testButton != null)
            {
                testButton.onClick.AddListener(OnTestButtonClick);
            }
            
            if (clearButton != null)
            {
                clearButton.onClick.AddListener(OnClearButtonClick);
            }
            
            if (backButton != null)
            {
                backButton.onClick.AddListener(OnBackButtonClick);
            }
            
            if (saveButton != null)
            {
                saveButton.onClick.AddListener(OnSaveButtonClick);
            }
            
            UpdateUI();
        }
        
        /// <summary>
        /// Создать кнопки деталей
        /// </summary>
        private void CreatePartButtons()
        {
            if (partButtonPrefab == null || partsContainer == null) return;
            
            foreach (var partData in availableParts)
            {
                GameObject buttonObj = Instantiate(partButtonPrefab, partsContainer);
                Button button = buttonObj.GetComponent<Button>();
                TextMeshProUGUI buttonText = buttonObj.GetComponentInChildren<TextMeshProUGUI>();
                
                if (buttonText != null)
                {
                    buttonText.text = partData.partName;
                }
                
                if (button != null)
                {
                    string partType = partData.partType;
                    button.onClick.AddListener(() => OnPartButtonClick(partType));
                }
                
                partButtons.Add(buttonObj);
            }
        }
        
        /// <summary>
        /// Обработчик клика по кнопке детали
        /// </summary>
        private void OnPartButtonClick(string partType)
        {
            if (buildingMode != null)
            {
                buildingMode.CreatePart(partType);
                UpdateUI();
            }
        }
        
        /// <summary>
        /// Обработчик кнопки "Тест"
        /// </summary>
        private void OnTestButtonClick()
        {
            if (buildingMode != null)
            {
                int partsCount = buildingMode.GetPartsCount();
                if (partsCount == 0)
                {
                    ShowStatus("Сначала создайте машину!");
                }
                else
                {
                    ShowStatus($"Тестирование машины с {partsCount} деталями...");
                }
            }
        }
        
        /// <summary>
        /// Обработчик кнопки "Очистить"
        /// </summary>
        private void OnClearButtonClick()
        {
            if (buildingMode != null)
            {
                // Очистка происходит в BuildingMode
                UpdateUI();
                ShowStatus("Площадка очищена!");
            }
        }
        
        /// <summary>
        /// Обработчик кнопки "Назад"
        /// </summary>
        private void OnBackButtonClick()
        {
            if (UIManager.Instance != null)
            {
                UIManager.Instance.ShowMainMenu();
            }
        }
        
        /// <summary>
        /// Обработчик кнопки "Сохранить"
        /// </summary>
        private void OnSaveButtonClick()
        {
            ShowStatus("Сохранение пока не реализовано!");
        }
        
        /// <summary>
        /// Обновить UI
        /// </summary>
        private void UpdateUI()
        {
            if (buildingMode != null)
            {
                int partsCount = buildingMode.GetPartsCount();
                if (partsCountText != null)
                {
                    partsCountText.text = $"Деталей: {partsCount}";
                }
            }
        }
        
        /// <summary>
        /// Показать статус
        /// </summary>
        private void ShowStatus(string message)
        {
            if (statusText != null)
            {
                statusText.text = message;
            }
            Debug.Log(message);
        }
        
        /// <summary>
        /// Показать UI
        /// </summary>
        public void Show()
        {
            if (buildingPanel != null)
            {
                buildingPanel.SetActive(true);
            }
            UpdateUI();
        }
        
        /// <summary>
        /// Скрыть UI
        /// </summary>
        public void Hide()
        {
            if (buildingPanel != null)
            {
                buildingPanel.SetActive(false);
            }
        }
    }
}
