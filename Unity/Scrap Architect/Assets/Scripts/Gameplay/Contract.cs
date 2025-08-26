using UnityEngine;
using System;
using System.Collections.Generic;
using ScrapArchitect.Physics;
using ScrapArchitect.Parts;

namespace ScrapArchitect.Gameplay
{
    /// <summary>
    /// Типы контрактов
    /// </summary>
    public enum ContractType
    {
        Delivery,       // Доставка груза
        Transport,      // Перевозка пассажиров
        Construction,   // Строительство
        Collection,     // Сбор предметов
        Destruction,    // Разрушение объектов
        Racing,         // Гонки
        Exploration,    // Исследование
        Rescue          // Спасение
    }
    
    /// <summary>
    /// Сложность контракта
    /// </summary>
    public enum ContractDifficulty
    {
        Easy,           // Легкий
        Medium,         // Средний
        Hard,           // Сложный
        Expert,         // Эксперт
        Master          // Мастер
    }
    
    /// <summary>
    /// Состояние контракта
    /// </summary>
    public enum ContractStatus
    {
        Available,      // Доступен
        Active,         // Активен
        Completed,      // Завершен
        Failed,         // Провален
        Expired         // Истек
    }
    
    /// <summary>
    /// Цель контракта
    /// </summary>
    [Serializable]
    public class ContractObjective
    {
        public string objectiveID;
        public string description;
        public ObjectiveType type;
        public float targetValue;
        public float currentValue;
        public bool isCompleted;
        public bool isOptional;
        
        public enum ObjectiveType
        {
            ReachLocation,       // Достичь локации
            CollectItems,        // Собрать предметы
            DeliverItems,        // Доставить предметы
            DestroyObjects,      // Уничтожить объекты
            BuildStructure,      // Построить структуру
            TravelDistance,      // Проехать расстояние
            ReachSpeed,          // Достичь скорости
            SurviveTime,         // Выжить время
            UseTool,             // Использовать инструмент
            ConnectParts         // Соединить детали
        }
        
        public ContractObjective()
        {
            objectiveID = Guid.NewGuid().ToString();
            isCompleted = false;
            isOptional = false;
        }
        
        /// <summary>
        /// Обновить прогресс цели
        /// </summary>
        public void UpdateProgress(float value)
        {
            currentValue = Mathf.Min(currentValue + value, targetValue);
            
            if (currentValue >= targetValue && !isCompleted)
            {
                isCompleted = true;
                Debug.Log($"Objective completed: {description}");
            }
        }
        
        /// <summary>
        /// Получить прогресс в процентах
        /// </summary>
        public float GetProgressPercentage()
        {
            return Mathf.Clamp01(currentValue / targetValue);
        }
        
        /// <summary>
        /// Сбросить прогресс
        /// </summary>
        public void ResetProgress()
        {
            currentValue = 0f;
            isCompleted = false;
        }
    }
    
    /// <summary>
    /// Награда за контракт
    /// </summary>
    [Serializable]
    public class ContractReward
    {
        public int scrapReward;          // Награда в скрапе
        public int experienceReward;     // Опыт
        public List<string> unlockParts; // Разблокируемые детали
        public List<string> unlockTools; // Разблокируемые инструменты
        public float reputationGain;     // Прирост репутации
        
        public ContractReward()
        {
            unlockParts = new List<string>();
            unlockTools = new List<string>();
        }
        
        /// <summary>
        /// Применить награду
        /// </summary>
        public void ApplyReward()
        {
            // В будущем интегрировать с системой прогресса
            Debug.Log($"Reward applied: {scrapReward} scrap, {experienceReward} XP");
        }
    }
    
    /// <summary>
    /// Основной класс контракта
    /// </summary>
    [Serializable]
    public class Contract
    {
        [Header("Contract Info")]
        public string contractID;
        public string title;
        public string description;
        public ContractType contractType;
        public ContractDifficulty difficulty;
        public ContractStatus status;
        
        [Header("Requirements")]
        public int requiredLevel;
        public List<string> requiredParts;
        public List<string> requiredTools;
        public float minVehicleMass;
        public float maxVehicleMass;
        public int minParts;
        public int maxParts;
        
        [Header("Objectives")]
        public List<ContractObjective> objectives;
        public float timeLimit;          // Время на выполнение (0 = без ограничений)
        public float startTime;
        
        [Header("Rewards")]
        public ContractReward reward;
        public ContractReward bonusReward; // Бонус за быстрое выполнение
        
        [Header("Location")]
        public Vector3 startLocation;
        public Vector3[] checkpoints;
        public Vector3 finishLocation;
        
        [Header("Metadata")]
        public string clientName;
        public string clientDescription;
        public DateTime creationDate;
        public DateTime expiryDate;
        public List<string> tags;
        
