using UnityEngine;
using System.Collections.Generic;
using System;

namespace ScrapArchitect.Gameplay
{
    /// <summary>
    /// Система прогресса игрока - уровни, опыт, достижения
    /// </summary>
    public class PlayerProgress : MonoBehaviour
    {
        [Header("Progress Settings")]
        public int startingLevel = 1;
        public int maxLevel = 50;
        public int startingExperience = 0;
        public float experienceMultiplier = 1.5f; // Множитель опыта для следующего уровня

        [Header("Level Rewards")]
        public int scrapPerLevel = 100;
        public int maxScrapReward = 1000;
        public bool enableLevelRewards = true;

        [Header("Audio Settings")]
        public AudioClip levelUpSound;
        public AudioClip achievementUnlockedSound;
        public AudioClip experienceGainedSound;

        // Текущий прогресс
        private int currentLevel;
        private int currentExperience;
        private int experienceToNextLevel;

        // Достижения
        private Dictionary<string, Achievement> achievements = new Dictionary<string, Achievement>();
        private List<string> unlockedAchievements = new List<string>();

        // Статистика
        private PlayerStats playerStats = new PlayerStats();

        // Events
        public System.Action<int> OnLevelUp;
        public System.Action<int> OnExperienceGained;
        public System.Action<string> OnAchievementUnlocked;
        public System.Action<int> OnScrapRewarded;

        private void Start()
        {
            InitializeProgress();
            InitializeAchievements();
        }

        /// <summary>
        /// Инициализация прогресса
        /// </summary>
        private void InitializeProgress()
        {
            currentLevel = startingLevel;
            currentExperience = startingExperience;
            CalculateExperienceToNextLevel();

            Debug.Log($"Player progress initialized: Level {currentLevel}, XP {currentExperience}");
        }

        /// <summary>
        /// Инициализация достижений
        /// </summary>
        private void InitializeAchievements()
        {
            // Достижения за уровни
            achievements["FirstLevel"] = new Achievement("FirstLevel", "Первый шаг", "Достигните 2 уровня", 1);
            achievements["Level10"] = new Achievement("Level10", "Опытный мастер", "Достигните 10 уровня", 10);
            achievements["Level25"] = new Achievement("Level25", "Ветеран", "Достигните 25 уровня", 25);
            achievements["Level50"] = new Achievement("Level50", "Легенда", "Достигните 50 уровня", 50);

            // Достижения за контракты
            achievements["FirstContract"] = new Achievement("FirstContract", "Первый заказ", "Выполните первый контракт", 1);
            achievements["ContractMaster"] = new Achievement("ContractMaster", "Мастер контрактов", "Выполните 50 контрактов", 50);
            achievements["PerfectContract"] = new Achievement("PerfectContract", "Идеальное выполнение", "Выполните контракт с максимальным рейтингом", 1);

            // Достижения за строительство
            achievements["FirstBuild"] = new Achievement("FirstBuild", "Первая машина", "Создайте первую машину", 1);
            achievements["ComplexBuild"] = new Achievement("ComplexBuild", "Сложная конструкция", "Создайте машину из 20+ деталей", 20);
            achievements["SpeedBuilder"] = new Achievement("SpeedBuilder", "Быстрый строитель", "Создайте машину за 30 секунд", 1);

            // Достижения за экономику
            achievements["FirstPurchase"] = new Achievement("FirstPurchase", "Первая покупка", "Купите первую деталь", 1);
            achievements["RichPlayer"] = new Achievement("RichPlayer", "Богач", "Накопите 10,000 скрапа", 10000);
            achievements["Shopaholic"] = new Achievement("Shopaholic", "Шопоголик", "Купите 100 деталей", 100);

            Debug.Log($"Initialized {achievements.Count} achievements");
        }

        /// <summary>
        /// Получить текущий уровень
        /// </summary>
        public int GetCurrentLevel()
        {
            return currentLevel;
        }

