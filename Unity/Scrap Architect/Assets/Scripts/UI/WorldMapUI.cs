using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections.Generic;
using System.Linq;
using ScrapArchitect.Gameplay;

namespace ScrapArchitect.UI
{
    /// <summary>
    /// Карта мира с контрактами
    /// </summary>
    public class WorldMapUI : UIBase
    {
        [Header("World Map Elements")]
        public RectTransform mapContainer;
        public GameObject locationPrefab;
        public Button backButton;
        public Button zoomInButton;
        public Button zoomOutButton;
        public Button resetViewButton;
        
        [Header("Location Details")]
        public GameObject locationDetailsPanel;
        public TextMeshProUGUI locationNameText;
        public TextMeshProUGUI locationDescriptionText;
        public TextMeshProUGUI availableContractsText;
        public Transform contractsListContainer;
        public GameObject contractItemPrefab;
        
        [Header("Map Controls")]
        public Slider zoomSlider;
        public TMP_Dropdown regionFilter;
        public Button clearFiltersButton;
        
        [Header("Info Elements")]
        public TextMeshProUGUI totalLocationsText;
        public TextMeshProUGUI totalContractsText;
        public TextMeshProUGUI unlockedRegionsText;
        
        [Header("Map Settings")]
        public float minZoom = 0.5f;
        public float maxZoom = 3f;
        public float defaultZoom = 1f;
        public float zoomSpeed = 0.1f;
        
        private List<WorldLocation> worldLocations = new List<WorldLocation>();
        private List<LocationMarker> locationMarkers = new List<LocationMarker>();
        private WorldLocation selectedLocation;
        private float currentZoom = 1f;
        private Vector2 mapOffset = Vector2.zero;
        private bool isDragging = false;
        private Vector2 lastMousePosition;
        
        [Serializable]
        public class WorldLocation
        {
            public string id;
            public string name;
            public string description;
            public Vector2 position;
            public string region;
            public bool isUnlocked;
            public List<Contract> availableContracts = new List<Contract>();
            public Sprite locationIcon;
        }
        
        [Serializable]
        public class LocationMarker
        {
            public WorldLocation location;
            public GameObject markerObject;
            public Button markerButton;
            public Image markerImage;
            public TextMeshProUGUI markerText;
        }
        
        private void Start()
        {
            SetupButtons();
            SetupControls();
            InitializeWorldMap();
            RefreshMap();
        }
        
        /// <summary>
        /// Настройка кнопок
        /// </summary>
        private void SetupButtons()
        {
            if (backButton != null)
            {
                backButton.onClick.AddListener(OnBackButtonClick);
            }
            
            if (zoomInButton != null)
            {
                zoomInButton.onClick.AddListener(OnZoomInClick);
            }
            
            if (zoomOutButton != null)
            {
                zoomOutButton.onClick.AddListener(OnZoomOutClick);
            }
            
            if (resetViewButton != null)
            {
                resetViewButton.onClick.AddListener(OnResetViewClick);
            }
            
            if (clearFiltersButton != null)
            {
                clearFiltersButton.onClick.AddListener(OnClearFiltersClick);
            }
        }
        
        /// <summary>
        /// Настройка элементов управления
        /// </summary>
        private void SetupControls()
        {
            if (zoomSlider != null)
            {
                zoomSlider.minValue = minZoom;
                zoomSlider.maxValue = maxZoom;
                zoomSlider.value = defaultZoom;
                zoomSlider.onValueChanged.AddListener(OnZoomSliderChanged);
            }
            
            if (regionFilter != null)
            {
                regionFilter.ClearOptions();
                regionFilter.AddOptions(new List<string> { "Все регионы", "Деревня", "Город", "Порт", "Горы", "Пустыня", "Лес" });
                regionFilter.onValueChanged.AddListener(OnRegionFilterChanged);
            }
        }
        
