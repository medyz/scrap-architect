using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace ScrapArchitect.Gameplay
{
    /// <summary>
    /// Менеджер сложности контрактов
    /// </summary>
    public class DifficultyManager : MonoBehaviour
    {
        // Singleton pattern
        public static DifficultyManager Instance { get; private set; }
        
        [Header("Difficulty Settings")]
        public float baseDifficultyMultiplier = 1f;
        public float difficultyIncreasePerLevel = 0.1f;
        public float maxDifficultyMultiplier = 3f;
        
        [Header("Player Progress")]
        public int playerLevel = 1;
        public int completedContracts = 0;
        public float averageCompletionTime = 0f;
        public float successRate = 1f;
        
        [Header("Dynamic Difficulty")]
        public bool enableDynamicDifficulty = true;
        public float adaptiveSpeed = 0.1f;
        
        private Dictionary<string, float> contractDifficultyModifiers = new Dictionary<string, float>();
        private List<float> recentCompletionTimes = new List<float>();
        private List<bool> recentSuccesses = new List<bool>();
        
        private void Awake()
        {
            // Singleton pattern
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                InitializeDifficultyManager();
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
        /// <summary>
        /// Инициализация менеджера сложности
        /// </summary>
        private void InitializeDifficultyManager()
        {
            LoadPlayerProgress();
            CalculateBaseDifficulty();
            Debug.Log("DifficultyManager initialized");
        }
        
        /// <summary>
        /// Загрузка прогресса игрока
        /// </summary>
        private void LoadPlayerProgress()
        {
            playerLevel = PlayerPrefs.GetInt("PlayerLevel", 1);
            completedContracts = PlayerPrefs.GetInt("CompletedContracts", 0);
            averageCompletionTime = PlayerPrefs.GetFloat("AverageCompletionTime", 0f);
            successRate = PlayerPrefs.GetFloat("SuccessRate", 1f);
        }
        
        /// <summary>
        /// Сохранение прогресса игрока
        /// </summary>
        private void SavePlayerProgress()
        {
            PlayerPrefs.SetInt("PlayerLevel", playerLevel);
            PlayerPrefs.SetInt("CompletedContracts", completedContracts);
            PlayerPrefs.SetFloat("AverageCompletionTime", averageCompletionTime);
            PlayerPrefs.SetFloat("SuccessRate", successRate);
            PlayerPrefs.Save();
        }
        
        /// <summary>
        /// Расчет базовой сложности
        /// </summary>
        private void CalculateBaseDifficulty()
        {
            baseDifficultyMultiplier = 1f + (playerLevel - 1) * difficultyIncreasePerLevel;
            baseDifficultyMultiplier = Mathf.Clamp(baseDifficultyMultiplier, 1f, maxDifficultyMultiplier);
        }
        
        /// <summary>
        /// Получить модификатор сложности для контракта
        /// </summary>
        public float GetContractDifficultyModifier(Contract contract)
        {
            if (contract == null) return 1f;
            
            string contractKey = contract.title;
            
            if (contractDifficultyModifiers.ContainsKey(contractKey))
            {
                return contractDifficultyModifiers[contractKey];
            }
            
            // Рассчитываем модификатор на основе базовой сложности
            float modifier = CalculateContractModifier(contract);
            contractDifficultyModifiers[contractKey] = modifier;
            
            return modifier;
        }
        
        /// <summary>
        /// Расчет модификатора для конкретного контракта
        /// </summary>
        private float CalculateContractModifier(Contract contract)
        {
            float modifier = baseDifficultyMultiplier;
            
            // Модификатор на основе типа контракта
            switch (contract.contractType)
            {
                case ContractType.Delivery:
                    modifier *= 1.0f; // Базовая сложность
                    break;
                case ContractType.Collection:
                    modifier *= 0.9f; // Немного проще
                    break;
                case ContractType.Racing:
                    modifier *= 1.2f; // Сложнее
                    break;
                case ContractType.Transport:
                    modifier *= 1.1f; // Средняя сложность
                    break;
                case ContractType.Construction:
                    modifier *= 1.3f; // Сложнее
                    break;
                case ContractType.Destruction:
                    modifier *= 1.1f; // Средняя сложность
                    break;
                case ContractType.Exploration:
                    modifier *= 0.8f; // Проще
                    break;
                case ContractType.Rescue:
                    modifier *= 1.4f; // Самая сложная
                    break;
            }
            
            // Модификатор на основе сложности контракта
            switch (contract.difficulty)
            {
                case ContractDifficulty.Easy:
                    modifier *= 0.7f;
                    break;
                case ContractDifficulty.Medium:
                    modifier *= 1.0f;
                    break;
                case ContractDifficulty.Hard:
                    modifier *= 1.3f;
                    break;
                case ContractDifficulty.Expert:
                    modifier *= 1.6f;
                    break;
                case ContractDifficulty.Master:
                    modifier *= 2.0f;
                    break;
            }
            
            // Адаптивная сложность на основе успешности игрока
            if (enableDynamicDifficulty)
            {
                modifier *= GetAdaptiveModifier();
            }
            
            return Mathf.Clamp(modifier, 0.5f, 3f);
        }
        
        /// <summary>
        /// Получить адаптивный модификатор
        /// </summary>
        private float GetAdaptiveModifier()
        {
            if (recentSuccesses.Count < 5) return 1f;
            
            float recentSuccessRate = recentSuccesses.TakeLast(5).Count(s => s) / 5f;
            float successDifference = successRate - recentSuccessRate;
            
            // Если игрок часто проигрывает, уменьшаем сложность
            if (successDifference > 0.2f)
            {
                return 0.9f;
            }
            // Если игрок часто выигрывает, увеличиваем сложность
            else if (successDifference < -0.2f)
            {
                return 1.1f;
            }
            
            return 1f;
        }
        
        /// <summary>
        /// Применить сложность к контракту
        /// </summary>
        public void ApplyDifficultyToContract(Contract contract)
        {
            if (contract == null) return;
            
            float modifier = GetContractDifficultyModifier(contract);
            
            // Модифицируем время выполнения
            if (contract.timeLimit > 0)
            {
                contract.timeLimit *= modifier;
            }
            
            // Модифицируем цели
            foreach (var objective in contract.objectives)
            {
                if (objective.targetValue > 0)
                {
                    objective.targetValue *= modifier;
                }
            }
            
            // Модифицируем награды
            contract.reward.scrapReward = Mathf.RoundToInt(contract.reward.scrapReward * modifier);
            contract.reward.experienceReward = Mathf.RoundToInt(contract.reward.experienceReward * modifier);
        }
        
        /// <summary>
        /// Зарегистрировать завершение контракта
        /// </summary>
        public void RegisterContractCompletion(Contract contract, float completionTime, bool success)
        {
            if (contract == null) return;
            
            completedContracts++;
            
            // Обновляем статистику
            recentCompletionTimes.Add(completionTime);
            recentSuccesses.Add(success);
            
            // Ограничиваем размер списков
            if (recentCompletionTimes.Count > 10)
            {
                recentCompletionTimes.RemoveAt(0);
            }
            if (recentSuccesses.Count > 10)
            {
                recentSuccesses.RemoveAt(0);
            }
            
            // Обновляем средние показатели
            UpdateAverageMetrics();
            
            // Проверяем повышение уровня
            CheckLevelUp();
            
            // Сохраняем прогресс
            SavePlayerProgress();
        }
        
        /// <summary>
        /// Обновление средних метрик
        /// </summary>
        private void UpdateAverageMetrics()
        {
            if (recentCompletionTimes.Count > 0)
            {
                averageCompletionTime = recentCompletionTimes.Average();
            }
            
            if (recentSuccesses.Count > 0)
            {
                successRate = recentSuccesses.Count(s => s) / (float)recentSuccesses.Count;
            }
        }
        
        /// <summary>
        /// Проверка повышения уровня
        /// </summary>
        private void CheckLevelUp()
        {
            int requiredContracts = playerLevel * 5; // 5 контрактов на уровень
            
            if (completedContracts >= requiredContracts)
            {
                playerLevel++;
                CalculateBaseDifficulty();
                
                Debug.Log($"Игрок достиг уровня {playerLevel}!");
                
                // Уведомляем другие системы о повышении уровня
                OnPlayerLevelUp?.Invoke(playerLevel);
            }
        }
        
        /// <summary>
        /// Получить рекомендуемую сложность для игрока
        /// </summary>
        public ContractDifficulty GetRecommendedDifficulty()
        {
            if (successRate > 0.8f && averageCompletionTime < 60f)
            {
                return ContractDifficulty.Hard;
            }
            else if (successRate > 0.6f && averageCompletionTime < 120f)
            {
                return ContractDifficulty.Medium;
            }
            else
            {
                return ContractDifficulty.Easy;
            }
        }
        
        /// <summary>
        /// Получить статистику сложности
        /// </summary>
        public DifficultyStats GetDifficultyStats()
        {
            return new DifficultyStats
            {
                playerLevel = playerLevel,
                completedContracts = completedContracts,
                averageCompletionTime = averageCompletionTime,
                successRate = successRate,
                baseDifficultyMultiplier = baseDifficultyMultiplier,
                recommendedDifficulty = GetRecommendedDifficulty()
            };
        }
        
        /// <summary>
        /// Сбросить прогресс сложности
        /// </summary>
        public void ResetDifficultyProgress()
        {
            playerLevel = 1;
            completedContracts = 0;
            averageCompletionTime = 0f;
            successRate = 1f;
            recentCompletionTimes.Clear();
            recentSuccesses.Clear();
            contractDifficultyModifiers.Clear();
            
            CalculateBaseDifficulty();
            SavePlayerProgress();
        }
        
        // Events
        public System.Action<int> OnPlayerLevelUp;
        
        [System.Serializable]
        public class DifficultyStats
        {
            public int playerLevel;
            public int completedContracts;
            public float averageCompletionTime;
            public float successRate;
            public float baseDifficultyMultiplier;
            public ContractDifficulty recommendedDifficulty;
        }
    }
}
