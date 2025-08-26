using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using ScrapArchitect.Core;
using ScrapArchitect.Parts;

namespace ScrapArchitect.UI
{
    /// <summary>
    /// UI для режима строительства - панель деталей, drag & drop, управление
    /// </summary>
    public class BuildModeUI : MonoBehaviour
    {
        [Header("Parts Panel")]
        public GameObject partsPanel;
        public Transform partsContainer;
        public GameObject partButtonPrefab;
        public ScrollRect partsScrollRect;
        
        [Header("Build Controls")]
        public Button clearButton;
        public Button resetButton;
        public Button testButton;
        public Button saveButton;
        public Button loadButton;
        
        [Header("Build Info")]
        public TextMeshProUGUI totalCostText;
        public TextMeshProUGUI partsCountText;
        public TextMeshProUGUI buildTimeText;
        
        [Header("Drag & Drop")]
        public GameObject dragPreview;
        public Image dragPreviewImage;
        public Canvas dragCanvas;
        
        [Header("Categories")]
        public Button blocksButton;
        public Button wheelsButton;
        public Button motorsButton;
        public Button toolsButton;
        
        // Списки деталей по категориям
        private List<PartData> availableParts = new List<PartData>();
        private List<PartData> currentCategoryParts = new List<PartData>();
        private List<GameObject> placedParts = new List<GameObject>();
        
        // Drag & Drop
        private GameObject currentDragPart;
        private bool isDragging = false;
        private Vector3 dragOffset;
        
        // Стоимость и статистика
        private float totalCost = 0f;
        private int partsCount = 0;
        private float buildStartTime;
        
        private void Start()
        {
            InitializeBuildUI();
            LoadAvailableParts();
            ShowCategory(PartCategory.Blocks);
        }
        
        /// <summary>
        /// Инициализация UI строительства
        /// </summary>
        private void InitializeBuildUI()
        {
            // Настраиваем кнопки
            if (clearButton != null)
                clearButton.onClick.AddListener(ClearAllParts);
            
            if (resetButton != null)
                resetButton.onClick.AddListener(ResetBuild);
            
            if (testButton != null)
                testButton.onClick.AddListener(StartTest);
            
            if (saveButton != null)
                saveButton.onClick.AddListener(SaveBuild);
            
            if (loadButton != null)
                loadButton.onClick.AddListener(LoadBuild);
            
            // Настраиваем кнопки категорий
            if (blocksButton != null)
                blocksButton.onClick.AddListener(() => ShowCategory(PartCategory.Blocks));
            
            if (wheelsButton != null)
                wheelsButton.onClick.AddListener(() => ShowCategory(PartCategory.Wheels));
            
            if (motorsButton != null)
                motorsButton.onClick.AddListener(() => ShowCategory(PartCategory.Motors));
            
            if (toolsButton != null)
                toolsButton.onClick.AddListener(() => ShowCategory(PartCategory.Tools));
            
            // Скрываем drag preview
            if (dragPreview != null)
                dragPreview.SetActive(false);
            
            buildStartTime = Time.time;
            
            Debug.Log("BuildModeUI initialized");
        }
        