        /// <summary>
        /// Инициализация карты мира
        /// </summary>
        private void InitializeWorldMap()
        {
            // Создаем локации для каждого контракта
            CreateWorldLocations();
            
            // Создаем маркеры на карте
            CreateLocationMarkers();
        }
        
        /// <summary>
        /// Создание локаций мира
        /// </summary>
        private void CreateWorldLocations()
        {
            worldLocations.Clear();
            
            // Получаем все доступные контракты
            var contracts = SpecificContracts.GetAllContracts();
            
            // Создаем локации на основе контрактов
            var locations = new Dictionary<string, WorldLocation>
            {
                { "village", new WorldLocation { id = "village", name = "Деревня", description = "Спокойная деревня с простыми заказами", position = new Vector2(-200, 100), region = "Деревня", isUnlocked = true } },
                { "city", new WorldLocation { id = "city", name = "Город", description = "Большой город с разнообразными контрактами", position = new Vector2(0, 0), region = "Город", isUnlocked = true } },
                { "port", new WorldLocation { id = "port", name = "Порт", description = "Морской порт с водными контрактами", position = new Vector2(200, -100), region = "Порт", isUnlocked = true } },
                { "mountains", new WorldLocation { id = "mountains", name = "Горы", description = "Горная местность с экстремальными заданиями", position = new Vector2(-100, -200), region = "Горы", isUnlocked = false } },
                { "desert", new WorldLocation { id = "desert", name = "Пустыня", description = "Жаркая пустыня с уникальными контрактами", position = new Vector2(300, 150), region = "Пустыня", isUnlocked = false } },
                { "forest", new WorldLocation { id = "forest", name = "Лес", description = "Таинственный лес с природными заданиями", position = new Vector2(-300, -50), region = "Лес", isUnlocked = false } }
            };
            
            // Распределяем контракты по локациям
            foreach (var contract in contracts)
            {
                string locationId = GetLocationForContract(contract);
                if (locations.ContainsKey(locationId))
                {
                    locations[locationId].availableContracts.Add(contract);
                }
            }
            
            worldLocations.AddRange(locations.Values);
        }
        
        /// <summary>
        /// Определение локации для контракта
        /// </summary>
        private string GetLocationForContract(Contract contract)
        {
            switch (contract.title)
            {
                case "Доставка арбузов":
                case "Покраска забора":
                case "Уборка двора":
                    return "village";
                case "Гонка на газонокосилках":
                case "Сбор цветов":
                    return "city";
                case "Перевозка яиц":
                case "Победа над соседом":
                    return "port";
                default:
                    return "city";
            }
        }
        
        /// <summary>
        /// Создание маркеров локаций
        /// </summary>
        private void CreateLocationMarkers()
        {
            // Очищаем старые маркеры
            foreach (var marker in locationMarkers)
            {
                if (marker.markerObject != null)
                {
                    DestroyImmediate(marker.markerObject);
                }
            }
            locationMarkers.Clear();
            
            // Создаем новые маркеры
            foreach (var location in worldLocations)
            {
                if (locationPrefab != null && mapContainer != null)
                {
                    GameObject markerObj = Instantiate(locationPrefab, mapContainer);
                    LocationMarker marker = new LocationMarker
                    {
                        location = location,
                        markerObject = markerObj,
                        markerButton = markerObj.GetComponent<Button>(),
                        markerImage = markerObj.GetComponent<Image>(),
                        markerText = markerObj.GetComponentInChildren<TextMeshProUGUI>()
                    };
                    
                    // Настраиваем маркер
                    SetupLocationMarker(marker);
                    locationMarkers.Add(marker);
                }
            }
        }
        