        // Events
        public Action<Contract> OnContractStarted;
        public Action<Contract> OnContractCompleted;
        public Action<Contract> OnContractFailed;
        public Action<Contract, ContractObjective> OnObjectiveCompleted;
        public Action<Contract> OnContractExpired;
        
        public Contract()
        {
            contractID = Guid.NewGuid().ToString();
            status = ContractStatus.Available;
            objectives = new List<ContractObjective>();
            requiredParts = new List<string>();
            requiredTools = new List<string>();
            tags = new List<string>();
            reward = new ContractReward();
            bonusReward = new ContractReward();
            creationDate = DateTime.Now;
        }
        
        /// <summary>
        /// Начать контракт
        /// </summary>
        public bool StartContract()
        {
            if (status != ContractStatus.Available)
            {
                Debug.LogWarning($"Cannot start contract {title}: status is {status}");
                return false;
            }
            
            // Проверить требования
            if (!CheckRequirements())
            {
                Debug.LogWarning($"Cannot start contract {title}: requirements not met");
                return false;
            }
            
            status = ContractStatus.Active;
            startTime = Time.time;
            
            // Сбросить прогресс всех целей
            foreach (var objective in objectives)
            {
                objective.ResetProgress();
            }
            
            OnContractStarted?.Invoke(this);
            Debug.Log($"Contract started: {title}");
            
            return true;
        }
        
        /// <summary>
        /// Проверить требования контракта
        /// </summary>
        private bool CheckRequirements()
        {
            // Проверить уровень игрока
            // if (PlayerProgress.Instance.GetLevel() < requiredLevel)
            //     return false;
            
            // Проверить доступные детали
            // foreach (string requiredPart in requiredParts)
            // {
            //     if (!PlayerProgress.Instance.HasPart(requiredPart))
            //         return false;
            // }
            
            // Проверить доступные инструменты
            // foreach (string requiredTool in requiredTools)
            // {
            //     if (!PlayerProgress.Instance.HasTool(requiredTool))
            //         return false;
            // }
            
            return true;
        }
        
        /// <summary>
        /// Обновить контракт
        /// </summary>
        public void UpdateContract()
        {
            if (status != ContractStatus.Active)
                return;
            
            // Проверить истечение времени
            if (timeLimit > 0 && Time.time - startTime > timeLimit)
            {
                FailContract("Time limit exceeded");
                return;
            }
            
            // Проверить завершение всех обязательных целей
            bool allRequiredCompleted = true;
            foreach (var objective in objectives)
            {
                if (!objective.isOptional && !objective.isCompleted)
                {
                    allRequiredCompleted = false;
                    break;
                }
            }
            
            if (allRequiredCompleted)
            {
                CompleteContract();
            }
        }
        
        /// <summary>
        /// Обновить прогресс цели
        /// </summary>
        public void UpdateObjective(string objectiveID, float progress)
        {
            ContractObjective objective = objectives.Find(o => o.objectiveID == objectiveID);
            if (objective != null)
            {
                objective.UpdateProgress(progress);
                
                if (objective.isCompleted)
                {
                    OnObjectiveCompleted?.Invoke(this, objective);
                }
            }
        }
        
        /// <summary>
        /// Завершить контракт
        /// </summary>
        public void CompleteContract()
        {
            if (status != ContractStatus.Active)
                return;
            
            status = ContractStatus.Completed;
            
            // Применить награду
            reward.ApplyReward();
            
            // Проверить бонус за быстрое выполнение
            if (bonusReward != null && timeLimit > 0)
            {
                float completionTime = Time.time - startTime;
                float timeRatio = completionTime / timeLimit;
                
                if (timeRatio < 0.5f) // Завершили за половину времени
                {
                    bonusReward.ApplyReward();
                    Debug.Log($"Bonus reward applied for fast completion!");
                }
            }
            
            OnContractCompleted?.Invoke(this);
            Debug.Log($"Contract completed: {title}");
        }
        
        /// <summary>
        /// Провалить контракт
        /// </summary>
        public void FailContract(string reason = "Unknown")
        {
            if (status != ContractStatus.Active)
                return;
            
            status = ContractStatus.Failed;
            
            OnContractFailed?.Invoke(this);
            Debug.Log($"Contract failed: {title} - {reason}");
        }
        
        /// <summary>
        /// Проверить истечение срока
        /// </summary>
        public void CheckExpiry()
        {
            if (status == ContractStatus.Available && DateTime.Now > expiryDate)
            {
                status = ContractStatus.Expired;
                OnContractExpired?.Invoke(this);
                Debug.Log($"Contract expired: {title}");
            }
        }
        
        /// <summary>
        /// Получить прогресс контракта
        /// </summary>
        public float GetProgress()
        {
            if (objectives.Count == 0)
                return 0f;
            
            float totalProgress = 0f;
            int completedObjectives = 0;
            
            foreach (var objective in objectives)
            {
                if (objective.isCompleted)
                {
                    completedObjectives++;
                }
                totalProgress += objective.GetProgressPercentage();
            }
            
            return totalProgress / objectives.Count;
        }
        
