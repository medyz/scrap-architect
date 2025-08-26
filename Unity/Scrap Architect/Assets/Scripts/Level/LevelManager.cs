using UnityEngine;
using System;
using System.Collections.Generic;
using ScrapArchitect.Gameplay;

namespace ScrapArchitect.Level
{
    /// <summary>
    /// Менеджер уровней - управляет загрузкой и выгрузкой игровых уровней
    /// </summary>
    public class LevelManager : MonoBehaviour
    {
        [Header("Level Settings")]
        public string currentLevelName = "";
        public bool isLevelLoaded = false;
        public float levelLoadTime = 0f;
        
        [Header("Level Prefabs")]
        public GameObject watermelonDeliveryLevel;
        public GameObject fencePaintingLevel;
        public GameObject yardCleaningLevel;
        public GameObject lawnmowerRaceLevel;
        public GameObject flowerCollectionLevel;
        public GameObject eggTransportLevel;
        public GameObject neighborVictoryLevel;
        public GameObject testPolygonLevel;
        
        [Header("Level Objects")]
        public Transform levelContainer;
        public Transform spawnPoint;
        public Transform[] checkpoints;
        public GameObject[] collectibleItems;
        public GameObject[] destructibleObjects;
        public GameObject[] interactiveObjects;
        
        [Header("Audio")]
        public AudioClip levelLoadSound;
        public AudioClip levelCompleteSound;
        public AudioClip levelFailSound;
        
        private GameObject currentLevelInstance;
        private Contract currentContract;
        private LevelData currentLevelData;
        
        // Events
        public Action<string> OnLevelLoaded;
        public Action<string> OnLevelUnloaded;
        public Action<Contract> OnLevelStarted;
        public Action<Contract> OnLevelCompleted;
        public Action<Contract> OnLevelFailed;
        
        // Singleton pattern
        public static LevelManager Instance { get; private set; }
        