        /// <summary>
        /// Настройка маркера локации
        /// </summary>
        private void SetupLocationMarker(LocationMarker marker)
        {
            if (marker.markerObject != null)
            {
                // Устанавливаем позицию
                RectTransform rectTransform = marker.markerObject.GetComponent<RectTransform>();
                if (rectTransform != null)
                {
                    rectTransform.anchoredPosition = marker.location.position;
                }
                
                // Настраиваем текст
                if (marker.markerText != null)
                {
                    marker.markerText.text = marker.location.name;
                }
                
                // Настраиваем кнопку
                if (marker.markerButton != null)
                {
                    marker.markerButton.onClick.AddListener(() => OnLocationClick(marker.location));
                }
                
                // Настраиваем внешний вид
                UpdateMarkerAppearance(marker);
            }
        }
        
        /// <summary>
        /// Обновление внешнего вида маркера
        /// </summary>
        private void UpdateMarkerAppearance(LocationMarker marker)
        {
            if (marker.markerImage != null)
            {
                // Разные цвета для разных статусов
                if (!marker.location.isUnlocked)
                {
                    marker.markerImage.color = Color.gray; // Заблокированные локации
                }
                else if (marker.location.availableContracts.Count > 0)
                {
                    marker.markerImage.color = Color.green; // Доступные локации с контрактами
                }
                else
                {
                    marker.markerImage.color = Color.blue; // Доступные локации без контрактов
                }
            }
        }
        
        /// <summary>
        /// Обновление карты
        /// </summary>
        private void RefreshMap()
        {
            UpdateLocationMarkers();
            UpdateInfoTexts();
        }
        
        /// <summary>
        /// Обновление маркеров локаций
        /// </summary>
        private void UpdateLocationMarkers()
        {
            foreach (var marker in locationMarkers)
            {
                UpdateMarkerAppearance(marker);
            }
        }
        
        /// <summary>
        /// Обновление информационных текстов
        /// </summary>
        private void UpdateInfoTexts()
        {
            if (totalLocationsText != null)
            {
                totalLocationsText.text = $"Локаций: {worldLocations.Count}";
            }
            
            if (totalContractsText != null)
            {
                int totalContracts = worldLocations.Sum(l => l.availableContracts.Count);
                totalContractsText.text = $"Контрактов: {totalContracts}";
            }
            
            if (unlockedRegionsText != null)
            {
                int unlockedCount = worldLocations.Count(l => l.isUnlocked);
                unlockedRegionsText.text = $"Открыто: {unlockedCount}/{worldLocations.Count}";
            }
        }
        
        /// <summary>
        /// Обработка клика по локации
        /// </summary>
        private void OnLocationClick(WorldLocation location)
        {
            if (!location.isUnlocked)
            {
                ShowLocationLockedMessage(location);
                return;
            }
            
            selectedLocation = location;
            ShowLocationDetails(location);
        }
        
        /// <summary>
        /// Показать детали локации
        /// </summary>
        private void ShowLocationDetails(WorldLocation location)
        {
            if (locationDetailsPanel != null)
            {
                locationDetailsPanel.SetActive(true);
                
                if (locationNameText != null)
                {
                    locationNameText.text = location.name;
                }
                
                if (locationDescriptionText != null)
                {
                    locationDescriptionText.text = location.description;
                }
                
                if (availableContractsText != null)
                {
                    availableContractsText.text = $"Доступно контрактов: {location.availableContracts.Count}";
                }
                
                // Показываем список контрактов
                ShowLocationContracts(location);
            }
        }
        
        /// <summary>
        /// Показать контракты локации
        /// </summary>
        private void ShowLocationContracts(WorldLocation location)
        {
            if (contractsListContainer != null)
            {
                // Очищаем старые элементы
                foreach (Transform child in contractsListContainer)
                {
                    DestroyImmediate(child.gameObject);
                }
                
                // Создаем новые элементы
                foreach (var contract in location.availableContracts)
                {
                    if (contractItemPrefab != null)
                    {
                        GameObject contractObj = Instantiate(contractItemPrefab, contractsListContainer);
                        
                        // Настраиваем текст контракта
                        TextMeshProUGUI contractText = contractObj.GetComponentInChildren<TextMeshProUGUI>();
                        if (contractText != null)
                        {
                            contractText.text = contract.title;
                        }
                        
                        // Настраиваем кнопку
                        Button contractButton = contractObj.GetComponent<Button>();
                        if (contractButton != null)
                        {
                            contractButton.onClick.AddListener(() => OnContractSelected(contract));
                        }
                    }
                }
            }
        }
        