        /// <summary>
        /// Получить текущий опыт
        /// </summary>
        public int GetCurrentExperience()
        {
            return currentExperience;
        }

        /// <summary>
        /// Получить опыт до следующего уровня
        /// </summary>
        public int GetExperienceToNextLevel()
        {
            return experienceToNextLevel;
        }

        /// <summary>
        /// Получить прогресс до следующего уровня (0-1)
        /// </summary>
        public float GetLevelProgress()
        {
            if (currentLevel >= maxLevel) return 1f;
            
            int experienceForCurrentLevel = GetExperienceForLevel(currentLevel);
            int experienceForNextLevel = GetExperienceForLevel(currentLevel + 1);
            int experienceInCurrentLevel = currentExperience - experienceForCurrentLevel;
            int experienceNeeded = experienceForNextLevel - experienceForCurrentLevel;

            return (float)experienceInCurrentLevel / experienceNeeded;
        }

        /// <summary>
        /// Добавить опыт
        /// </summary>
        public void AddExperience(int amount)
        {
            if (amount <= 0 || currentLevel >= maxLevel) return;

            currentExperience += amount;
            OnExperienceGained?.Invoke(amount);

            PlayExperienceGainedSound();
            Debug.Log($"Gained {amount} experience. Total: {currentExperience}");

            // Проверить повышение уровня
            CheckLevelUp();
        }

        /// <summary>
        /// Проверить повышение уровня
        /// </summary>
        private void CheckLevelUp()
        {
            while (currentExperience >= experienceToNextLevel && currentLevel < maxLevel)
            {
                LevelUp();
            }
        }

        /// <summary>
        /// Повысить уровень
        /// </summary>
        private void LevelUp()
        {
            currentLevel++;
            CalculateExperienceToNextLevel();

            // Награды за уровень
            if (enableLevelRewards)
            {
                int scrapReward = Mathf.Min(scrapPerLevel * currentLevel, maxScrapReward);
                EconomyManager economyManager = FindObjectOfType<EconomyManager>();
                if (economyManager != null)
                {
                    economyManager.AddScrap(scrapReward);
                    OnScrapRewarded?.Invoke(scrapReward);
                }
            }

            // Проверить достижения за уровни
            CheckLevelAchievements();

            OnLevelUp?.Invoke(currentLevel);
            PlayLevelUpSound();

            Debug.Log($"Level up! New level: {currentLevel}");
        }

        /// <summary>
        /// Рассчитать опыт до следующего уровня
        /// </summary>
        private void CalculateExperienceToNextLevel()
        {
            experienceToNextLevel = GetExperienceForLevel(currentLevel + 1);
        }

        /// <summary>
        /// Получить опыт для определенного уровня
        /// </summary>
        private int GetExperienceForLevel(int level)
        {
            if (level <= 1) return 0;
            
            // Формула: базовый опыт * (множитель ^ (уровень - 1))
            int baseExperience = 100;
            return Mathf.RoundToInt(baseExperience * Mathf.Pow(experienceMultiplier, level - 1));
        }

        /// <summary>
        /// Проверить достижения за уровни
        /// </summary>
        private void CheckLevelAchievements()
        {
            CheckAchievement("FirstLevel", currentLevel >= 2);
            CheckAchievement("Level10", currentLevel >= 10);
            CheckAchievement("Level25", currentLevel >= 25);
            CheckAchievement("Level50", currentLevel >= 50);
        }

        /// <summary>
        /// Проверить достижение
        /// </summary>
        public void CheckAchievement(string achievementID, bool condition)
        {
            if (!achievements.ContainsKey(achievementID)) return;
            if (unlockedAchievements.Contains(achievementID)) return;

            if (condition)
            {
                UnlockAchievement(achievementID);
            }
        }

        /// <summary>
        /// Разблокировать достижение
        /// </summary>
        private void UnlockAchievement(string achievementID)
        {
            if (!achievements.ContainsKey(achievementID)) return;
            if (unlockedAchievements.Contains(achievementID)) return;

            unlockedAchievements.Add(achievementID);
            OnAchievementUnlocked?.Invoke(achievementID);
            PlayAchievementUnlockedSound();

            Debug.Log($"Achievement unlocked: {achievements[achievementID].title}");
        }