        private void Awake()
        {
            // Singleton pattern
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                InitializeLevelManager();
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
        private void Start()
        {
            // Подписываемся на события контрактов
            if (ContractManager.Instance != null)
            {
                ContractManager.Instance.OnContractAccepted += OnContractAcceptedHandler;
                ContractManager.Instance.OnContractCompleted += OnContractCompletedHandler;
                ContractManager.Instance.OnContractFailed += OnContractFailedHandler;
            }
        }
        
        private void OnDestroy()
        {
            // Отписываемся от событий
            if (ContractManager.Instance != null)
            {
                ContractManager.Instance.OnContractAccepted -= OnContractAcceptedHandler;
                ContractManager.Instance.OnContractCompleted -= OnContractCompletedHandler;
                ContractManager.Instance.OnContractFailed -= OnContractFailedHandler;
            }
        }
        
        /// <summary>
        /// Инициализация менеджера уровней
        /// </summary>
        private void InitializeLevelManager()
        {
            Debug.Log("LevelManager initialized");
        }
        
        /// <summary>
        /// Загрузить уровень для контракта
        /// </summary>
        public bool LoadLevelForContract(Contract contract)
        {
            if (contract == null)
            {
                Debug.LogError("Cannot load level: contract is null");
                return false;
            }
            
            // Выгрузить текущий уровень
            UnloadCurrentLevel();
            
            // Определить какой уровень загружать
            GameObject levelPrefab = GetLevelPrefabForContract(contract);
            if (levelPrefab == null)
            {
                Debug.LogError($"No level prefab found for contract: {contract.title}");
                return false;
            }
            
            // Загрузить уровень
            return LoadLevel(levelPrefab, contract);
        }
        
        /// <summary>
        /// Получить префаб уровня для контракта
        /// </summary>
        private GameObject GetLevelPrefabForContract(Contract contract)
        {
            switch (contract.title)
            {
                case "Доставка арбузов":
                    return watermelonDeliveryLevel;
                    
                case "Покраска забора":
                    return fencePaintingLevel;
                    
                case "Уборка двора":
                    return yardCleaningLevel;
                    
                case "Гонка на газонокосилках":
                    return lawnmowerRaceLevel;
                    
                case "Сбор цветов":
                    return flowerCollectionLevel;
                    
                case "Перевозка яиц":
                    return eggTransportLevel;
                    
                case "Победа над соседом":
                    return neighborVictoryLevel;
                    
                default:
                    // Для случайных контрактов используем тестовый полигон
                    return testPolygonLevel;
            }
        }
        
        /// <summary>
        /// Загрузить уровень
        /// </summary>
        private bool LoadLevel(GameObject levelPrefab, Contract contract)
        {
            try
            {
                // Создать экземпляр уровня
                currentLevelInstance = Instantiate(levelPrefab, Vector3.zero, Quaternion.identity);
                currentLevelInstance.name = $"Level_{contract.title.Replace(" ", "_")}";
                
                // Настроить уровень
                SetupLevel(contract);
                
                // Обновить состояние
                currentLevelName = contract.title;
                currentContract = contract;
                isLevelLoaded = true;
                levelLoadTime = Time.time;
                
                // Воспроизвести звук загрузки
                PlayLevelLoadSound();
                
                // Вызвать событие
                OnLevelLoaded?.Invoke(currentLevelName);
                OnLevelStarted?.Invoke(contract);
                
                Debug.Log($"Level loaded: {currentLevelName}");
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to load level: {e.Message}");
                return false;
            }
        }
        
        /// <summary>
        /// Настроить уровень
        /// </summary>
        private void SetupLevel(Contract contract)
        {
            if (currentLevelInstance == null) return;
            
            // Найти компоненты уровня
            levelContainer = currentLevelInstance.transform.Find("LevelContainer");
            spawnPoint = currentLevelInstance.transform.Find("SpawnPoint");
            
            // Настроить чекпоинты
            SetupCheckpoints(contract);
            
            // Настроить собираемые предметы
            SetupCollectibleItems(contract);
            
            // Настроить интерактивные объекты
            SetupInteractiveObjects(contract);
            
            // Настроить физику уровня
            SetupLevelPhysics();
        }
        
        /// <summary>
        /// Настроить чекпоинты
        /// </summary>
        private void SetupCheckpoints(Contract contract)
        {
            if (contract.checkpoints == null || contract.checkpoints.Length == 0) return;
            
            // Найти контейнер чекпоинтов
            Transform checkpointsContainer = currentLevelInstance.transform.Find("Checkpoints");
            if (checkpointsContainer == null) return;
            
            checkpoints = new Transform[contract.checkpoints.Length];
            
            for (int i = 0; i < contract.checkpoints.Length; i++)
            {
                GameObject checkpoint = new GameObject($"Checkpoint_{i + 1}");
                checkpoint.transform.SetParent(checkpointsContainer);
                checkpoint.transform.position = contract.checkpoints[i];
                
                // Добавить компонент чекпоинта
                Checkpoint checkpointComponent = checkpoint.AddComponent<Checkpoint>();
                checkpointComponent.checkpointID = i;
                checkpointComponent.checkpointName = $"Checkpoint {i + 1}";
                
                checkpoints[i] = checkpoint.transform;
            }
        }
        
        /// <summary>
        /// Настроить собираемые предметы
        /// </summary>
        private void SetupCollectibleItems(Contract contract)
        {
            if (contract.contractType != ContractType.Collection) return;
            
            // Найти контейнер предметов
            Transform itemsContainer = currentLevelInstance.transform.Find("CollectibleItems");
            if (itemsContainer == null) return;
            
            // Определить количество предметов
            int itemCount = 0;
            foreach (var objective in contract.objectives)
            {
                if (objective.type == ContractObjective.ObjectiveType.CollectItems)
                {
                    itemCount = Mathf.RoundToInt(objective.targetValue);
                    break;
                }
            }
            
            if (itemCount <= 0) return;
            
            collectibleItems = new GameObject[itemCount];
            
            for (int i = 0; i < itemCount; i++)
            {
                // Создать предмет в случайной позиции
                Vector3 randomPosition = GetRandomPositionInLevel();
                GameObject item = CreateCollectibleItem(contract.title, randomPosition);
                
                if (item != null)
                {
                    item.transform.SetParent(itemsContainer);
                    collectibleItems[i] = item;
                }
            }
        }
        
        /// <summary>
        /// Создать собираемый предмет
        /// </summary>
        private GameObject CreateCollectibleItem(string contractTitle, Vector3 position)
        {
            GameObject item = null;
            
            switch (contractTitle)
            {
                case "Доставка арбузов":
                    item = CreateWatermelon(position);
                    break;
                    
                case "Уборка двора":
                    item = CreateTrashItem(position);
                    break;
                    
                case "Сбор цветов":
                    item = CreateFlower(position);
                    break;
                    
                case "Перевозка яиц":
                    item = CreateEgg(position);
                    break;
                    
                default:
                    item = CreateGenericItem(position);
                    break;
            }
            
            return item;
        }
        
        /// <summary>
        /// Создать арбуз
        /// </summary>
        private GameObject CreateWatermelon(Vector3 position)
        {
            GameObject watermelon = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            watermelon.name = "Watermelon";
            watermelon.transform.position = position;
            watermelon.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            
            // Настроить материал
            Renderer renderer = watermelon.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.color = Color.green;
            }
            
            // Добавить компонент собираемого предмета
            CollectibleItem collectible = watermelon.AddComponent<CollectibleItem>();
            collectible.itemType = "watermelon";
            collectible.itemValue = 1;
            
            return watermelon;
        }
        
