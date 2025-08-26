using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ScrapArchitect.Gameplay
{
    /// <summary>
    /// Менеджер контрактов - управляет игровыми задачами
    /// </summary>
    public class ContractManager : MonoBehaviour
    {
        [Header("Contract Settings")]
        public int maxActiveContracts = 3;
        public int maxAvailableContracts = 10;
        public float contractRefreshInterval = 300f; // 5 минут
        public bool autoGenerateContracts = true;
        
        [Header("Generation Settings")]
        public float easyContractChance = 0.4f;
        public float mediumContractChance = 0.3f;
        public float hardContractChance = 0.2f;
        public float expertContractChance = 0.08f;
        public float masterContractChance = 0.02f;
        
        [Header("UI References")]
        public GameObject contractPanel;
        public Transform availableContractsContent;
        public Transform activeContractsContent;
        public GameObject contractItemPrefab;
        
        [Header("Audio Settings")]
        public AudioClip contractAcceptedSound;
        public AudioClip contractCompletedSound;
        public AudioClip contractFailedSound;
        public AudioClip objectiveCompletedSound;
        
        private List<Contract> availableContracts = new List<Contract>();
        private List<Contract> activeContracts = new List<Contract>();
        private List<Contract> completedContracts = new List<Contract>();
        private float lastRefreshTime;
        
        // Events
        public Action<Contract> OnContractAccepted;
        public Action<Contract> OnContractCompleted;
        public Action<Contract> OnContractFailed;
        public Action<Contract, ContractObjective> OnObjectiveCompleted;
        public Action<List<Contract>> OnAvailableContractsUpdated;
        public Action<List<Contract>> OnActiveContractsUpdated;
        
        private void Start()
        {
            InitializeContractManager();
            GenerateInitialContracts();
        }
        
        private void Update()
        {
            UpdateActiveContracts();
            
            // Автоматическое обновление доступных контрактов
            if (autoGenerateContracts && Time.time - lastRefreshTime >= contractRefreshInterval)
            {
                RefreshAvailableContracts();
            }
        }
        
        /// <summary>
        /// Инициализация менеджера контрактов
        /// </summary>
        private void InitializeContractManager()
        {
            lastRefreshTime = Time.time;
            Debug.Log("ContractManager initialized");
        }
        
        /// <summary>
        /// Генерация начальных контрактов
        /// </summary>
        private void GenerateInitialContracts()
        {
            for (int i = 0; i < maxAvailableContracts; i++)
            {
                Contract contract = GenerateRandomContract();
                if (contract != null)
                {
                    availableContracts.Add(contract);
                }
            }
            
            OnAvailableContractsUpdated?.Invoke(availableContracts);
            Debug.Log($"Generated {availableContracts.Count} initial contracts");
        }
        
        /// <summary>
        /// Генерация случайного контракта
        /// </summary>
        private Contract GenerateRandomContract()
        {
            ContractType contractType = GetRandomContractType();
            ContractDifficulty difficulty = GetRandomDifficulty();
            
            switch (contractType)
            {
                case ContractType.Delivery:
                    return GenerateDeliveryContract(difficulty);
                    
                case ContractType.Collection:
                    return GenerateCollectionContract(difficulty);
                    
                case ContractType.Racing:
                    return GenerateRacingContract(difficulty);
                    
                case ContractType.Transport:
                    return GenerateTransportContract(difficulty);
                    
                case ContractType.Construction:
                    return GenerateConstructionContract(difficulty);
                    
                case ContractType.Destruction:
                    return GenerateDestructionContract(difficulty);
                    
                case ContractType.Exploration:
                    return GenerateExplorationContract(difficulty);
                    
                case ContractType.Rescue:
                    return GenerateRescueContract(difficulty);
                    
                default:
                    return GenerateDeliveryContract(difficulty);
            }
        }
        
        /// <summary>
        /// Получить случайный тип контракта
        /// </summary>
        private ContractType GetRandomContractType()
        {
            ContractType[] types = (ContractType[])Enum.GetValues(typeof(ContractType));
            return types[UnityEngine.Random.Range(0, types.Length)];
        }
        
        /// <summary>
        /// Получить случайную сложность
        /// </summary>
        private ContractDifficulty GetRandomDifficulty()
        {
            float random = UnityEngine.Random.Range(0f, 1f);
            
            if (random < easyContractChance)
                return ContractDifficulty.Easy;
            else if (random < easyContractChance + mediumContractChance)
                return ContractDifficulty.Medium;
            else if (random < easyContractChance + mediumContractChance + hardContractChance)
                return ContractDifficulty.Hard;
            else if (random < easyContractChance + mediumContractChance + hardContractChance + expertContractChance)
                return ContractDifficulty.Expert;
            else
                return ContractDifficulty.Master;
        }
        
        /// <summary>
        /// Генерация контракта доставки
        /// </summary>
        private Contract GenerateDeliveryContract(ContractDifficulty difficulty)
        {
            Vector3 startPos = GetRandomPosition();
            Vector3 finishPos = GetRandomPosition();
            float distance = Vector3.Distance(startPos, finishPos);
            
            string[] titles = {
                "Доставка груза",
                "Транспортировка материалов",
                "Перевозка оборудования",
                "Доставка запчастей",
                "Транспортировка груза"
            };
            
            string title = titles[UnityEngine.Random.Range(0, titles.Length)];
            Contract contract = Contract.CreateDeliveryContract(title, startPos, finishPos, distance);
            contract.difficulty = difficulty;
            
            // Настройка наград в зависимости от сложности
            float multiplier = contract.GetDifficultyMultiplier();
            contract.reward.scrapReward = Mathf.RoundToInt(contract.reward.scrapReward * multiplier);
            contract.reward.experienceReward = Mathf.RoundToInt(contract.reward.experienceReward * multiplier);
            
            return contract;
        }
        
        /// <summary>
        /// Генерация контракта сбора
        /// </summary>
        private Contract GenerateCollectionContract(ContractDifficulty difficulty)
        {
            string[] itemTypes = { "металлолом", "пластик", "электроника", "топливо", "детали" };
            string itemType = itemTypes[UnityEngine.Random.Range(0, itemTypes.Length)];
            
            int itemCount = difficulty switch
            {
                ContractDifficulty.Easy => UnityEngine.Random.Range(3, 8),
                ContractDifficulty.Medium => UnityEngine.Random.Range(8, 15),
                ContractDifficulty.Hard => UnityEngine.Random.Range(15, 25),
                ContractDifficulty.Expert => UnityEngine.Random.Range(25, 40),
                ContractDifficulty.Master => UnityEngine.Random.Range(40, 60),
                _ => 10
            };
            
            string title = $"Сбор {itemType}";
            Contract contract = Contract.CreateCollectionContract(title, itemCount, itemType);
            contract.difficulty = difficulty;
            
            float multiplier = contract.GetDifficultyMultiplier();
            contract.reward.scrapReward = Mathf.RoundToInt(contract.reward.scrapReward * multiplier);
            contract.reward.experienceReward = Mathf.RoundToInt(contract.reward.experienceReward * multiplier);
            
            return contract;
        }
        
        /// <summary>
        /// Генерация контракта гонки
        /// </summary>
        private Contract GenerateRacingContract(ContractDifficulty difficulty)
        {
            int checkpointCount = difficulty switch
            {
                ContractDifficulty.Easy => 3,
                ContractDifficulty.Medium => 5,
                ContractDifficulty.Hard => 7,
                ContractDifficulty.Expert => 10,
                ContractDifficulty.Master => 15,
                _ => 5
            };
            
            Vector3[] checkpoints = new Vector3[checkpointCount];
            for (int i = 0; i < checkpointCount; i++)
            {
                checkpoints[i] = GetRandomPosition();
            }
            
            float timeLimit = checkpointCount * 30f; // 30 секунд на чекпоинт
            
            string title = $"Гонка {checkpointCount} чекпоинтов";
            Contract contract = Contract.CreateRacingContract(title, checkpoints, timeLimit);
            contract.difficulty = difficulty;
            
            float multiplier = contract.GetDifficultyMultiplier();
            contract.reward.scrapReward = Mathf.RoundToInt(contract.reward.scrapReward * multiplier);
            contract.reward.experienceReward = Mathf.RoundToInt(contract.reward.experienceReward * multiplier);
            
            return contract;
        }
        
        /// <summary>
        /// Генерация контракта перевозки
        /// </summary>
        private Contract GenerateTransportContract(ContractDifficulty difficulty)
        {
            Vector3 startPos = GetRandomPosition();
            Vector3 finishPos = GetRandomPosition();
            float distance = Vector3.Distance(startPos, finishPos);
            
            Contract contract = new Contract();
            contract.title = "Перевозка пассажиров";
            contract.description = $"Перевезите пассажиров из точки A в точку B. Расстояние: {distance:F0}m";
            contract.contractType = ContractType.Transport;
            contract.difficulty = difficulty;
            contract.startLocation = startPos;
            contract.finishLocation = finishPos;
            
            // Цель: достичь финиша с пассажирами
            ContractObjective objective = new ContractObjective();
            objective.description = "Доставить пассажиров в пункт назначения";
            objective.type = ContractObjective.ObjectiveType.ReachLocation;
            objective.targetValue = 1f;
            contract.objectives.Add(objective);
            
            // Награда
            contract.reward.scrapReward = Mathf.RoundToInt(distance * 15f * contract.GetDifficultyMultiplier());
            contract.reward.experienceReward = Mathf.RoundToInt(distance * 8f * contract.GetDifficultyMultiplier());
            
            return contract;
        }
        
        /// <summary>
        /// Генерация контракта строительства
        /// </summary>
        private Contract GenerateConstructionContract(ContractDifficulty difficulty)
        {
            Contract contract = new Contract();
            contract.title = "Строительство";
            contract.description = "Постройте структуру согласно техническому заданию";
            contract.contractType = ContractType.Construction;
            contract.difficulty = difficulty;
            
            // Цель: построить структуру
            ContractObjective objective = new ContractObjective();
            objective.description = "Построить требуемую структуру";
            objective.type = ContractObjective.ObjectiveType.BuildStructure;
            objective.targetValue = 1f;
            contract.objectives.Add(objective);
            
            // Награда
            contract.reward.scrapReward = 200 * (int)contract.GetDifficultyMultiplier();
            contract.reward.experienceReward = 100 * (int)contract.GetDifficultyMultiplier();
            
            return contract;
        }
        
        /// <summary>
        /// Генерация контракта разрушения
        /// </summary>
        private Contract GenerateDestructionContract(ContractDifficulty difficulty)
        {
            int targetCount = difficulty switch
            {
                ContractDifficulty.Easy => 3,
                ContractDifficulty.Medium => 5,
                ContractDifficulty.Hard => 8,
                ContractDifficulty.Expert => 12,
                ContractDifficulty.Master => 20,
                _ => 5
            };
            
            Contract contract = new Contract();
            contract.title = "Разрушение";
            contract.description = $"Уничтожьте {targetCount} объектов";
            contract.contractType = ContractType.Destruction;
            contract.difficulty = difficulty;
            
            // Цель: уничтожить объекты
            ContractObjective objective = new ContractObjective();
            objective.description = $"Уничтожить {targetCount} объектов";
            objective.type = ContractObjective.ObjectiveType.DestroyObjects;
            objective.targetValue = targetCount;
            contract.objectives.Add(objective);
            
            // Награда
            contract.reward.scrapReward = targetCount * 30 * (int)contract.GetDifficultyMultiplier();
            contract.reward.experienceReward = targetCount * 15 * (int)contract.GetDifficultyMultiplier();
            
            return contract;
        }
        
        /// <summary>
        /// Генерация контракта исследования
        /// </summary>
        private Contract GenerateExplorationContract(ContractDifficulty difficulty)
        {
            Contract contract = new Contract();
            contract.title = "Исследование";
            contract.description = "Исследуйте указанную область";
            contract.contractType = ContractType.Exploration;
            contract.difficulty = difficulty;
            
            // Цель: исследовать область
            ContractObjective objective = new ContractObjective();
            objective.description = "Исследовать указанную область";
            objective.type = ContractObjective.ObjectiveType.ReachLocation;
            objective.targetValue = 1f;
            contract.objectives.Add(objective);
            
            // Награда
            contract.reward.scrapReward = 150 * (int)contract.GetDifficultyMultiplier();
            contract.reward.experienceReward = 75 * (int)contract.GetDifficultyMultiplier();
            
            return contract;
        }
        
        /// <summary>
        /// Генерация контракта спасения
        /// </summary>
        private Contract GenerateRescueContract(ContractDifficulty difficulty)
        {
            Contract contract = new Contract();
            contract.title = "Спасение";
            contract.description = "Спасите пострадавших";
            contract.contractType = ContractType.Rescue;
            contract.difficulty = difficulty;
            
            // Цель: спасти пострадавших
            ContractObjective objective = new ContractObjective();
            objective.description = "Спасти пострадавших";
            objective.type = ContractObjective.ObjectiveType.ReachLocation;
            objective.targetValue = 1f;
            contract.objectives.Add(objective);
            
            // Награда
            contract.reward.scrapReward = 300 * (int)contract.GetDifficultyMultiplier();
            contract.reward.experienceReward = 150 * (int)contract.GetDifficultyMultiplier();
            
            return contract;
        }
        
        /// <summary>
        /// Получить случайную позицию
        /// </summary>
        private Vector3 GetRandomPosition()
        {
            float x = UnityEngine.Random.Range(-50f, 50f);
            float z = UnityEngine.Random.Range(-50f, 50f);
            return new Vector3(x, 0f, z);
        }
        
        /// <summary>
        /// Принять контракт
        /// </summary>
        public bool AcceptContract(Contract contract)
        {
            if (contract == null || !contract.CanAccept())
            {
                Debug.LogWarning("Cannot accept contract");
                return false;
            }
            
            if (activeContracts.Count >= maxActiveContracts)
            {
                Debug.LogWarning("Maximum active contracts reached");
                return false;
            }
            
            // Начать контракт
            if (contract.StartContract())
            {
                availableContracts.Remove(contract);
                activeContracts.Add(contract);
                
                // Подписаться на события контракта
                contract.OnContractCompleted += OnContractCompletedHandler;
                contract.OnContractFailed += OnContractFailedHandler;
                contract.OnObjectiveCompleted += OnObjectiveCompletedHandler;
                
                OnContractAccepted?.Invoke(contract);
                OnAvailableContractsUpdated?.Invoke(availableContracts);
                OnActiveContractsUpdated?.Invoke(activeContracts);
                
                PlayContractAcceptedSound();
                Debug.Log($"Contract accepted: {contract.title}");
                
                return true;
            }
            
            return false;
        }
        
        /// <summary>
        /// Отменить активный контракт
        /// </summary>
        public bool CancelContract(Contract contract)
        {
            if (contract == null || contract.status != ContractStatus.Active)
            {
                return false;
            }
            
            activeContracts.Remove(contract);
            contract.status = ContractStatus.Failed;
            
            // Отписаться от событий
            contract.OnContractCompleted -= OnContractCompletedHandler;
            contract.OnContractFailed -= OnContractFailedHandler;
            contract.OnObjectiveCompleted -= OnObjectiveCompletedHandler;
            
            OnActiveContractsUpdated?.Invoke(activeContracts);
            Debug.Log($"Contract cancelled: {contract.title}");
            
            return true;
        }
        
        /// <summary>
        /// Обновить активные контракты
        /// </summary>
        private void UpdateActiveContracts()
        {
            foreach (var contract in activeContracts.ToList())
            {
                contract.UpdateContract();
                contract.CheckExpiry();
            }
        }
        
        /// <summary>
        /// Обновить доступные контракты
        /// </summary>
        private void RefreshAvailableContracts()
        {
            // Удалить истекшие контракты
            availableContracts.RemoveAll(c => c.status == ContractStatus.Expired);
            
            // Добавить новые контракты
            int neededContracts = maxAvailableContracts - availableContracts.Count;
            for (int i = 0; i < neededContracts; i++)
            {
                Contract contract = GenerateRandomContract();
                if (contract != null)
                {
                    availableContracts.Add(contract);
                }
            }
            
            lastRefreshTime = Time.time;
            OnAvailableContractsUpdated?.Invoke(availableContracts);
            Debug.Log($"Refreshed available contracts: {availableContracts.Count}");
        }
        
        /// <summary>
        /// Обработчик завершения контракта
        /// </summary>
        private void OnContractCompletedHandler(Contract contract)
        {
            activeContracts.Remove(contract);
            completedContracts.Add(contract);
            
            // Отписаться от событий
            contract.OnContractCompleted -= OnContractCompletedHandler;
            contract.OnContractFailed -= OnContractFailedHandler;
            contract.OnObjectiveCompleted -= OnObjectiveCompletedHandler;
            
            OnContractCompleted?.Invoke(contract);
            OnActiveContractsUpdated?.Invoke(activeContracts);
            
            PlayContractCompletedSound();
            Debug.Log($"Contract completed: {contract.title}");
        }
        
        /// <summary>
        /// Обработчик провала контракта
        /// </summary>
        private void OnContractFailedHandler(Contract contract)
        {
            activeContracts.Remove(contract);
            
            // Отписаться от событий
            contract.OnContractCompleted -= OnContractCompletedHandler;
            contract.OnContractFailed -= OnContractFailedHandler;
            contract.OnObjectiveCompleted -= OnObjectiveCompletedHandler;
            
            OnContractFailed?.Invoke(contract);
            OnActiveContractsUpdated?.Invoke(activeContracts);
            
            PlayContractFailedSound();
            Debug.Log($"Contract failed: {contract.title}");
        }
        
        /// <summary>
        /// Обработчик завершения цели
        /// </summary>
        private void OnObjectiveCompletedHandler(Contract contract, ContractObjective objective)
        {
            OnObjectiveCompleted?.Invoke(contract, objective);
            PlayObjectiveCompletedSound();
            Debug.Log($"Objective completed: {objective.description}");
        }
        
        /// <summary>
        /// Получить доступные контракты
        /// </summary>
        public List<Contract> GetAvailableContracts()
        {
            return new List<Contract>(availableContracts);
        }
        
        /// <summary>
        /// Получить активные контракты
        /// </summary>
        public List<Contract> GetActiveContracts()
        {
            return new List<Contract>(activeContracts);
        }
        
        /// <summary>
        /// Получить завершенные контракты
        /// </summary>
        public List<Contract> GetCompletedContracts()
        {
            return new List<Contract>(completedContracts);
        }
        
        /// <summary>
        /// Получить контракт по ID
        /// </summary>
        public Contract GetContractByID(string contractID)
        {
            Contract contract = availableContracts.Find(c => c.contractID == contractID);
            if (contract != null) return contract;
            
            contract = activeContracts.Find(c => c.contractID == contractID);
            if (contract != null) return contract;
            
            contract = completedContracts.Find(c => c.contractID == contractID);
            return contract;
        }
        
        /// <summary>
        /// Очистить все контракты
        /// </summary>
        public void ClearAllContracts()
        {
            availableContracts.Clear();
            activeContracts.Clear();
            completedContracts.Clear();
            
            OnAvailableContractsUpdated?.Invoke(availableContracts);
            OnActiveContractsUpdated?.Invoke(activeContracts);
            
            Debug.Log("All contracts cleared");
        }
        
        #region Audio Methods
        
        private void PlayContractAcceptedSound()
        {
            if (contractAcceptedSound != null)
            {
                AudioSource.PlayClipAtPoint(contractAcceptedSound, Camera.main.transform.position);
            }
        }
        
        private void PlayContractCompletedSound()
        {
            if (contractCompletedSound != null)
            {
                AudioSource.PlayClipAtPoint(contractCompletedSound, Camera.main.transform.position);
            }
        }
        
        private void PlayContractFailedSound()
        {
            if (contractFailedSound != null)
            {
                AudioSource.PlayClipAtPoint(contractFailedSound, Camera.main.transform.position);
            }
        }
        
        private void PlayObjectiveCompletedSound()
        {
            if (objectiveCompletedSound != null)
            {
                AudioSource.PlayClipAtPoint(objectiveCompletedSound, Camera.main.transform.position);
            }
        }
        
        #endregion
    }
}
