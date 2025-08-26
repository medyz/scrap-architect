using UnityEngine;
using ScrapArchitect.Economy;

namespace ScrapArchitect.Gameplay
{
    /// <summary>
    /// Интеграция системы прогресса с другими системами игры
    /// </summary>
    public class ProgressIntegration : MonoBehaviour
    {
        [Header("Experience Rewards")]
        public int contractCompletionXP = 50;
        public int contractPerfectXP = 100;
        public int vehicleBuiltXP = 25;
        public int partPurchasedXP = 5;
        public int achievementUnlockedXP = 25;

        [Header("Level Unlocks")]
        public bool enableLevelUnlocks = true;
        public int[] levelUnlockThresholds = { 1, 5, 10, 15, 20, 25, 30, 35, 40, 45, 50 };

        private PlayerProgress playerProgress;
        private EconomyManager economyManager;
        private ContractManager contractManager;

        private void Start()
        {
            InitializeIntegration();
        }

        /// <summary>
        /// Инициализация интеграции
        /// </summary>
        private void InitializeIntegration()
        {
            playerProgress = FindObjectOfType<PlayerProgress>();
            economyManager = FindObjectOfType<EconomyManager>();
            contractManager = FindObjectOfType<ContractManager>();

            if (playerProgress == null)
            {
                Debug.LogError("PlayerProgress not found for integration!");
                return;
            }

            // Подписаться на события контрактов
            if (contractManager != null)
            {
                contractManager.OnContractCompleted += OnContractCompleted;
                contractManager.OnContractFailed += OnContractFailed;
            }

            // Подписаться на события экономики
            if (economyManager != null)
            {
                economyManager.OnItemPurchased += OnItemPurchased;
                economyManager.OnScrapChanged += OnScrapChanged;
            }

            // Подписаться на события прогресса
            playerProgress.OnLevelUp += OnLevelUp;
            playerProgress.OnAchievementUnlocked += OnAchievementUnlocked;

            Debug.Log("Progress integration initialized");
        }

        #region Contract Integration

        /// <summary>
        /// Обработчик завершения контракта
        /// </summary>
        private void OnContractCompleted(Contract contract, float rating)
        {
            if (playerProgress == null) return;

            // Опыт за завершение контракта
            int xpReward = contractCompletionXP;
            
            // Бонус за идеальное выполнение
            if (rating >= 0.9f)
            {
                xpReward += contractPerfectXP;
                playerProgress.CheckAchievement("PerfectContract", true);
            }

            playerProgress.AddExperience(xpReward);

            // Обновить статистику
            playerProgress.UpdateStats("ContractsCompleted");
            playerProgress.UpdateStats("ScrapEarned", contract.reward.scrapAmount);

            // Проверить достижения
            CheckContractAchievements();

            Debug.Log($"Contract completed! XP gained: {xpReward}, Rating: {rating:P0}");
        }

        /// <summary>
        /// Обработчик провала контракта
        /// </summary>
        private void OnContractFailed(Contract contract)
        {
            if (playerProgress == null) return;

            // Небольшой опыт даже за провал
            int xpReward = Mathf.RoundToInt(contractCompletionXP * 0.1f);
            playerProgress.AddExperience(xpReward);

            Debug.Log($"Contract failed! XP gained: {xpReward}");
        }

        /// <summary>
        /// Проверить достижения за контракты
        /// </summary>
        private void CheckContractAchievements()
        {
            if (playerProgress == null) return;

            int contractsCompleted = playerProgress.GetPlayerStats().GetStat("ContractsCompleted");
            
            playerProgress.CheckAchievement("FirstContract", contractsCompleted >= 1);
            playerProgress.CheckAchievement("ContractMaster", contractsCompleted >= 50);
        }

        #endregion

        #region Economy Integration

        /// <summary>
        /// Обработчик покупки предмета
        /// </summary>
        private void OnItemPurchased(string itemID, int cost)
        {
            if (playerProgress == null) return;

            // Опыт за покупку
            playerProgress.AddExperience(partPurchasedXP);

            // Обновить статистику
            playerProgress.UpdateStats("PartsPurchased");

            // Проверить достижения
            CheckEconomyAchievements();

            Debug.Log($"Item purchased! XP gained: {partPurchasedXP}");
        }

        /// <summary>
        /// Обработчик изменения скрапа
        /// </summary>
        private void OnScrapChanged(int newAmount)
        {
            if (playerProgress == null) return;

            // Проверить достижения за богатство
            playerProgress.CheckAchievement("RichPlayer", newAmount >= 10000);
        }

        /// <summary>
        /// Проверить достижения за экономику
        /// </summary>
        private void CheckEconomyAchievements()
        {
            if (playerProgress == null) return;

            int partsPurchased = playerProgress.GetPlayerStats().GetStat("PartsPurchased");
            
            playerProgress.CheckAchievement("FirstPurchase", partsPurchased >= 1);
            playerProgress.CheckAchievement("Shopaholic", partsPurchased >= 100);
        }

        #endregion

        #region Building Integration

        /// <summary>
        /// Обработчик создания машины
        /// </summary>
        public void OnVehicleBuilt(int partCount, float buildTime)
        {
            if (playerProgress == null) return;

            // Опыт за создание машины
            int xpReward = vehicleBuiltXP + (partCount * 2); // Бонус за количество деталей
            playerProgress.AddExperience(xpReward);

            // Обновить статистику
            playerProgress.UpdateStats("VehiclesBuilt");
            playerProgress.UpdateStats("PartsUsed", partCount);

            // Проверить достижения
            CheckBuildingAchievements(partCount, buildTime);

            Debug.Log($"Vehicle built! XP gained: {xpReward}, Parts: {partCount}");
        }