        /// <summary>
        /// Создать предмет мусора
        /// </summary>
        private GameObject CreateTrashItem(Vector3 position)
        {
            GameObject trash = GameObject.CreatePrimitive(PrimitiveType.Cube);
            trash.name = "Trash";
            trash.transform.position = position;
            trash.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
            
            // Настроить материал
            Renderer renderer = trash.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.color = Color.gray;
            }
            
            // Добавить компонент собираемого предмета
            CollectibleItem collectible = trash.AddComponent<CollectibleItem>();
            collectible.itemType = "trash";
            collectible.itemValue = 1;
            
            return trash;
        }
        
        /// <summary>
        /// Создать цветок
        /// </summary>
        private GameObject CreateFlower(Vector3 position)
        {
            GameObject flower = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            flower.name = "Flower";
            flower.transform.position = position;
            flower.transform.localScale = new Vector3(0.1f, 0.5f, 0.1f);
            
            // Настроить материал
            Renderer renderer = flower.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.color = Color.yellow;
            }
            
            // Добавить компонент собираемого предмета
            CollectibleItem collectible = flower.AddComponent<CollectibleItem>();
            collectible.itemType = "flower";
            collectible.itemValue = 1;
            
            return flower;
        }
        
        /// <summary>
        /// Создать яйцо
        /// </summary>
        private GameObject CreateEgg(Vector3 position)
        {
            GameObject egg = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            egg.name = "Egg";
            egg.transform.position = position;
            egg.transform.localScale = new Vector3(0.2f, 0.3f, 0.2f);
            
            // Настроить материал
            Renderer renderer = egg.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.color = Color.white;
            }
            
            // Добавить компонент собираемого предмета
            CollectibleItem collectible = egg.AddComponent<CollectibleItem>();
            collectible.itemType = "egg";
            collectible.itemValue = 1;
            
            return egg;
        }
        
        /// <summary>
        /// Создать общий предмет
        /// </summary>
        private GameObject CreateGenericItem(Vector3 position)
        {
            GameObject item = GameObject.CreatePrimitive(PrimitiveType.Cube);
            item.name = "GenericItem";
            item.transform.position = position;
            item.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            
            // Настроить материал
            Renderer renderer = item.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.color = Color.blue;
            }
            
            // Добавить компонент собираемого предмета
            CollectibleItem collectible = item.AddComponent<CollectibleItem>();
            collectible.itemType = "generic";
            collectible.itemValue = 1;
            
            return item;
        }
        
        /// <summary>
        /// Настроить интерактивные объекты
        /// </summary>
        private void SetupInteractiveObjects(Contract contract)
        {
            // Найти контейнер интерактивных объектов
            Transform interactiveContainer = currentLevelInstance.transform.Find("InteractiveObjects");
            if (interactiveContainer == null) return;
            
            // Создать объекты в зависимости от типа контракта
            switch (contract.title)
            {
                case "Покраска забора":
                    CreateFenceForPainting();
                    break;
                    
                case "Уборка двора":
                    CreateWeedsForCleaning();
                    break;
            }
        }
        
        /// <summary>
        /// Создать забор для покраски
        /// </summary>
        private void CreateFenceForPainting()
        {
            Transform fenceContainer = currentLevelInstance.transform.Find("InteractiveObjects/Fence");
            if (fenceContainer == null) return;
            
            // Создать сегменты забора
            for (int i = 0; i < 10; i++)
            {
                GameObject fenceSegment = GameObject.CreatePrimitive(PrimitiveType.Cube);
                fenceSegment.name = $"FenceSegment_{i}";
                fenceSegment.transform.SetParent(fenceContainer);
                fenceSegment.transform.position = new Vector3(i * 2f, 1f, 0f);
                fenceSegment.transform.localScale = new Vector3(1.5f, 2f, 0.1f);
                
                // Настроить материал
                Renderer renderer = fenceSegment.GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.material.color = Color.gray;
                }
                
                // Добавить компонент для покраски
                PaintableObject paintable = fenceSegment.AddComponent<PaintableObject>();
                paintable.paintProgress = 0f;
                paintable.requiredPaint = 100f;
            }
        }
        
        /// <summary>
        /// Создать сорняки для уборки
        /// </summary>
        private void CreateWeedsForCleaning()
        {
            Transform weedsContainer = currentLevelInstance.transform.Find("InteractiveObjects/Weeds");
            if (weedsContainer == null) return;
            
            // Создать сорняки в случайных позициях
            for (int i = 0; i < 10; i++)
            {
                Vector3 randomPosition = GetRandomPositionInLevel();
                GameObject weed = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                weed.name = $"Weed_{i}";
                weed.transform.SetParent(weedsContainer);
                weed.transform.position = randomPosition;
                weed.transform.localScale = new Vector3(0.1f, 0.3f, 0.1f);
                
                // Настроить материал
                Renderer renderer = weed.GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.material.color = Color.green;
                }
                
                // Добавить компонент для уборки
                CleanableObject cleanable = weed.AddComponent<CleanableObject>();
                cleanable.cleanProgress = 0f;
                cleanable.requiredCleaning = 100f;
            }
        }
        
        /// <summary>
        /// Настроить физику уровня
        /// </summary>
        private void SetupLevelPhysics()
        {
            // Найти все объекты с коллайдерами
            Collider[] colliders = currentLevelInstance.GetComponentsInChildren<Collider>();
            
            foreach (var collider in colliders)
            {
                // Настроить физические свойства
                Rigidbody rb = collider.GetComponent<Rigidbody>();
                if (rb == null)
                {
                    rb = collider.gameObject.AddComponent<Rigidbody>();
                }
                
                // Настроить массу и сопротивление
                rb.mass = 1f;
                rb.drag = 0.5f;
                rb.angularDrag = 0.5f;
            }
        }
        
        /// <summary>
        /// Получить случайную позицию в пределах уровня
        /// </summary>
        private Vector3 GetRandomPositionInLevel()
        {
            float x = UnityEngine.Random.Range(-20f, 20f);
            float z = UnityEngine.Random.Range(-20f, 20f);
            return new Vector3(x, 0f, z);
        }
        
        /// <summary>
        /// Выгрузить текущий уровень
        /// </summary>
        public void UnloadCurrentLevel()
        {
            if (currentLevelInstance != null)
            {
                string levelName = currentLevelName;
                
                Destroy(currentLevelInstance);
                currentLevelInstance = null;
                currentLevelName = "";
                currentContract = null;
                isLevelLoaded = false;
                
                // Очистить ссылки
                levelContainer = null;
                spawnPoint = null;
                checkpoints = null;
                collectibleItems = null;
                destructibleObjects = null;
                interactiveObjects = null;
                
                OnLevelUnloaded?.Invoke(levelName);
                Debug.Log($"Level unloaded: {levelName}");
            }
        }
        
        /// <summary>
        /// Получить текущий уровень
        /// </summary>
        public GameObject GetCurrentLevel()
        {
            return currentLevelInstance;
        }
        
        /// <summary>
        /// Получить текущий контракт
        /// </summary>
        public Contract GetCurrentContract()
        {
            return currentContract;
        }
        
        /// <summary>
        /// Получить точку спавна
        /// </summary>
        public Transform GetSpawnPoint()
        {
            return spawnPoint;
        }
        
        /// <summary>
        /// Получить чекпоинты
        /// </summary>
        public Transform[] GetCheckpoints()
        {
            return checkpoints;
        }
        
        /// <summary>
        /// Получить собираемые предметы
        /// </summary>
        public GameObject[] GetCollectibleItems()
        {
            return collectibleItems;
        }
        
        #region Event Handlers
        
        /// <summary>
        /// Обработчик принятия контракта
        /// </summary>
        private void OnContractAcceptedHandler(Contract contract)
        {
            LoadLevelForContract(contract);
        }
        
        /// <summary>
        /// Обработчик завершения контракта
        /// </summary>
        private void OnContractCompletedHandler(Contract contract)
        {
            PlayLevelCompleteSound();
            OnLevelCompleted?.Invoke(contract);
        }
        
        /// <summary>
        /// Обработчик провала контракта
        /// </summary>
        private void OnContractFailedHandler(Contract contract)
        {
            PlayLevelFailSound();
            OnLevelFailed?.Invoke(contract);
        }
        
        #endregion
        
        #region Audio Methods
        
        /// <summary>
        /// Воспроизвести звук загрузки уровня
        /// </summary>
        private void PlayLevelLoadSound()
        {
            if (levelLoadSound != null)
            {
                AudioSource.PlayClipAtPoint(levelLoadSound, Camera.main.transform.position);
            }
        }
        
        /// <summary>
        /// Воспроизвести звук завершения уровня
        /// </summary>
        private void PlayLevelCompleteSound()
        {
            if (levelCompleteSound != null)
            {
                AudioSource.PlayClipAtPoint(levelCompleteSound, Camera.main.transform.position);
            }
        }
        
        /// <summary>
        /// Воспроизвести звук провала уровня
        /// </summary>
        private void PlayLevelFailSound()
        {
            if (levelFailSound != null)
            {
                AudioSource.PlayClipAtPoint(levelFailSound, Camera.main.transform.position);
            }
        }
        
        #endregion
    }
    
    /// <summary>
    /// Данные уровня
    /// </summary>
    [Serializable]
    public class LevelData
    {
        public string levelName;
        public string levelDescription;
        public Vector3 spawnPoint;
        public Vector3[] checkpoints;
        public GameObject[] collectibleItems;
        public GameObject[] destructibleObjects;
        public GameObject[] interactiveObjects;
        public float timeLimit;
        public int maxParts;
        public float maxMass;
    }
}