        /// <summary>
        /// Получить информацию о достижении
        /// </summary>
        public Achievement GetAchievement(string achievementID)
        {
            return achievements.ContainsKey(achievementID) ? achievements[achievementID] : null;
        }

        /// <summary>
        /// Получить все достижения
        /// </summary>
        public Dictionary<string, Achievement> GetAllAchievements()
        {
            return new Dictionary<string, Achievement>(achievements);
        }

        /// <summary>
        /// Получить разблокированные достижения
        /// </summary>
        public List<string> GetUnlockedAchievements()
        {
            return new List<string>(unlockedAchievements);
        }

        /// <summary>
        /// Получить статистику игрока
        /// </summary>
        public PlayerStats GetPlayerStats()
        {
            return playerStats;
        }

        /// <summary>
        /// Обновить статистику
        /// </summary>
        public void UpdateStats(string statName, int value = 1)
        {
            playerStats.UpdateStat(statName, value);
        }

        /// <summary>
        /// Получить информацию о прогрессе
        /// </summary>
        public string GetProgressInfo()
        {
            string info = $"Level: {currentLevel}\n";
            info += $"Experience: {currentExperience}/{experienceToNextLevel}\n";
            info += $"Progress: {GetLevelProgress():P1}\n";
            info += $"Achievements: {unlockedAchievements.Count}/{achievements.Count}\n";
            info += $"Stats: {playerStats.GetTotalStats()}";

            return info;
        }

        /// <summary>
        /// Сбросить прогресс (для тестирования)
        /// </summary>
        public void ResetProgress()
        {
            currentLevel = startingLevel;
            currentExperience = startingExperience;
            unlockedAchievements.Clear();
            playerStats.ResetStats();

            CalculateExperienceToNextLevel();
            Debug.Log("Player progress reset");
        }

        #region Audio Methods

        private void PlayLevelUpSound()
        {
            if (levelUpSound != null)
            {
                AudioSource.PlayClipAtPoint(levelUpSound, Camera.main.transform.position);
            }
        }

        private void PlayAchievementUnlockedSound()
        {
            if (achievementUnlockedSound != null)
            {
                AudioSource.PlayClipAtPoint(achievementUnlockedSound, Camera.main.transform.position);
            }
        }

        private void PlayExperienceGainedSound()
        {
            if (experienceGainedSound != null)
            {
                AudioSource.PlayClipAtPoint(experienceGainedSound, Camera.main.transform.position);
            }
        }

        #endregion
    }

    /// <summary>
    /// Класс достижения
    /// </summary>
    [Serializable]
    public class Achievement
    {
        public string id;
        public string title;
        public string description;
        public int requirement;
        public bool isUnlocked;

        public Achievement(string id, string title, string description, int requirement)
        {
            this.id = id;
            this.title = title;
            this.description = description;
            this.requirement = requirement;
            this.isUnlocked = false;
        }
    }

    /// <summary>
    /// Статистика игрока
    /// </summary>
    [Serializable]
    public class PlayerStats
    {
        private Dictionary<string, int> stats = new Dictionary<string, int>();

        public void UpdateStat(string statName, int value = 1)
        {
            if (!stats.ContainsKey(statName))
            {
                stats[statName] = 0;
            }
            stats[statName] += value;
        }

        public int GetStat(string statName)
        {
            return stats.ContainsKey(statName) ? stats[statName] : 0;
        }

        public Dictionary<string, int> GetAllStats()
        {
            return new Dictionary<string, int>(stats);
        }

        public int GetTotalStats()
        {
            int total = 0;
            foreach (var stat in stats.Values)
            {
                total += stat;
            }
            return total;
        }

        public void ResetStats()
        {
            stats.Clear();
        }
    }
}
