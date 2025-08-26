using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

namespace ScrapArchitect.UI
{
    /// <summary>
    /// Экран загрузки
    /// </summary>
    public class LoadingScreen : UIBase
    {
        [Header("Loading Elements")]
        public TextMeshProUGUI loadingText;
        public TextMeshProUGUI progressText;
        public Slider progressSlider;
        public Image loadingIcon;
        
        [Header("Animation")]
        public float iconRotationSpeed = 90f;
        public float textPulseSpeed = 2f;
        public string[] loadingMessages = {
            "Загрузка...",
            "Подготовка уровня...",
            "Инициализация физики...",
            "Загрузка контрактов...",
            "Готово!"
        };
        
        private float currentProgress = 0f;
        private int currentMessageIndex = 0;
        private bool isLoading = false;
        
        private void Start()
        {
            SetupLoadingScreen();
        }
        
        /// <summary>
        /// Настройка экрана загрузки
        /// </summary>
        private void SetupLoadingScreen()
        {
            if (progressSlider != null)
            {
                progressSlider.minValue = 0f;
                progressSlider.maxValue = 1f;
                progressSlider.value = 0f;
            }
            
            if (loadingText != null)
            {
                loadingText.text = loadingMessages[0];
            }
            
            if (progressText != null)
            {
                progressText.text = "0%";
            }
        }
        
        /// <summary>
        /// Вызывается при показе экрана загрузки
        /// </summary>
        protected override void OnShow()
        {
            base.OnShow();
            
            StartLoading();
        }
        
        /// <summary>
        /// Начать загрузку
        /// </summary>
        public void StartLoading()
        {
            isLoading = true;
            currentProgress = 0f;
            currentMessageIndex = 0;
            
            StartCoroutine(LoadingAnimation());
        }
        
        /// <summary>
        /// Анимация загрузки
        /// </summary>
        private IEnumerator LoadingAnimation()
        {
            float totalDuration = 3f; // Общее время загрузки
            float elapsedTime = 0f;
            
            while (elapsedTime < totalDuration && isLoading)
            {
                elapsedTime += Time.unscaledDeltaTime;
                float progress = elapsedTime / totalDuration;
                
                SetProgress(progress);
                
                // Обновление сообщений
                int messageIndex = Mathf.FloorToInt(progress * (loadingMessages.Length - 1));
                if (messageIndex != currentMessageIndex && messageIndex < loadingMessages.Length)
                {
                    currentMessageIndex = messageIndex;
                    UpdateLoadingMessage();
                }
                
                yield return null;
            }
            
            // Завершение загрузки
            SetProgress(1f);
            currentMessageIndex = loadingMessages.Length - 1;
            UpdateLoadingMessage();
            
            yield return new WaitForSecondsRealtime(0.5f);
            
            isLoading = false;
        }
        
        /// <summary>
        /// Установить прогресс загрузки
        /// </summary>
        public void SetProgress(float progress)
        {
            currentProgress = Mathf.Clamp01(progress);
            
            if (progressSlider != null)
            {
                progressSlider.value = currentProgress;
            }
            
            if (progressText != null)
            {
                progressText.text = $"{Mathf.RoundToInt(currentProgress * 100)}%";
            }
        }
        
        /// <summary>
        /// Обновить сообщение загрузки
        /// </summary>
        private void UpdateLoadingMessage()
        {
            if (loadingText != null && currentMessageIndex < loadingMessages.Length)
            {
                loadingText.text = loadingMessages[currentMessageIndex];
            }
        }
        
        /// <summary>
        /// Остановить загрузку
        /// </summary>
        public void StopLoading()
        {
            isLoading = false;
        }
        
        private void Update()
        {
            // Вращение иконки загрузки
            if (loadingIcon != null && isLoading)
            {
                loadingIcon.transform.Rotate(0f, 0f, iconRotationSpeed * Time.unscaledDeltaTime);
            }
            
            // Пульсация текста
            if (loadingText != null && isLoading)
            {
                float alpha = 0.5f + 0.5f * Mathf.Sin(Time.unscaledTime * textPulseSpeed);
                Color color = loadingText.color;
                color.a = alpha;
                loadingText.color = color;
            }
        }
        
        /// <summary>
        /// Получить текущий прогресс
        /// </summary>
        public float GetProgress()
        {
            return currentProgress;
        }
        
        /// <summary>
        /// Проверить, завершена ли загрузка
        /// </summary>
        public bool IsLoadingComplete()
        {
            return !isLoading && currentProgress >= 1f;
        }
    }
}