        /// <summary>
        /// Проверить достижения за строительство
        /// </summary>
        private void CheckBuildingAchievements(int partCount, float buildTime)
        {
            if (playerProgress == null) return;

            int vehiclesBuilt = playerProgress.GetPlayerStats().GetStat("VehiclesBuilt");
            
            playerProgress.CheckAchievement("FirstBuild", vehiclesBuilt >= 1);
            playerProgress.CheckAchievement("ComplexBuild", partCount >= 20);
            playerProgress.CheckAchievement("SpeedBuilder", buildTime <= 30f);
        }

        #endregion

        #region Progress Integration

        /// <summary>
        /// Обработчик повышения уровня
        /// </summary>
        private void OnLevelUp(int newLevel)
        {
            // Разблокировать новые детали по уровню
            if (enableLevelUnlocks && economyManager != null)
            {
                UnlockPartsForLevel(newLevel);
            }

            Debug.Log($"Level up! New level: {newLevel}");
        }

        /// <summary>
        /// Обработчик разблокировки достижения
        /// </summary>
        private void OnAchievementUnlocked(string achievementID)
        {
            if (playerProgress == null) return;

            // Опыт за достижение
            playerProgress.AddExperience(achievementUnlockedXP);

            // Обновить статистику
            playerProgress.UpdateStats("AchievementsUnlocked");

            Debug.Log($"Achievement unlocked! XP gained: {achievementUnlockedXP}");
        }

        #endregion

        /// <summary>
        /// Разблокировать детали для определенного уровня
        /// </summary>
        private void UnlockPartsForLevel(int level)
        {
            if (economyManager == null) return;

            // Определить какие детали разблокировать для уровня
            string[] partsToUnlock = GetPartsForLevel(level);

            foreach (string partID in partsToUnlock)
            {
                economyManager.UnlockItem(partID);
                Debug.Log($"Unlocked {partID} for level {level}");
            }
        }

        /// <summary>
        /// Получить детали для определенного уровня
        /// </summary>
        private string[] GetPartsForLevel(int level)
        {
            return level switch
            {
                1 => new string[] { "WoodBlock", "SmallWheel", "ElectricMotor", "FixedJoint", "BasicSeat" },
                2 => new string[] { "MetalBlock", "MediumWheel", "Hammer" },
                3 => new string[] { "PlasticBlock", "GasolineMotor", "HingeJoint" },
                4 => new string[] { "StoneBlock", "LargeWheel", "Wrench" },
                5 => new string[] { "OffRoadWheel", "DieselMotor", "SpringJoint", "ComfortSeat" },
                6 => new string[] { "JetMotor", "SliderJoint", "Screwdriver" },
                7 => new string[] { "ConfigurableJoint", "RacingSeat", "Drill" },
                8 => new string[] { "LuxurySeat", "Welder" },
                9 => new string[] { "IndustrialSeat", "Magnet" },
                10 => new string[] { "Vacuum", "Sprayer" },
                _ => new string[] { }
            };
        }

        /// <summary>
        /// Получить информацию об интеграции
        /// </summary>
        public string GetIntegrationInfo()
        {
            if (playerProgress == null) return "Integration not initialized";

            string info = "Progress Integration Info:\n";
            info += $"Current Level: {playerProgress.GetCurrentLevel()}\n";
            info += $"Experience: {playerProgress.GetCurrentExperience()}\n";
            info += $"Contracts Completed: {playerProgress.GetPlayerStats().GetStat("ContractsCompleted")}\n";
            info += $"Vehicles Built: {playerProgress.GetPlayerStats().GetStat("VehiclesBuilt")}\n";
            info += $"Parts Purchased: {playerProgress.GetPlayerStats().GetStat("PartsPurchased")}\n";
            info += $"Achievements: {playerProgress.GetUnlockedAchievements().Count}";

            return info;
        }

        /// <summary>
        /// Добавить опыт (публичный метод для внешних систем)
        /// </summary>
        public void AddExperience(int amount)
        {
            if (playerProgress != null)
            {
                playerProgress.AddExperience(amount);
            }
        }

        /// <summary>
        /// Обновить статистику (публичный метод для внешних систем)
        /// </summary>
        public void UpdateStat(string statName, int value = 1)
        {
            if (playerProgress != null)
            {
                playerProgress.UpdateStats(statName, value);
            }
        }

        /// <summary>
        /// Проверить достижение (публичный метод для внешних систем)
        /// </summary>
        public void CheckAchievement(string achievementID, bool condition)
        {
            if (playerProgress != null)
            {
                playerProgress.CheckAchievement(achievementID, condition);
            }
        }

        private void OnDestroy()
        {
            // Отписаться от событий
            if (contractManager != null)
            {
                contractManager.OnContractCompleted -= OnContractCompleted;
                contractManager.OnContractFailed -= OnContractFailed;
            }

            if (economyManager != null)
            {
                economyManager.OnItemPurchased -= OnItemPurchased;
                economyManager.OnScrapChanged -= OnScrapChanged;
            }

            if (playerProgress != null)
            {
                playerProgress.OnLevelUp -= OnLevelUp;
                playerProgress.OnAchievementUnlocked -= OnAchievementUnlocked;
            }
        }
    }
}
