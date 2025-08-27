using UnityEngine;
using System;
using System.Collections.Generic;

namespace ScrapArchitect.Gameplay
{
    /// <summary>
    /// Система звездного рейтинга для контрактов
    /// </summary>
    public class StarRatingSystem : MonoBehaviour
    {
        // Singleton pattern
        public static StarRatingSystem Instance { get; private set; }
        
        [Header("Star Rating Settings")]
        public int maxStars = 3;
        public float timeBonusThreshold = 0.8f; // 80% от лимита времени
        public float objectiveBonusThreshold = 0.9f; // 90% выполнения целей
        public float efficiencyBonusThreshold = 0.7f; // 70% эффективности
        
        [Header("Scoring Weights")]
        public float timeWeight = 0.4f;
        public float objectivesWeight = 0.4f;
        public float efficiencyWeight = 0.2f;
        
        [Header("Bonus Multipliers")]
        public float perfectTimeBonus = 1.5f;
        public float allObjectivesBonus = 1.3f;
        public float noDamageBonus = 1.2f;
        public float speedBonus = 1.1f;
        
        private Dictionary<string, int> contractStarRatings = new Dictionary<string, int>();
        private Dictionary<string, float> contractScores = new Dictionary<string, float>();
        
        private void Awake()
        {
            // Singleton pattern
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                InitializeStarRatingSystem();
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
        /// <summary>
        /// Инициализация системы звезд
        /// </summary>
        private void InitializeStarRatingSystem()
        {
            LoadStarRatings();
            Debug.Log("StarRatingSystem initialized");
        }
        
        /// <summary>
        /// Загрузка звездных рейтингов
        /// </summary>
        private void LoadStarRatings()
        {
            string ratingsJson = PlayerPrefs.GetString("ContractStarRatings", "{}");
            if (!string.IsNullOrEmpty(ratingsJson))
            {
                try
                {
                    // TODO: Реализовать JSON десериализацию
                    // contractStarRatings = JsonUtility.FromJson<Dictionary<string, int>>(ratingsJson);
                }
                catch (Exception e)
                {
                    Debug.LogWarning($"Failed to load star ratings: {e.Message}");
                }
            }
        }
        
        /// <summary>
        /// Сохранение звездных рейтингов
        /// </summary>
        private void SaveStarRatings()
        {
            try
            {
                // TODO: Реализовать JSON сериализацию
                // string ratingsJson = JsonUtility.ToJson(contractStarRatings);
                // PlayerPrefs.SetString("ContractStarRatings", ratingsJson);
                // PlayerPrefs.Save();
            }
            catch (Exception e)
            {
                Debug.LogWarning($"Failed to save star ratings: {e.Message}");
            }
        }
        
        /// <summary>
        /// Рассчитать звездный рейтинг для контракта
        /// </summary>
        public StarRating CalculateStarRating(Contract contract, ContractCompletionData completionData)
        {
            if (contract == null || completionData == null)
            {
                return new StarRating { stars = 0, score = 0f };
            }
            
            float totalScore = 0f;
            float maxScore = 100f;
            
            // Оценка по времени (40% от общего балла)
            float timeScore = CalculateTimeScore(contract, completionData);
            totalScore += timeScore * timeWeight;
            
            // Оценка по целям (40% от общего балла)
            float objectivesScore = CalculateObjectivesScore(contract, completionData);
            totalScore += objectivesScore * objectivesWeight;
            
            // Оценка по эффективности (20% от общего балла)
            float efficiencyScore = CalculateEfficiencyScore(contract, completionData);
            totalScore += efficiencyScore * efficiencyWeight;
            
            // Применяем бонусы
            totalScore = ApplyBonuses(totalScore, contract, completionData);
            
            // Определяем количество звезд
            int stars = CalculateStarsFromScore(totalScore, maxScore);
            
            // Сохраняем результат
            string contractKey = contract.contractID;
            contractStarRatings[contractKey] = stars;
            contractScores[contractKey] = totalScore;
            SaveStarRatings();
            
            return new StarRating
            {
                stars = stars,
                score = totalScore,
                maxScore = maxScore,
                timeScore = timeScore,
                objectivesScore = objectivesScore,
                efficiencyScore = efficiencyScore
            };
        }
        
        /// <summary>
        /// Рассчитать оценку по времени
        /// </summary>
        private float CalculateTimeScore(Contract contract, ContractCompletionData completionData)
        {
            if (contract.timeLimit <= 0) return 100f;
            
            float timeRatio = completionData.completionTime / contract.timeLimit;
            
            if (timeRatio <= timeBonusThreshold)
            {
                // Бонус за быстрое выполнение
                return 100f + (1f - timeRatio) * 50f;
            }
            else if (timeRatio <= 1f)
            {
                // Нормальное выполнение
                return 100f - (timeRatio - timeBonusThreshold) * 100f / (1f - timeBonusThreshold);
            }
            else
            {
                // Превышение времени
                return Mathf.Max(0f, 100f - (timeRatio - 1f) * 100f);
            }
        }
        
        /// <summary>
        /// Рассчитать оценку по целям
        /// </summary>
        private float CalculateObjectivesScore(Contract contract, ContractCompletionData completionData)
        {
            if (contract.objectives.Count == 0) return 100f;
            
            float totalObjectiveScore = 0f;
            int completedObjectives = 0;
            
            foreach (var objective in contract.objectives)
            {
                if (objective.isOptional) continue;
                
                float objectiveProgress = objective.currentValue / objective.targetValue;
                float objectiveScore = Mathf.Clamp(objectiveProgress * 100f, 0f, 100f);
                
                totalObjectiveScore += objectiveScore;
                
                if (objectiveProgress >= 1f)
                {
                    completedObjectives++;
                }
            }
            
            float averageScore = totalObjectiveScore / contract.objectives.Count;
            
            // Бонус за выполнение всех целей
            if (completedObjectives == contract.objectives.Count)
            {
                averageScore *= allObjectivesBonus;
            }
            
            return Mathf.Clamp(averageScore, 0f, 100f);
        }
        
        /// <summary>
        /// Рассчитать оценку по эффективности
        /// </summary>
        private float CalculateEfficiencyScore(Contract contract, ContractCompletionData completionData)
        {
            float efficiencyScore = 100f;
            
            // Штраф за повреждения
            if (completionData.damageTaken > 0)
            {
                efficiencyScore -= completionData.damageTaken * 10f;
            }
            
            // Бонус за эффективное использование ресурсов
            if (completionData.resourcesUsed < completionData.resourcesAvailable * 0.5f)
            {
                efficiencyScore += 20f;
            }
            
            // Бонус за минимальное количество деталей
            if (completionData.partsUsed < completionData.recommendedParts * 0.8f)
            {
                efficiencyScore += 15f;
            }
            
            return Mathf.Clamp(efficiencyScore, 0f, 100f);
        }
        
        /// <summary>
        /// Применить бонусы
        /// </summary>
        private float ApplyBonuses(float baseScore, Contract contract, ContractCompletionData completionData)
        {
            float finalScore = baseScore;
            
            // Бонус за идеальное время
            if (completionData.completionTime <= contract.timeLimit * 0.5f)
            {
                finalScore *= perfectTimeBonus;
            }
            
            // Бонус за отсутствие повреждений
            if (completionData.damageTaken == 0)
            {
                finalScore *= noDamageBonus;
            }
            
            // Бонус за скорость
            if (completionData.completionTime < contract.timeLimit * 0.7f)
            {
                finalScore *= speedBonus;
            }
            
            return finalScore;
        }
        
        /// <summary>
        /// Рассчитать количество звезд из балла
        /// </summary>
        private int CalculateStarsFromScore(float score, float maxScore)
        {
            float percentage = score / maxScore;
            
            if (percentage >= 0.9f)
            {
                return 3; // Золотая звезда
            }
            else if (percentage >= 0.7f)
            {
                return 2; // Серебряная звезда
            }
            else if (percentage >= 0.5f)
            {
                return 1; // Бронзовая звезда
            }
            else
            {
                return 0; // Без звезд
            }
        }
        
        /// <summary>
        /// Получить звездный рейтинг контракта
        /// </summary>
        public int GetContractStarRating(string contractID)
        {
            if (contractStarRatings.ContainsKey(contractID))
            {
                return contractStarRatings[contractID];
            }
            return 0;
        }
        
        /// <summary>
        /// Получить балл контракта
        /// </summary>
        public float GetContractScore(string contractID)
        {
            if (contractScores.ContainsKey(contractID))
            {
                return contractScores[contractID];
            }
            return 0f;
        }
        
        /// <summary>
        /// Получить общую статистику звезд
        /// </summary>
        public StarRatingStats GetStarRatingStats()
        {
            int totalContracts = contractStarRatings.Count;
            int totalStars = 0;
            int threeStarContracts = 0;
            int twoStarContracts = 0;
            int oneStarContracts = 0;
            int zeroStarContracts = 0;
            
            foreach (var rating in contractStarRatings.Values)
            {
                totalStars += rating;
                
                switch (rating)
                {
                    case 3:
                        threeStarContracts++;
                        break;
                    case 2:
                        twoStarContracts++;
                        break;
                    case 1:
                        oneStarContracts++;
                        break;
                    case 0:
                        zeroStarContracts++;
                        break;
                }
            }
            
            return new StarRatingStats
            {
                totalContracts = totalContracts,
                totalStars = totalStars,
                averageStars = totalContracts > 0 ? (float)totalStars / totalContracts : 0f,
                threeStarContracts = threeStarContracts,
                twoStarContracts = twoStarContracts,
                oneStarContracts = oneStarContracts,
                zeroStarContracts = zeroStarContracts
            };
        }
        
        /// <summary>
        /// Сбросить все звездные рейтинги
        /// </summary>
        public void ResetAllStarRatings()
        {
            contractStarRatings.Clear();
            contractScores.Clear();
            PlayerPrefs.DeleteKey("ContractStarRatings");
            PlayerPrefs.Save();
        }
        
        [Serializable]
        public class StarRating
        {
            public int stars;
            public float score;
            public float maxScore;
            public float timeScore;
            public float objectivesScore;
            public float efficiencyScore;
        }
        
        [Serializable]
        public class ContractCompletionData
        {
            public float completionTime;
            public float damageTaken;
            public int resourcesUsed;
            public int resourcesAvailable;
            public int partsUsed;
            public int recommendedParts;
            public bool allObjectivesCompleted;
            public bool noDamageTaken;
            public bool perfectTime;
        }
        
        [Serializable]
        public class StarRatingStats
        {
            public int totalContracts;
            public int totalStars;
            public float averageStars;
            public int threeStarContracts;
            public int twoStarContracts;
            public int oneStarContracts;
            public int zeroStarContracts;
        }
    }
}