        /// <summary>
        /// Показать сообщение о заблокированной локации
        /// </summary>
        private void ShowLocationLockedMessage(WorldLocation location)
        {
            Debug.Log($"Локация '{location.name}' заблокирована. Выполните предыдущие контракты для разблокировки.");
            // TODO: Показать UI сообщение
        }
        
        /// <summary>
        /// Обработка выбора контракта
        /// </summary>
        private void OnContractSelected(Contract contract)
        {
            // Принимаем контракт
            if (ContractManager.Instance != null)
            {
                ContractManager.Instance.AcceptContract(contract);
                
                // Переходим к игровому экрану
                if (UIManager.Instance != null)
                {
                    UIManager.Instance.ShowPanel<GameplayUI>();
                }
            }
        }
        
        // Обработчики кнопок
        private void OnBackButtonClick()
        {
            if (UIManager.Instance != null)
            {
                UIManager.Instance.ShowPanel<MainMenuUI>();
            }
        }
        
        private void OnZoomInClick()
        {
            SetZoom(currentZoom + zoomSpeed);
        }
        
        private void OnZoomOutClick()
        {
            SetZoom(currentZoom - zoomSpeed);
        }
        
        private void OnResetViewClick()
        {
            SetZoom(defaultZoom);
            mapOffset = Vector2.zero;
            UpdateMapTransform();
        }
        
        private void OnClearFiltersClick()
        {
            if (regionFilter != null)
            {
                regionFilter.value = 0;
            }
        }
        
        // Обработчики элементов управления
        private void OnZoomSliderChanged(float value)
        {
            SetZoom(value);
        }
        
        private void OnRegionFilterChanged(int index)
        {
            // TODO: Реализовать фильтрацию по регионам
        }
        
        /// <summary>
        /// Установка зума
        /// </summary>
        private void SetZoom(float zoom)
        {
            currentZoom = Mathf.Clamp(zoom, minZoom, maxZoom);
            
            if (zoomSlider != null)
            {
                zoomSlider.value = currentZoom;
            }
            
            UpdateMapTransform();
        }
        
        /// <summary>
        /// Обновление трансформации карты
        /// </summary>
        private void UpdateMapTransform()
        {
            if (mapContainer != null)
            {
                mapContainer.localScale = Vector3.one * currentZoom;
                mapContainer.anchoredPosition = mapOffset;
            }
        }
        
        private void Update()
        {
            HandleMapInput();
        }
        
        /// <summary>
        /// Обработка ввода для карты
        /// </summary>
        private void HandleMapInput()
        {
            // Зум колесиком мыши
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            if (scroll != 0)
            {
                SetZoom(currentZoom + scroll * zoomSpeed * 10f);
            }
            
            // Перетаскивание карты
            if (Input.GetMouseButtonDown(0))
            {
                isDragging = true;
                lastMousePosition = Input.mousePosition;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                isDragging = false;
            }
            
            if (isDragging)
            {
                Vector2 delta = (Vector2)Input.mousePosition - lastMousePosition;
                mapOffset += delta / currentZoom;
                lastMousePosition = Input.mousePosition;
                UpdateMapTransform();
            }
        }
        
        public override void Show()
        {
            base.Show();
            RefreshMap();
        }
        
        public override void Hide()
        {
            if (locationDetailsPanel != null)
            {
                locationDetailsPanel.SetActive(false);
            }
            base.Hide();
        }
    }
}