        /// <summary>
        /// Загрузка доступных деталей
        /// </summary>
        private void LoadAvailableParts()
        {
            availableParts.Clear();
            
            // Добавляем блоки
            availableParts.Add(new PartData { partName = "Wood Block", partType = PartType.Block, category = PartCategory.Blocks, cost = 5f, prefabName = "WoodBlock" });
            availableParts.Add(new PartData { partName = "Metal Block", partType = PartType.Block, category = PartCategory.Blocks, cost = 15f, prefabName = "MetalBlock" });
            availableParts.Add(new PartData { partName = "Plastic Block", partType = PartType.Block, category = PartCategory.Blocks, cost = 3f, prefabName = "PlasticBlock" });
            availableParts.Add(new PartData { partName = "Stone Block", partType = PartType.Block, category = PartCategory.Blocks, cost = 20f, prefabName = "StoneBlock" });
            
            // Добавляем колеса
            availableParts.Add(new PartData { partName = "Small Wheel", partType = PartType.Wheel, category = PartCategory.Wheels, cost = 8f, prefabName = "SmallWheel" });
            availableParts.Add(new PartData { partName = "Medium Wheel", partType = PartType.Wheel, category = PartCategory.Wheels, cost = 15f, prefabName = "MediumWheel" });
            availableParts.Add(new PartData { partName = "Large Wheel", partType = PartType.Wheel, category = PartCategory.Wheels, cost = 25f, prefabName = "LargeWheel" });
            availableParts.Add(new PartData { partName = "Off-Road Wheel", partType = PartType.Wheel, category = PartCategory.Wheels, cost = 20f, prefabName = "OffRoadWheel" });
            
            // Добавляем двигатели
            availableParts.Add(new PartData { partName = "Electric Motor", partType = PartType.Motor, category = PartCategory.Motors, cost = 20f, prefabName = "ElectricMotor" });
            availableParts.Add(new PartData { partName = "Gasoline Engine", partType = PartType.Motor, category = PartCategory.Motors, cost = 30f, prefabName = "GasolineEngine" });
            availableParts.Add(new PartData { partName = "Diesel Engine", partType = PartType.Motor, category = PartCategory.Motors, cost = 40f, prefabName = "DieselEngine" });
            availableParts.Add(new PartData { partName = "Jet Engine", partType = PartType.Motor, category = PartCategory.Motors, cost = 50f, prefabName = "JetEngine" });
            
            Debug.Log($"Loaded {availableParts.Count} available parts");
        }
        
        /// <summary>
        /// Показать категорию деталей
        /// </summary>
        public void ShowCategory(PartCategory category)
        {
            currentCategoryParts.Clear();
            
            // Фильтруем детали по категории
            foreach (var part in availableParts)
            {
                if (part.category == category)
                {
                    currentCategoryParts.Add(part);
                }
            }
            
            // Очищаем контейнер
            if (partsContainer != null)
            {
                foreach (Transform child in partsContainer)
                {
                    Destroy(child.gameObject);
                }
            }
            
            // Создаем кнопки для деталей
            foreach (var part in currentCategoryParts)
            {
                CreatePartButton(part);
            }
            
            Debug.Log($"Showing {currentCategoryParts.Count} parts in category {category}");
        }
        
        /// <summary>
        /// Создать кнопку детали
        /// </summary>
        private void CreatePartButton(PartData partData)
        {
            if (partButtonPrefab == null || partsContainer == null)
                return;
            
            GameObject buttonObj = Instantiate(partButtonPrefab, partsContainer);
            PartButton partButton = buttonObj.GetComponent<PartButton>();
            
            if (partButton != null)
            {
                partButton.Initialize(partData);
                partButton.OnPartSelected += OnPartButtonClicked;
            }
        }
        
        /// <summary>
        /// Обработчик нажатия на кнопку детали
        /// </summary>
        private void OnPartButtonClicked(PartData partData)
        {
            StartDragging(partData);
        }
        
        /// <summary>
        /// Начать перетаскивание детали
        /// </summary>
        private void StartDragging(PartData partData)
        {
            if (isDragging)
                return;
            
            // Создаем превью для перетаскивания
            if (dragPreview != null)
            {
                dragPreview.SetActive(true);
                if (dragPreviewImage != null)
                {
                    // Здесь можно установить иконку детали
                    dragPreviewImage.color = Color.white;
                }
            }
            
            isDragging = true;
            
            Debug.Log($"Started dragging {partData.partName}");
        }
        
        /// <summary>
        /// Обновление перетаскивания
        /// </summary>
        private void Update()
        {
            if (isDragging)
            {
                UpdateDrag();
            }
        }
        
        /// <summary>
        /// Обновление перетаскивания
        /// </summary>
        private void UpdateDrag()
        {
            if (!isDragging || dragPreview == null)
                return;
            
            // Обновляем позицию превью
            Vector3 mousePosition = Input.mousePosition;
            dragPreview.transform.position = mousePosition + dragOffset;
            
            // Проверяем нажатие мыши для размещения
            if (Input.GetMouseButtonDown(0))
            {
                PlacePart();
            }
            
            // Проверяем отмену перетаскивания
            if (Input.GetMouseButtonDown(1))
            {
                CancelDrag();
            }
        }
        