        /// <summary>
        /// Получить оставшееся время
        /// </summary>
        public float GetRemainingTime()
        {
            if (timeLimit <= 0)
                return -1f;
            
            float elapsed = Time.time - startTime;
            return Mathf.Max(0f, timeLimit - elapsed);
        }
        
        /// <summary>
        /// Получить информацию о контракте
        /// </summary>
        public string GetContractInfo()
        {
            string info = $"Title: {title}\n";
            info += $"Type: {contractType}\n";
            info += $"Difficulty: {difficulty}\n";
            info += $"Status: {status}\n";
            info += $"Progress: {GetProgress():P0}\n";
            
            if (timeLimit > 0)
            {
                float remaining = GetRemainingTime();
                if (remaining >= 0)
                {
                    info += $"Time remaining: {remaining:F1}s\n";
                }
            }
            
            info += $"Reward: {reward.scrapReward} scrap, {reward.experienceReward} XP\n";
            
            return info;
        }
        
        /// <summary>
        /// Проверить, можно ли принять контракт
        /// </summary>
        public bool CanAccept()
        {
            return status == ContractStatus.Available && CheckRequirements();
        }
        
        /// <summary>
        /// Получить сложность в числовом виде
        /// </summary>
        public int GetDifficultyValue()
        {
            switch (difficulty)
            {
                case ContractDifficulty.Easy: return 1;
                case ContractDifficulty.Medium: return 2;
                case ContractDifficulty.Hard: return 3;
                case ContractDifficulty.Expert: return 4;
                case ContractDifficulty.Master: return 5;
                default: return 1;
            }
        }
        
        /// <summary>
        /// Получить множитель награды за сложность
        /// </summary>
        public float GetDifficultyMultiplier()
        {
            return GetDifficultyValue() * 0.5f + 0.5f; // 1.0 - 3.0
        }
        
        /// <summary>
        /// Создать простой контракт доставки
        /// </summary>
        public static Contract CreateDeliveryContract(string title, Vector3 start, Vector3 finish, float distance)
        {
            Contract contract = new Contract();
            contract.title = title;
            contract.description = $"Доставьте груз из точки A в точку B. Расстояние: {distance:F0}m";
            contract.contractType = ContractType.Delivery;
            contract.difficulty = ContractDifficulty.Easy;
            contract.startLocation = start;
            contract.finishLocation = finish;
            
            // Цель: достичь финиша
            ContractObjective objective = new ContractObjective();
            objective.description = "Достичь точки назначения";
            objective.type = ContractObjective.ObjectiveType.ReachLocation;
            objective.targetValue = 1f;
            contract.objectives.Add(objective);
            
            // Награда
            contract.reward.scrapReward = Mathf.RoundToInt(distance * 10f);
            contract.reward.experienceReward = Mathf.RoundToInt(distance * 5f);
            
            return contract;
        }
        
        /// <summary>
        /// Создать контракт сбора
        /// </summary>
        public static Contract CreateCollectionContract(string title, int itemCount, string itemType)
        {
            Contract contract = new Contract();
            contract.title = title;
            contract.description = $"Соберите {itemCount} {itemType}";
            contract.contractType = ContractType.Collection;
            contract.difficulty = ContractDifficulty.Medium;
            
            // Цель: собрать предметы
            ContractObjective objective = new ContractObjective();
            objective.description = $"Собрать {itemCount} {itemType}";
            objective.type = ContractObjective.ObjectiveType.CollectItems;
            objective.targetValue = itemCount;
            contract.objectives.Add(objective);
            
            // Награда
            contract.reward.scrapReward = itemCount * 50;
            contract.reward.experienceReward = itemCount * 25;
            
            return contract;
        }
        
        /// <summary>
        /// Создать контракт гонки
        /// </summary>
        public static Contract CreateRacingContract(string title, Vector3[] checkpoints, float timeLimit)
        {
            Contract contract = new Contract();
            contract.title = title;
            contract.description = $"Пройдите трассу за {timeLimit:F0} секунд";
            contract.contractType = ContractType.Racing;
            contract.difficulty = ContractDifficulty.Hard;
            contract.timeLimit = timeLimit;
            contract.checkpoints = checkpoints;
            
            // Цель: пройти все чекпоинты
            for (int i = 0; i < checkpoints.Length; i++)
            {
                ContractObjective objective = new ContractObjective();
                objective.description = $"Достичь чекпоинта {i + 1}";
                objective.type = ContractObjective.ObjectiveType.ReachLocation;
                objective.targetValue = 1f;
                contract.objectives.Add(objective);
            }
            
            // Награда
            contract.reward.scrapReward = Mathf.RoundToInt(timeLimit * 20f);
            contract.reward.experienceReward = Mathf.RoundToInt(timeLimit * 10f);
            
            return contract;
        }
    }
}
