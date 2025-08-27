using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

namespace ScrapArchitect.Gameplay
{
    /// <summary>
    /// Режим строительства - гараж для сборки машин
    /// </summary>
    public class BuildingMode : MonoBehaviour
    {
        [Header("Building Area")]
        public Transform buildingArea;
        public Transform partsPanel;
        public GameObject partPrefab;
        
        [Header("UI Elements")]
        public Button testButton;
        public Button clearButton;
        public Button backButton;
        public TextMeshProUGUI instructionText;
        
        [Header("Parts Categories")]
        public GameObject[] partCategories;
        public string[] categoryNames = { "Рама", "Двигатели", "Колеса", "Инструменты", "Декорации" };
        
        private List<GameObject> placedParts = new List<GameObject>();
        private GameObject selectedPart;
        private Camera buildingCamera;
        
        private void Start()
        {
            SetupUI();
            SetupCamera();
            ShowInstructions();
        }
        
        /// <summary>
        /// Настройка UI элементов
        /// </summary>
        private void SetupUI()
        {
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
        }
        
        /// <summary>
        /// Настройка камеры
        /// </summary>
        private void SetupCamera()
        {
            buildingCamera = Camera.main;
            if (buildingCamera != null)
            {
                buildingCamera.transform.position = new Vector3(0, 5, -10);
                buildingCamera.transform.LookAt(buildingArea);
            }
        }
        
        /// <summary>
        /// Показать инструкции
        /// </summary>
        private void ShowInstructions()
        {
            if (instructionText != null)
            {
                instructionText.text = "Добро пожаловать в гараж!\n\n" +
                    "• Перетаскивайте детали из панели слева на площадку\n" +
                    "• Соединяйте детали для создания машины\n" +
                    "• Нажмите 'Тест' чтобы проверить ваше творение\n" +
                    "• Нажмите 'Очистить' чтобы начать заново";
            }
        }
        
        /// <summary>
        /// Создать деталь
        /// </summary>
        public void CreatePart(string partType)
        {
            if (partPrefab != null && buildingArea != null)
            {
                GameObject newPart = Instantiate(partPrefab, buildingArea);
                newPart.name = $"{partType}_{placedParts.Count}";
                
                // Добавляем компоненты для перетаскивания
                PartDragger dragger = newPart.AddComponent<PartDragger>();
                dragger.Initialize(this);
                
                placedParts.Add(newPart);
                
                Debug.Log($"Создана деталь: {partType}");
            }
        }
        
        /// <summary>
        /// Обработчик кнопки "Тест"
        /// </summary>
        private void OnTestButtonClick()
        {
            if (placedParts.Count == 0)
            {
                ShowMessage("Сначала создайте машину!");
                return;
            }
            
            Debug.Log("Запуск тестирования машины...");
            // TODO: Переход к режиму тестирования
            ShowMessage("Режим тестирования пока в разработке!");
        }
        
        /// <summary>
        /// Обработчик кнопки "Очистить"
        /// </summary>
        private void OnClearButtonClick()
        {
            ClearAllParts();
            ShowMessage("Площадка очищена!");
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
        /// Очистить все детали
        /// </summary>
        private void ClearAllParts()
        {
            foreach (var part in placedParts)
            {
                if (part != null)
                {
                    DestroyImmediate(part);
                }
            }
            placedParts.Clear();
        }
        
        /// <summary>
        /// Показать сообщение
        /// </summary>
        private void ShowMessage(string message)
        {
            Debug.Log(message);
            if (instructionText != null)
            {
                instructionText.text = message;
            }
        }
        
        /// <summary>
        /// Получить количество деталей
        /// </summary>
        public int GetPartsCount()
        {
            return placedParts.Count;
        }
    }
    
    /// <summary>
    /// Компонент для перетаскивания деталей
    /// </summary>
    public class PartDragger : MonoBehaviour
    {
        private bool isDragging = false;
        private Vector3 offset;
        private BuildingMode buildingMode;
        
        public void Initialize(BuildingMode mode)
        {
            buildingMode = mode;
        }
        
        private void OnMouseDown()
        {
            isDragging = true;
            offset = transform.position - GetMouseWorldPosition();
        }
        
        private void OnMouseDrag()
        {
            if (isDragging)
            {
                transform.position = GetMouseWorldPosition() + offset;
            }
        }
        
        private void OnMouseUp()
        {
            isDragging = false;
        }
        
        private Vector3 GetMouseWorldPosition()
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = -Camera.main.transform.position.z;
            return Camera.main.ScreenToWorldPoint(mousePos);
        }
    }
}