        /// <summary>
        /// Разместить деталь
        /// </summary>
        private void PlacePart()
        {
            if (!isDragging)
                return;
            
            // Получаем позицию в мире
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            
            if (Physics.Raycast(ray, out hit))
            {
                // Создаем деталь в мире
                Vector3 placePosition = hit.point;
                GameObject placedPart = CreatePartInWorld(placePosition);
                
                if (placedPart != null)
                {
                    placedParts.Add(placedPart);
                    UpdateBuildStats();
                }
            }
            
            CancelDrag();
        }
        
        /// <summary>
        /// Создать деталь в мире
        /// </summary>
        private GameObject CreatePartInWorld(Vector3 position)
        {
            // Здесь должна быть логика создания конкретной детали
            // Пока создаем простой куб
            GameObject part = GameObject.CreatePrimitive(PrimitiveType.Cube);
            part.transform.position = position;
            part.name = "PlacedPart";
            
            // Добавляем компоненты
            Rigidbody rb = part.AddComponent<Rigidbody>();
            PartController partController = part.AddComponent<PartController>();
            
            if (partController != null)
            {
                partController.partName = "Placed Part";
                partController.mass = 1f;
            }
            
            Debug.Log($"Placed part at {position}");
            return part;
        }
        
        /// <summary>
        /// Отменить перетаскивание
        /// </summary>
        private void CancelDrag()
        {
            isDragging = false;
            
            if (dragPreview != null)
            {
                dragPreview.SetActive(false);
            }
            
            Debug.Log("Drag cancelled");
        }
        
        /// <summary>
        /// Обновить статистику строительства
        /// </summary>
        private void UpdateBuildStats()
        {
            partsCount = placedParts.Count;
            
            if (partsCountText != null)
            {
                partsCountText.text = $"Parts: {partsCount}";
            }
            
            if (buildTimeText != null)
            {
                float buildTime = Time.time - buildStartTime;
                int minutes = Mathf.FloorToInt(buildTime / 60f);
                int seconds = Mathf.FloorToInt(buildTime % 60f);
                buildTimeText.text = $"Build Time: {minutes:00}:{seconds:00}";
            }
        }
        
        /// <summary>
        /// Очистить все детали
        /// </summary>
        public void ClearAllParts()
        {
            foreach (var part in placedParts)
            {
                if (part != null)
                {
                    Destroy(part);
                }
            }
            
            placedParts.Clear();
            UpdateBuildStats();
            
            Debug.Log("All parts cleared");
        }
        
        /// <summary>
        /// Сбросить строительство
        /// </summary>
        public void ResetBuild()
        {
            ClearAllParts();
            buildStartTime = Time.time;
            
            Debug.Log("Build reset");
        }
        
        /// <summary>
        /// Начать тестирование
        /// </summary>
        public void StartTest()
        {
            if (Core.GameManager.Instance != null)
            {
                Core.GameManager.Instance.EnterTestMode();
            }
            
            Debug.Log("Starting test mode");
        }
        
        /// <summary>
        /// Сохранить строительство
        /// </summary>
        public void SaveBuild()
        {
            // TODO: Реализовать сохранение
            Debug.Log("Saving build...");
        }
        
        /// <summary>
        /// Загрузить строительство
        /// </summary>
        public void LoadBuild()
        {
            // TODO: Реализовать загрузку
            Debug.Log("Loading build...");
        }
        
        /// <summary>
        /// Показать UI
        /// </summary>
        public void Show()
        {
            gameObject.SetActive(true);
        }
        
        /// <summary>
        /// Скрыть UI
        /// </summary>
        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
    
    /// <summary>
    /// Данные детали для UI
    /// </summary>
    [System.Serializable]
    public class PartData
    {
        public string partName;
        public PartType partType;
        public PartCategory category;
        public float cost;
        public string prefabName;
        public Sprite icon;
    }
    
    /// <summary>
    /// Категории деталей
    /// </summary>
    public enum PartCategory
    {
        Blocks,
        Wheels,
        Motors,
        Tools
    }
}
