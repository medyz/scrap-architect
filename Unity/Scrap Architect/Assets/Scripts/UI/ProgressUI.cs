using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using ScrapArchitect.Gameplay;

namespace ScrapArchitect.UI
{
    /// <summary>
    /// UI для отображения прогресса игрока
    /// </summary>
    public class ProgressUI : MonoBehaviour
    {
        [Header("Level Display")]
        public TextMeshProUGUI levelText;
        public TextMeshProUGUI experienceText;
        public Slider progressBar;
        public Image progressBarFill;

        [Header("Achievements")]
        public GameObject achievementPanel;
        public Transform achievementContent;
        public GameObject achievementItemPrefab;
        public TextMeshProUGUI achievementCountText;

        [Header("Stats")]
        public GameObject statsPanel;
        public Transform statsContent;
        public GameObject statItemPrefab;
        public TextMeshProUGUI totalStatsText;

        [Header("Visual Settings")]
        public Color progressBarColor = Color.green;
        public Color levelUpColor = Color.yellow;
        public Color achievementColor = Color.orange;

        [Header("Animation Settings")]
        public float animationDuration = 0.5f;
        public bool enableAnimations = true;

        private PlayerProgress playerProgress;
        private bool isInitialized = false;

        private void Start()
        {
            InitializeUI();
        }

        /// <summary>
        /// Инициализация UI
        /// </summary>
        private void InitializeUI()
        {
            playerProgress = FindObjectOfType<PlayerProgress>();
            if (playerProgress == null)
            {
                Debug.LogError("PlayerProgress not found!");
                return;
            }

            // Подписаться на события
            playerProgress.OnLevelUp += OnLevelUp;
            playerProgress.OnExperienceGained += OnExperienceGained;
            playerProgress.OnAchievementUnlocked += OnAchievementUnlocked;

            UpdateLevelDisplay();
            UpdateAchievementsDisplay();
            UpdateStatsDisplay();

            isInitialized = true;
            Debug.Log("Progress UI initialized");
        }

        /// <summary>
        /// Обновить отображение уровня
        /// </summary>
        private void UpdateLevelDisplay()
        {
            if (!isInitialized || playerProgress == null) return;

            int currentLevel = playerProgress.GetCurrentLevel();
            int currentExp = playerProgress.GetCurrentExperience();
            int expToNext = playerProgress.GetExperienceToNextLevel();
            float progress = playerProgress.GetLevelProgress();

            // Обновить текст
            if (levelText != null)
            {
                levelText.text = $"Уровень {currentLevel}";
            }

            if (experienceText != null)
            {
                experienceText.text = $"{currentExp} / {expToNext} XP";
            }

            // Обновить прогресс-бар
            if (progressBar != null)
            {
                if (enableAnimations)
                {
                    StartCoroutine(AnimateProgressBar(progressBar.value, progress));
                }
                else
                {
                    progressBar.value = progress;
                }
            }

            // Обновить цвет прогресс-бара
            if (progressBarFill != null)
            {
                progressBarFill.color = progressBarColor;
            }
        }

        /// <summary>
        /// Обновить отображение достижений
        /// </summary>
        private void UpdateAchievementsDisplay()
        {
            if (!isInitialized || playerProgress == null) return;

            // Очистить существующие элементы
            if (achievementContent != null)
            {
                foreach (Transform child in achievementContent)
                {
                    Destroy(child.gameObject);
                }
            }

            // Получить все достижения
            var allAchievements = playerProgress.GetAllAchievements();
            var unlockedAchievements = playerProgress.GetUnlockedAchievements();

            // Создать элементы достижений
            foreach (var achievement in allAchievements.Values)
            {
                if (achievementItemPrefab != null && achievementContent != null)
                {
                    GameObject item = Instantiate(achievementItemPrefab, achievementContent);
                    AchievementItem achievementItem = item.GetComponent<AchievementItem>();

                    if (achievementItem != null)
                    {
                        bool isUnlocked = unlockedAchievements.Contains(achievement.id);
                        achievementItem.Initialize(achievement, isUnlocked);
                    }
                }
            }

            // Обновить счетчик достижений
            if (achievementCountText != null)
            {
                achievementCountText.text = $"{unlockedAchievements.Count} / {allAchievements.Count}";
            }
        }

        /// <summary>
        /// Обновить отображение статистики
        /// </summary>
        private void UpdateStatsDisplay()
        {
            if (!isInitialized || playerProgress == null) return;

            // Очистить существующие элементы
            if (statsContent != null)
            {
                foreach (Transform child in statsContent)
                {
                    Destroy(child.gameObject);
                }
            }

            // Получить статистику
            var stats = playerProgress.GetPlayerStats().GetAllStats();

            // Создать элементы статистики
            foreach (var stat in stats)
            {
                if (statItemPrefab != null && statsContent != null)
                {
                    GameObject item = Instantiate(statItemPrefab, statsContent);
                    StatItem statItem = item.GetComponent<StatItem>();

                    if (statItem != null)
                    {
                        statItem.Initialize(stat.Key, stat.Value);
                    }
                }
            }

            // Обновить общую статистику
            if (totalStatsText != null)
            {
                int totalStats = playerProgress.GetPlayerStats().GetTotalStats();
                totalStatsText.text = $"Всего: {totalStats}";
            }
        }

        /// <summary>
        /// Показать панель достижений
        /// </summary>
        public void ShowAchievementsPanel()
        {
            if (achievementPanel != null)
            {
                achievementPanel.SetActive(true);
                UpdateAchievementsDisplay();
            }
        }

        /// <summary>
        /// Скрыть панель достижений
        /// </summary>
        public void HideAchievementsPanel()
        {
            if (achievementPanel != null)
            {
                achievementPanel.SetActive(false);
            }
        }

        /// <summary>
        /// Показать панель статистики
        /// </summary>
        public void ShowStatsPanel()
        {
            if (statsPanel != null)
            {
                statsPanel.SetActive(true);
                UpdateStatsDisplay();
            }
        }

        /// <summary>
        /// Скрыть панель статистики
        /// </summary>
        public void HideStatsPanel()
        {
            if (statsPanel != null)
            {
                statsPanel.SetActive(false);
            }
        }

        /// <summary>
        /// Переключить панель достижений
        /// </summary>
        public void ToggleAchievementsPanel()
        {
            if (achievementPanel != null)
            {
                bool isActive = achievementPanel.activeSelf;
                if (isActive)
                {
                    HideAchievementsPanel();
                }
                else
                {
                    ShowAchievementsPanel();
                }
            }
        }

        /// <summary>
        /// Переключить панель статистики
        /// </summary>
        public void ToggleStatsPanel()
        {
            if (statsPanel != null)
            {
                bool isActive = statsPanel.activeSelf;
                if (isActive)
                {
                    HideStatsPanel();
                }
                else
                {
                    ShowStatsPanel();
                }
            }
        }

        #region Event Handlers

        private void OnLevelUp(int newLevel)
        {
            UpdateLevelDisplay();
            
            // Анимация повышения уровня
            if (levelText != null && enableAnimations)
            {
                StartCoroutine(AnimateLevelUp());
            }
        }

        private void OnExperienceGained(int amount)
        {
            UpdateLevelDisplay();
        }

        private void OnAchievementUnlocked(string achievementID)
        {
            UpdateAchievementsDisplay();
            
            // Показать уведомление о достижении
            ShowAchievementNotification(achievementID);
        }

        #endregion

        /// <summary>
        /// Показать уведомление о достижении
        /// </summary>
        private void ShowAchievementNotification(string achievementID)
        {
            Achievement achievement = playerProgress.GetAchievement(achievementID);
            if (achievement != null)
            {
                Debug.Log($"🎉 Достижение разблокировано: {achievement.title} - {achievement.description}");
                
                // Здесь можно добавить всплывающее уведомление
                // ShowPopupNotification($"Достижение: {achievement.title}", achievement.description);
            }
        }

        /// <summary>
        /// Анимация прогресс-бара
        /// </summary>
        private IEnumerator AnimateProgressBar(float fromValue, float toValue)
        {
            float elapsed = 0f;
            
            while (elapsed < animationDuration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / animationDuration;
                progressBar.value = Mathf.Lerp(fromValue, toValue, t);
                yield return null;
            }
            
            progressBar.value = toValue;
        }

        /// <summary>
        /// Анимация повышения уровня
        /// </summary>
        private IEnumerator AnimateLevelUp()
        {
            Color originalColor = levelText.color;
            Vector3 originalScale = levelText.transform.localScale;
            
            // Увеличить и изменить цвет
            levelText.color = levelUpColor;
            levelText.transform.localScale = originalScale * 1.2f;
            
            yield return new WaitForSeconds(0.3f);
            
            // Вернуть к исходному состоянию
            levelText.color = originalColor;
            levelText.transform.localScale = originalScale;
        }

        /// <summary>
        /// Обновить UI (вызывается извне)
        /// </summary>
        public void RefreshUI()
        {
            UpdateLevelDisplay();
            UpdateAchievementsDisplay();
            UpdateStatsDisplay();
        }

        private void OnDestroy()
        {
            if (playerProgress != null)
            {
                playerProgress.OnLevelUp -= OnLevelUp;
                playerProgress.OnExperienceGained -= OnExperienceGained;
                playerProgress.OnAchievementUnlocked -= OnAchievementUnlocked;
            }
        }
    }

    /// <summary>
    /// UI элемент для отображения достижения
    /// </summary>
    public class AchievementItem : MonoBehaviour
    {
        [Header("UI References")]
        public TextMeshProUGUI titleText;
        public TextMeshProUGUI descriptionText;
        public Image iconImage;
        public GameObject unlockedIcon;
        public GameObject lockedIcon;

        [Header("Visual Settings")]
        public Color unlockedColor = Color.green;
        public Color lockedColor = Color.gray;

        public void Initialize(Achievement achievement, bool isUnlocked)
        {
            if (titleText != null)
            {
                titleText.text = achievement.title;
                titleText.color = isUnlocked ? unlockedColor : lockedColor;
            }

            if (descriptionText != null)
            {
                descriptionText.text = achievement.description;
                descriptionText.color = isUnlocked ? unlockedColor : lockedColor;
            }

            if (unlockedIcon != null)
            {
                unlockedIcon.SetActive(isUnlocked);
            }

            if (lockedIcon != null)
            {
                lockedIcon.SetActive(!isUnlocked);
            }

            if (iconImage != null)
            {
                iconImage.color = isUnlocked ? unlockedColor : lockedColor;
            }
        }
    }

    /// <summary>
    /// UI элемент для отображения статистики
    /// </summary>
    public class StatItem : MonoBehaviour
    {
        [Header("UI References")]
        public TextMeshProUGUI statNameText;
        public TextMeshProUGUI statValueText;

        public void Initialize(string statName, int value)
        {
            if (statNameText != null)
            {
                statNameText.text = GetLocalizedStatName(statName);
            }

            if (statValueText != null)
            {
                statValueText.text = value.ToString();
            }
        }

        private string GetLocalizedStatName(string statName)
        {
            return statName switch
            {
                "ContractsCompleted" => "Контрактов выполнено",
                "VehiclesBuilt" => "Машин построено",
                "PartsUsed" => "Деталей использовано",
                "ScrapEarned" => "Скрапа заработано",
                "TimePlayed" => "Время игры",
                "AchievementsUnlocked" => "Достижений разблокировано",
                _ => statName
            };
        }
    }
}
